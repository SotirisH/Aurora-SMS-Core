using Aurora.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Company.Infrastructure.Persistence
{
    public class DbContext : AuditableDbContext
    {

        public DbContext()
        {
        }

        public DbContext(DbContextOptions<DbContext> options,
            ICurrentUserService currentUserService) : base(options, currentUserService)
        {
        }

       
        public virtual DbSet<Company.Domain.Models.Entities.Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // When we run the migration commands these are executed in the dev DB
                optionsBuilder.UseSqlServer(@"Server=.\SQL16;Database=Company;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
