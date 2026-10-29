# Provider Assignment – Completed Solution

This repository implements the take-home assignment using ASP.NET Core MVC (.NET 8), Razor Views, Entity Framework Core, and SQLite. The solution focuses on maintainability, production-support concerns, and a defensible soft-delete design.

## Implemented requirements

- Provider listing with provider-name/county search and status filtering.
- Create and edit providers.
- Provider soft delete only; no physical delete path is exposed.
- Provider and license records include audit timestamps and deletion metadata.
- License create, edit, and soft delete from a provider detail page.
- Required scenario queries:
  - active providers with active licenses;
  - active providers with expired licenses;
  - exclusion of soft-deleted providers from standard results.
- Read-only operational dashboard backed by database queries.
- Seeded SQLite database with representative active, pending, inactive, expired, and soft-deleted data.
- SQL schema and scenario scripts included under `Scripts/`.

## Architecture

The application is intentionally small and conventional:

- `Models/` – Provider and License domain entities.
- `Data/` – EF Core DbContext and deterministic startup seed.
- `Services/` – application/data-access orchestration and dashboard queries.
- `Controllers/` – HTTP endpoints and MVC composition.
- `ViewModels/` – request/view-specific models to avoid binding domain entities directly.
- `Views/` – Razor UI.
- `Scripts/` – SQLite schema and scenario SQL.

This keeps controllers thin and makes the business/data rules easy to locate and explain.

## Soft-delete design

Hard deletion is prohibited by the assignment, so the implementation uses two complementary protections:

1. `IsDeleted` and `DeletedDate` are persisted on both Providers and Licenses.
2. EF Core global query filters (`HasQueryFilter`) hide deleted records from ordinary queries.
3. `ApplicationDbContext.SaveChanges` / `SaveChangesAsync` intercept `EntityState.Deleted` and convert it into an update that sets `IsDeleted = true` and `DeletedDate`.
4. Standard application queries therefore cannot accidentally display deleted records, while audit/support code can use `IgnoreQueryFilters()` to inspect historical rows.
5. The provider-license relationship uses `DeleteBehavior.Restrict`, preventing database-level cascading hard deletes.

This is deliberately defense-in-depth: the UI does not expose hard delete, the service performs soft delete, EF filters normal reads, and the DbContext prevents accidental physical deletion through `Remove()`.

## Database design decisions

SQLite is used as required. Integer primary keys are simple and efficient for this small internal application. `ProviderName` is indexed for search; license `(ProviderId, LicenseNumber)` is unique to prevent duplicate license numbers for the same provider; expiration dates are indexed for operational queries.

`CreatedDate`, `UpdatedDate`, and `DeletedDate` are stored in UTC. The UI converts the provider creation/update timestamps to local time for display.

Provider status and license status remain strings rather than enums because the assignment describes business-managed lifecycle values and explicitly gives example values. This keeps the database flexible without hiding business values behind numeric enum storage.

## Running locally

Prerequisites:

- .NET 8 SDK

From the repository root:

```bash
dotnet restore
dotnet run
```

The app creates `provider-assignment.db` on first startup if it does not exist and seeds representative records. If the included database is present, startup preserves it and does not reseed.

For a clean local database, delete `provider-assignment.db` and run the app again.

## Database scripts

- `Scripts/001_InitialSchema.sql` – SQLite schema and indexes.
- `Scripts/002_RequiredScenarios.sql` – SQL equivalents of the required business scenarios and an audit query.

The application uses EF Core for normal runtime data access; the scripts are included so the physical database design and required query patterns are explicit and reviewable.

## Included database

`provider-assignment.db` is included in the repository with seeded records so the required scenarios can be inspected immediately.

The seed intentionally contains:

- active providers with valid active licenses;
- active providers with expired licenses;
- pending/inactive providers;
- a soft-deleted provider that should not appear in standard screens.

## Assumptions and trade-offs

- Authentication/authorization is outside the assignment scope; all users are treated as authorized internal staff.
- Provider status and license status are validated against the small set of values presented by the UI rather than introducing lookup tables.
- Licenses are also soft-deleted. This is a conservative extension of the organization's no-hard-delete rule and avoids losing license history.
- The dashboard is implemented in Razor rather than adding a separate React build pipeline because the React dashboard is explicitly optional and the assignment prioritizes backend/database correctness.
- `EnsureCreated` keeps the take-home easy to run. In a production application with ongoing schema evolution, EF migrations would be preferable.

## What I would improve with more time

- Add a full automated test project with integration tests against SQLite for the soft-delete invariant and required scenario queries.
- Add authorization/audit-user fields (e.g., DeletedBy) once an identity provider is defined.
- Replace free-text status values with reference data or strongly validated domain types if the business vocabulary is stable.
- Add pagination and more advanced search for a larger provider population.
- Add structured logging and correlation IDs around write operations.
- Move schema evolution from `EnsureCreated` to EF migrations for production deployments.
- Add CI to restore, build, test, and verify the SQLite scripts on every pull request.

## Submission note

The provided assignment guide asks candidates to work in a branch/fork and submit the GitHub repository or branch/PR link. This ZIP is supplied as a convenient upload artifact; after extracting it, initialize/push it to your own repository/branch as required by the guide.

## Automated tests

The solution includes `Tests/ProviderAssignmentStarter.Tests.csproj` with SQLite in-memory tests covering the key soft-delete invariant. Run:

```bash
dotnet test
```

The test suite verifies that deleting a provider through EF persists `IsDeleted`/`DeletedDate` while ordinary queries no longer return the row.
