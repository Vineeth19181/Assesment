# Soft-delete verification checklist

The runtime design can be verified with these scenarios:

1. Create a provider. It appears in the provider listing.
2. Delete the provider from its details page. The delete action is a POST and the `SaveChanges` interceptor converts the delete into an update.
3. Refresh the provider listing. The provider is absent because the global query filter applies `IsDeleted = 0`.
4. Query the database with `SELECT ... FROM Providers WHERE IsDeleted = 1;`. The row still exists and contains `DeletedDate`.
5. Use EF Core `IgnoreQueryFilters()` in audit/support code to retrieve the deleted provider.
6. Attempting to call `DbSet.Remove(provider)` elsewhere in application code still results in a soft delete because the DbContext interceptor is the final persistence safeguard.

The SQL equivalents are also included in `Scripts/002_RequiredScenarios.sql`.
