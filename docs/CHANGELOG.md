# Cirreum.Authentication.Oidc Changelog

All notable changes to **Cirreum.Authentication.Oidc** are documented in this file.

Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) — [SemVer](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added

- Initial release. Generic OIDC authentication scheme of the Cirreum framework, established as part of the **Cirreum 1.0 Foundation Reset** wave.
- **Renamed and re-homed from the deprecated `Cirreum.Authorization.Oidc`** following the Three Security Pillars separation. The scheme content was always authentication (JWT bearer validation via `AddJwtBearer` + OpenID Connect via `AddOpenIdConnect`); only the package name was misclassified.
- Surface preserved from 1.0.x of the predecessor package:
  - `OidcAuthenticationRegistrar` extends `AudienceAuthenticationProviderRegistrar` (renamed from `AudienceAuthorizationProviderRegistrar` in the migration)
  - `OidcAuthenticationInstanceSettings` with `Authority` + `RequiredScopes`
  - `OidcAuthenticationSettings` collection
  - Web API (JWT bearer) and Web App (OIDC code+PKCE) wiring
  - Scope validation against `scp` / `scope` claims
- Audience-claim dispatch via the dynamic forward resolver — no `ISchemeSelector` needed for generic OIDC since dispatch is JWT-audience-based.

### Migration

Apps consuming `Cirreum.Authorization.Oidc` migrate by installing `Cirreum.Authentication.Oidc` and switching their composition root from `AddAuthorization(...)` to `AddAuthentication(...)`. See [`docs/MIGRATION-v1.md`](MIGRATION-v1.md).
