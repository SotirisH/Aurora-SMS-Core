using Aurora.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Company.Infrastructure.Persistence
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
        {
        }

        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
        {
        }
        //public LocalDbContext(DbContextOptions<LocalDbContext> options,
        //    ICurrentUserService currentUserService) : base(options, currentUserService)
        //{
        //}


        public virtual DbSet<Domain.Models.Entities.Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // When we run the migration commands these are executed in the dev DB
                optionsBuilder.UseSqlServer(@"Server=.\SQL16;Database=Company;Trusted_Connection=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
