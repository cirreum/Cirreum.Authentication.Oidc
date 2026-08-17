# Cirreum.Authentication.Oidc 1.1.0 — OIDC declares its people; the cookie registers once

## Why this release exists

The attribute-authority model has providers declare what kind of party they authenticate — which
matters most for OIDC, since an application that owns its users' attributes issues deliberately
thin tokens, and thin tokens are what the old inference misread as machine callers. OIDC schemes
validate tokens issued to people; this release says so, per instance, through the registration
funnel — and fixes a latent Web App composition bug found on the way.

## What's new

**`SubjectKind.Human`, contributed per instance.** The audience registrar base declares every
OIDC instance beside its audience-routing registration. The instance's optional `ClaimAuthority`
block — the declaration an application owning its users' profile and roles writes — rides the
same contribution.

**The Web App cookie scheme registers once per host, and is declared.** The cookie session
scheme keeps the platform-default name (`CookieAuthenticationDefaults.AuthenticationScheme`) for
sign-in interop, and its registration is now guarded: previously each interactive instance called
`AddCookie` again, so a second Web App instance failed at startup with an opaque
scheme-already-exists error. One cookie scheme per host, shared by every interactive instance,
declared `SubjectKind.Unknown` — a continuation re-presenting the subject the OIDC sign-in
established.

## Compatibility

- **Applications have nothing to change.** Instance configuration and composition are untouched;
  single-instance Web App hosts behave identically, and multi-instance Web App hosts now compose
  instead of throwing.
- **Registrar hooks changed signature** per the `Cirreum.AuthenticationProvider` 3.0.x contract
  consolidation. Framework-invoked members no application calls directly; shipped as a Minor
  with that scope stated deliberately.
- The declarations are read by higher-layer packages releasing later in the same wave; until
  then they change no behavior.

## See also

- `Cirreum.AuthenticationProvider 3.0.1` — the registration funnel.
- `Cirreum.Kernel 2.1.0` — the `SubjectKind` / `ClaimAuthority` vocabulary.
