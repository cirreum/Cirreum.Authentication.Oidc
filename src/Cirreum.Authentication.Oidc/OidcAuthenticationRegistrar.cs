namespace Cirreum.Authentication;

using Cirreum.AuthenticationProvider;
using Cirreum.Authentication.Configuration;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

/// <summary>
/// Registrar for generic OIDC authorization provider instances.
/// Validates JWTs from any OIDC-compliant issuer using standard <c>AddJwtBearer()</c>.
/// No vendor-specific SDK dependency.
/// </summary>
public sealed class OidcAuthenticationRegistrar
	: AudienceAuthenticationProviderRegistrar<
		OidcAuthenticationSettings,
		OidcAuthenticationInstanceSettings> {

	/// <inheritdoc/>
	public override string ProviderName => "Oidc";

	/// <inheritdoc/>
	public override void ValidateSettings(OidcAuthenticationInstanceSettings settings) {

		if (string.IsNullOrWhiteSpace(settings.Authority)) {
			throw new InvalidOperationException(
				$"OIDC provider instance '{settings.Scheme}' requires an Authority.");
		}

	}

	/// <inheritdoc/>
	public override void AddAuthenticationForWebApi(IConfigurationSection instanceSection,
		OidcAuthenticationInstanceSettings providerSettings,
		AuthenticationBuilder authBuilder) {
		authBuilder.AddJwtBearer(providerSettings.Scheme, options => {

			// Secure default — consumer can override via config if needed (e.g. dev tunnels).
			options.RequireHttpsMetadata = true;

			// Opinionated defaults for modern OIDC. These align with the standard OIDC core
			// claims (`name`, `roles`) and disable the legacy short-name → Microsoft URI
			// mapping. Consumers can override any of these through the config section below
			// (e.g., RoleClaimType = "groups" for Okta, or a namespaced claim for Auth0).
			options.MapInboundClaims = false;
			options.TokenValidationParameters.RoleClaimType = "roles";
			options.TokenValidationParameters.NameClaimType = "name";

			// Bind the instance configuration section to JwtBearerOptions. This lets
			// consumers override any JWT validation setting from appsettings, including
			// nested TokenValidationParameters properties such as ClockSkew, NameClaimType,
			// RoleClaimType, and ValidAlgorithms.
			instanceSection.Bind(options);

			// Re-pin required values from the strongly-typed settings to ensure they're
			// authoritative even if the section used different naming conventions.
			options.Authority = providerSettings.Authority;
			options.Audience = providerSettings.Audience;

			if (providerSettings.RequiredScopes is { Count: > 0 }) {
				var requiredScopes = providerSettings.RequiredScopes;
				options.Events = new JwtBearerEvents {
					OnTokenValidated = context => {
						ValidateRequiredScopes(context.Principal, requiredScopes, context);
						return Task.CompletedTask;
					}
				};
			}
		});
	}

	/// <inheritdoc/>
	public override void AddAuthenticationForWebApp(IConfigurationSection instanceSection,
		OidcAuthenticationInstanceSettings providerSettings,
		AuthenticationBuilder authBuilder) {
		authBuilder.AddCookie("Cookies");
		authBuilder.AddOpenIdConnect(providerSettings.Scheme, options => {

			options.RequireHttpsMetadata = true;

			// Modern OIDC web app defaults — Auth Code + PKCE, no implicit, no hybrid.
			options.ResponseType = "code";
			options.UsePkce = true;
			options.ResponseMode = "query";

			// Persist id/access/refresh tokens in the auth cookie so downstream code
			// can call APIs without re-prompting. Consumers can disable via config.
			options.SaveTokens = true;

			// Standard OIDC scopes — additive; RequiredScopes appended below.
			options.Scope.Add("openid");
			options.Scope.Add("profile");

			// Sign user into the cookie scheme; sign challenges out of it.
			options.SignInScheme = "Cookies";
			options.SignOutScheme = "Cookies";

			// Claim mapping — same as WebApi.
			options.MapInboundClaims = false;
			options.TokenValidationParameters.RoleClaimType = "roles";
			options.TokenValidationParameters.NameClaimType = "name";

			// Bind config last so consumers can override anything above.
			instanceSection.Bind(options);

			// Re-pin authoritative values.
			options.Authority = providerSettings.Authority;
			options.ClientId = providerSettings.Audience;

			if (providerSettings.RequiredScopes is { Count: > 0 }) {
				foreach (var scope in providerSettings.RequiredScopes) {
					if (!options.Scope.Contains(scope)) {
						options.Scope.Add(scope);
					}
				}
			}
		});
	}

	private static void ValidateRequiredScopes(
		ClaimsPrincipal? principal,
		IEnumerable<string> requiredScopes,
		TokenValidatedContext context) {

		if (principal is null) {
			context.Fail("No principal available for scope validation.");
			return;
		}

		// OIDC issuers use either "scp" (space-delimited) or "scope" claim
		var tokenScopes = principal.FindAll("scp")
			.Concat(principal.FindAll("scope"))
			.SelectMany(c => c.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
			.ToHashSet(StringComparer.OrdinalIgnoreCase);
		if (tokenScopes.Count == 0) {
			context.Fail("Token does not contain a scope claim.");
			return;
		}

		foreach (var required in requiredScopes) {
			if (!tokenScopes.Contains(required)) {
				context.Fail($"Token is missing required scope '{required}'.");
				return;
			}
		}

	}

}