using Aurora.Insurance.EFModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.Insurance.Data
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().HasMaxLength(7);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(50);
        }
    }

    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contract");
            builder.Property(p => p.ContractNumber).IsRequired().HasMaxLength(15);
            builder.Property(p => p.ReceiptNumber).IsRequired().HasMaxLength(15);
            // Composite index
            builder.HasIndex(p => new {p.ContractNumber,p.ReceiptNumber});
            builder.Property(p => p.GrossAmount).IsRequired();
            builder.Property(p => p.NetAmount).IsRequired();
            builder.Property(p => p.TaxAmount).IsRequired();
            builder.Property(p => p.PlateNumber).IsRequired().HasMaxLength(15);
            // Single Navigation Property
            builder.HasOne(p => p.Company);
        }
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");
            builder.Property(p => p.FirstName).HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DrivingLicenceNum).HasMaxLength(50);
            builder.Property(p => p.Address).HasMaxLength(250);
            builder.Property(p => p.ZipCode).HasMaxLength(12);
        }
    }

    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.ToTable("Phone");
            builder.Property(p => p.Number).IsRequired().HasMaxLength(50);
            builder.HasOne(p => p.Person).WithMany(p => p.Phones).IsRequired();
        }
    }
}
