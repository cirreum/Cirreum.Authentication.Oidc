# Cirreum.Authentication.Oidc Changelog

All notable changes to **Cirreum.Authentication.Oidc** are documented in this file.

Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) — [SemVer](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

## [1.1.1] - 2026-08-19

### Updated

- Updated NuGet packages.

## [1.1.0] - 2026-08-17

### Added

- **Declares `SubjectKind.Human`.** OIDC schemes validate tokens issued to people, so nothing
  downstream has to infer it from whether a token happens to carry a name claim — which matters
  most here, since an application that owns its users' attributes issues deliberately thin tokens.
  The declaration is contributed per instance by the audience registrar base (the registration
  funnel, `Cirreum.AuthenticationProvider` 3.0.1).
- **The Web App cookie scheme is registered once per host and declared.** The cookie session
  scheme keeps the platform-default name for sign-in interop, is now registered once regardless
  of instance count (a second interactive instance previously re-registered it and failed at
  startup), and declares `SubjectKind.Unknown` — a continuation re-presenting the subject the
  OIDC sign-in established.

### Changed

- Registrar hooks take `IAuthenticationBuilder` per the `Cirreum.AuthenticationProvider` 3.0.1
  contract consolidation. Registrar plumbing only; not app-facing surface.

### Updated

- Updated NuGet packages.

## [1.0.11] - 2026-08-04

### Updated

- Updated NuGet packages (Cirreum spine 4.2.0 wave: `Cirreum.Contracts` 4.2.0 / `Cirreum.Domain` 4.2.0 and current patch releases).

## [1.0.10] - 2026-07-31

### Updated

- Updated NuGet packages (Cirreum spine 4.0.1 wave: `Cirreum.Contracts` 4.0.1 / `Cirreum.Domain` 4.0.1 / `Cirreum.Kernel` 2.0.1 / `Cirreum.AuthenticationProvider` 2.0.3).

## [1.0.9] - 2026-07-29

### Updated

- Updated NuGet packages.

## [1.0.8] - 2026-07-27

### Updated

- Updated NuGet packages.

## [1.0.7] - 2026-07-24

### Updated

- Updated NuGet packages.

## [1.0.6] - 2026-07-20

### Updated

- Updated NuGet packages.

## [1.0.5] - 2026-07-19

### Updated

- Updated NuGet packages.

## [1.0.1] - 2026-07-04

### Updated

- Updated NuGet packages.

## [1.0.0] - 2026-07-03

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
