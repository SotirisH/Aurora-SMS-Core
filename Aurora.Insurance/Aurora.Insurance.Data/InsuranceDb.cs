using Aurora.Core.Data;
using Aurora.Insurance.EFModel;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Data
{
    public class InsuranceDb : AuditableDbContext
    {
        /// <summary>
        ///     Internal costructor for Migration commands
        /// </summary>
        internal InsuranceDb()
        {
        }

        public InsuranceDb(DbContextOptions<InsuranceDb> options,
            ICurrentUserService currentUserService) : base(options, currentUserService)
        {
        }

        // Remember to setup the entities here or the tables will not be created!
        // Note that the DbSet properties on the context are marked as virtual. 
        //This will allow the mocking framework to derive from our context and overriding these properties with a mocked implementation.
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // When we run the migration commands these are executed in the dev DB
                optionsBuilder.UseSqlServer(@"Server=.\SQL16;Database=InsuranceCore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
