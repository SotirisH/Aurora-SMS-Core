using Aurora.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.SMS.Data
{
    /// <summary>
    /// The DBContext to the database
    /// </summary>
    public class SMSDb: AuditableDbContext
    {
        public SMSDb():base()
        {

        }
        public SMSDb(DbContextOptions options,
                                 ICurrentUserService currentUserService) :base(options,currentUserService)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQL16;Database=SMSDbCore;Trusted_Connection=True;");
            }
        }

        public virtual DbSet<EFModel.Provider> Providers { get; set; }
        public virtual DbSet<EFModel.SMSHistory> SMSHistoryRecords { get; set; }
        public virtual DbSet<EFModel.Template> Templates { get; set; }
        public virtual DbSet<EFModel.TemplateField> TemplateFields { get; set; }

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
