using Microsoft.EntityFrameworkCore;
using ProviderAssignmentStarter.Models;

namespace ProviderAssignmentStarter.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<License> Licenses => Set<License>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(x => x.ProviderId);
            entity.Property(x => x.ProviderName).IsRequired().HasMaxLength(200);
            entity.Property(x => x.County).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(30);
            entity.HasIndex(x => x.ProviderName);
            entity.HasIndex(x => x.IsDeleted);
            entity.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.HasKey(x => x.LicenseId);
            entity.Property(x => x.LicenseNumber).IsRequired().HasMaxLength(100);
            entity.Property(x => x.LicenseStatus).IsRequired().HasMaxLength(30);
            entity.HasIndex(x => new { x.ProviderId, x.LicenseNumber }).IsUnique();
            entity.HasIndex(x => x.ExpirationDate);
            entity.HasIndex(x => x.IsDeleted);
            entity.HasQueryFilter(x => !x.IsDeleted);
            entity.HasOne(x => x.Provider)
                .WithMany(x => x.Licenses)
                .HasForeignKey(x => x.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditAndSoftDelete();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditAndSoftDelete();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAuditAndSoftDelete()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Provider provider)
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    provider.IsDeleted = true;
                    provider.DeletedDate = now;
                    provider.UpdatedDate = now;
                }
                else if (entry.State == EntityState.Added)
                {
                    provider.CreatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    provider.UpdatedDate = now;
                }
            }

            if (entry.Entity is License license)
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    license.IsDeleted = true;
                    license.DeletedDate = now;
                    license.UpdatedDate = now;
                }
                else if (entry.State == EntityState.Added)
                {
                    license.CreatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    license.UpdatedDate = now;
                }
            }
        }
    }
}
