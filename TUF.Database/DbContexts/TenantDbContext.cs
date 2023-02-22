using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Database.TUFDB;

namespace TUF.Database.DbContexts;

public class TenantDbContext : EFCoreStoreDbContext<TUFTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TUFTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}
