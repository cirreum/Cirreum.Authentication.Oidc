namespace Cirreum.Authentication.Configuration;

using Cirreum.AuthenticationProvider.Configuration;

/// <summary>
/// Root settings for the generic OIDC authorization provider.
/// </summary>
public class OidcAuthenticationSettings
	: AuthenticationProviderSettings<OidcAuthenticationInstanceSettings>;
