using Aurora.Insurance.EFModel;
using Aurora.Insurance.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurora.Insurance.Data
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().HasMaxLength(StandardStringLengths.Code);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.LogoData).IsUnicode(false);
        }
    }

    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contract");
            builder.Property(p => p.ContractNumber).IsRequired().HasMaxLength(InsuranceStringLengths.ContractNumber);
            builder.Property(p => p.ReceiptNumber).IsRequired().HasMaxLength(InsuranceStringLengths.ContractNumber);
            // Composite index
            builder.HasIndex(p => new
            {
                p.ContractNumber,
                p.ReceiptNumber
            });
            builder.Property(p => p.GrossAmount).IsRequired();
            builder.Property(p => p.NetAmount).IsRequired();
            builder.Property(p => p.TaxAmount).IsRequired();
            builder.Property(p => p.PlateNumber).IsRequired().HasMaxLength(InsuranceStringLengths.ContractNumber);
            // Single Navigation Property
            builder.HasOne(p => p.Company);
        }
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");
            builder.Property(p => p.FirstName).HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.DrivingLicenceNum).HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.Address).HasMaxLength(StandardStringLengths.LongString);
            builder.Property(p => p.ZipCode).HasMaxLength(InsuranceStringLengths.ZipCode);
        }
    }

    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.ToTable("Phone");
            builder.Property(p => p.Number).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.HasOne(p => p.Person).WithMany(p => p.Phones).IsRequired();
        }
    }
}
