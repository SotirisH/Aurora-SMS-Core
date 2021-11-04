using Aurora.Insurance.Server.Entity.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Server.Entity.Infrastructure.Persistence
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
        {
        }

        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<DrivingLicence> DrivingLicences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // When we run the migration commands these are executed in the dev DB
                optionsBuilder.UseSqlServer(@"Server=.\SQL2019;Database=Entity;Trusted_Connection=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactConfiguration).Assembly);
        }
    }
}
