using Aurora.Core.Data;
using Aurora.SMS.EFModel;
using Microsoft.EntityFrameworkCore;

namespace Aurora.SMS.Data
{
    /// <summary>
    ///     The DBContext to the database
    /// </summary>
    public class SMSDb : AuditableDbContext
    {
        /// <summary>
        ///     Internal costructor for Migration commands
        /// </summary>
        internal SMSDb()
        {
        }

        public SMSDb(DbContextOptions<SMSDb> options,
            ICurrentUserService currentUserService) : base(options, currentUserService)
        {
            //https://stackoverflow.com/questions/41513296/can-i-safely-use-the-non-generic-dbcontextoptions-in-asp-net-core-and-ef-core
        }

        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<SMSHistory> SMSHistoryRecords { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<TemplateField> TemplateFields { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // When we run the migration commands these are executed in the dev DB
                optionsBuilder.UseSqlServer(@"Server=.\SQL16;Database=SMSDbCore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateFieldConfiguration());
            modelBuilder.ApplyConfiguration(new SMSHistoryConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
