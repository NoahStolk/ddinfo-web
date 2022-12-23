# Types libraries

There are currently two types libraries:
- `Types.Web`
- `Types.Core`

These projects contain enums which propagate throughout the entire code base.

For both types libraries, the reason for their separation is that we do not want to reference any core or web project in the API projects, but we do want to make the enums shared instead of duplicating them in every specification.

## Types.Web

Types.Web contains all the enums used by the API and the web projects (server, server domain, and client). Core libraries cannot depend on Types.Web.

## Types.Core

Types.Core contains all the enums used by the core libraries. These are usually types defined by the internals of Devil Daggers itself, rather than DevilDaggersInfo. Custom types may also be present.

## Enum rules

### Changing

Changing enum values must never be done. These values may be stored in the database or used in the API.

When adding an enum member, be sure to check all consuming libraries.

### Consuming

#### Apps

When using enums in an app, be sure to implement fallback behavior in case a new enum value is added to the API. Throwing exceptions should be avoided.
