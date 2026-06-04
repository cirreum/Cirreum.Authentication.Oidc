# Migration to Cirreum.Authentication.Oidc v1.0

**From:** `Cirreum.Authorization.Oidc 1.0.x` (now deprecated)
**To:** `Cirreum.Authentication.Oidc 1.0.0`

## Why v1

Generic OIDC bearer/OpenID Connect validation is unambiguously authentication — `AddJwtBearer` and `AddOpenIdConnect` validate JWTs and run the auth-code flow. The package was misclassified under the Authorization pillar in its 1.0.x life; the **Cirreum 1.0 Foundation Reset** moves it to the Authentication pillar.

## Breaking Changes — Find/Replace Table

| Before | After |
|---|---|
| `<PackageReference Include="Cirreum.Authorization.Oidc" ... />` | `<PackageReference Include="Cirreum.Authentication.Oidc" ... />` |
| `OidcAuthorizationRegistrar` | `OidcAuthenticationRegistrar` |
| `OidcAuthorizationInstanceSettings` | `OidcAuthenticationInstanceSettings` |
| `OidcAuthorizationSettings` | `OidcAuthenticationSettings` |
| `AddAuthorization(authz => authz.AddOidc(...))` | `AddAuthentication(auth => auth.AddOidc(...))` |
| `Cirreum:Authorization:Providers:Oidc:Instances:{name}` | `Cirreum:Authentication:Providers:Oidc:Instances:{name}` |

## What Didn't Change

- JWT validation behavior (`Authority`, `Audience`, scope claims, etc.)
- Web API vs Web App branching (Web App uses Code Flow + PKCE)
- Required-scopes enforcement via `JwtBearerEvents.OnTokenValidated`
- Token validation parameters (modern OIDC defaults: `roles` / `name` claim mapping)
- Token persistence in the auth cookie for Web App scenarios

## Migration Walkthrough

1. **Update `<PackageReference>`** in your csproj.
2. Apply the find/replace table above.
3. **Update `appsettings.json`** configuration root.
4. **Move the `AddOidc` call** from `AddAuthorization(...)` to `AddAuthentication(...)`.
5. Rebuild and verify.
