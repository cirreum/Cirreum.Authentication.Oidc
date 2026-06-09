# Cirreum Authentication - OIDC

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Authentication.Oidc.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Authentication.Oidc/)
[![License](https://img.shields.io/badge/license-MIT-F2F2F2?style=flat-square&labelColor=1F1F1F)](https://github.com/cirreum/Cirreum.Authentication.Oidc/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Generic OIDC authentication scheme for the Cirreum framework**

> **Migrating from `Cirreum.Authorization.Oidc`?** Renamed successor. See [`docs/MIGRATION-v1.md`](docs/MIGRATION-v1.md).

## Overview

**Cirreum.Authentication.Oidc** is the generic OIDC authentication scheme — validates JWTs from any OIDC-compliant issuer using standard `AddJwtBearer` (Web API) or `AddOpenIdConnect` (Web App), no vendor SDK. Suitable for Auth0, Okta, Ping, Descope, generic OIDC providers, and any IdP that publishes a `.well-known/openid-configuration`.

For Entra-specific integration, use [`Cirreum.Authentication.Entra`](https://github.com/cirreum/Cirreum.Authentication.Entra) which wraps Microsoft.Identity.Web.

## Installation

```bash
dotnet add package Cirreum.Authentication.Oidc
```

## Configuration

```json
{
  "Cirreum": {
    "Authentication": {
      "Providers": {
        "Oidc": {
          "Instances": {
            "auth0": {
              "Enabled": true,
              "Authority": "https://myapp.auth0.com",
              "Audience": "https://api.myapp.com",
              "RequiredScopes": ["read:data", "write:data"]
            },
            "okta": {
              "Enabled": true,
              "Authority": "https://myapp.okta.com/oauth2/default",
              "Audience": "api://default"
            }
          }
        }
      }
    }
  }
}
```

## What's wired

- **Web API** — `AddJwtBearer(...)` with `Authority` + `Audience` validation, JWKS auto-discovery
- **Web App** — `AddOpenIdConnect(...)` with Auth Code Flow + PKCE, cookie sign-in
- **Modern claim mapping** — `roles` + `name` claims (no legacy short-name → Microsoft URI mapping)
- **Scope enforcement** — `RequiredScopes` checked against `scp` / `scope` claims via `JwtBearerEvents.OnTokenValidated`
- **Configuration override** — instance section bound to options; strongly-typed values re-pinned authoritatively

## License

MIT — see [LICENSE](LICENSE).

---

**Cirreum Foundation Framework**
*Layered simplicity for modern .NET*
