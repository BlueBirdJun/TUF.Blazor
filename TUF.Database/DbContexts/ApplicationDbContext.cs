using Daniel.Common.Models;
using Daniel.Common.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Database.TUFDB;

namespace TUF.Database.DbContexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTimeService _dateTime;
        //private readonly IAuthenticatedUserService _authenticatedUser;
        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IDateTimeService dateTime) : base(options)
        //, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            _dateTime = dateTime;
            //_authenticatedUser = authenticatedUser;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AudiTableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.Now;
                        //entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.Now;
                        //entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
            //if (_authenticatedUser.UserId == null)
            //{
            //    return await base.SaveChangesAsync(cancellationToken);
            //}
            //else
            //{
            //    return await base.SaveChangesAsync(_authenticatedUser.UserId);
            //}
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
        }

        public DbSet<CommonCode> CommonCode { get; set; }
        public DbSet<Board> Board { get; set; }
        public DbSet<BoardComment> BoardComment { get; set; }
    }
}
