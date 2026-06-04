# Cirreum.Authentication.Oidc 1.0.0 — Renamed home for generic OIDC

## Why this release exists

Generic OIDC bearer + OpenID Connect — `AddJwtBearer` and `AddOpenIdConnect` wired through the Cirreum provider framework — is authentication content. The **Cirreum 1.0 Foundation Reset** moves it from the deprecated `Cirreum.Authorization.Oidc` to its correct home under the Authentication pillar.

## What's new

This is a rename release. The OIDC validation pipeline is unchanged.

## What's preserved

- Standard `AddJwtBearer(...)` for Web API
- Standard `AddOpenIdConnect(...)` for Web App (Auth Code + PKCE; no implicit / hybrid)
- Scope validation against `scp` / `scope` claims
- Modern OIDC claim mapping (`name`, `roles`; no legacy short-name → MS URI mapping)
- `Authority` + `Audience` strongly-typed override of the bound configuration section

## Compatibility

- **.NET 10.0** target.
- **Cirreum.Providers 1.2.0+**.
- **Cirreum.AuthenticationProvider 1.0.0+**.
- Apps migrating from `Cirreum.Authorization.Oidc` follow [`MIGRATION-v1.md`](MIGRATION-v1.md).

## See also

- [`MIGRATION-v1.md`](MIGRATION-v1.md), [`CHANGELOG.md`](CHANGELOG.md)
