using Aurora.Core.Data.Abstractions;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Aurora.Insurance.Server.Entity.Infrastructure.Persistence
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organization");
            builder.HasKey(p => p.OrganizationId);
            builder.HasOne(p => p.Broker)
                .WithOne()
                .HasPrincipalKey<Organization>(p => p.BrokerId)
                .HasForeignKey<Agent>(p => p.ContactId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");
            builder.HasKey(p => p.ContactId);
            builder.Property(p => p.ContactId).HasValueGenerator<SequentialGuidValueGenerator>();
            builder.Property(p => p.TaxId).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.EmailAddress).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(StandardStringLengths.LongString);
            builder.Property(p => p.FirstName).HasMaxLength(StandardStringLengths.DefaultString);
            builder.HasOne(p => p.Address);
            builder.HasMany(p => p.Phones);
            builder.HasOne(p=>p.Organization)
                .WithMany()
                .HasPrincipalKey(p => p.OrganizationId)
                .HasForeignKey(p => p.OrganizationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.ToTable("Agent");
            builder.Ignore(p => p.AgentId);
            builder.HasMany(p => p.Agents)
                .WithOne()
                .HasPrincipalKey(p => p.ContactId)
                .HasForeignKey(p => p.ContactId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasOne(p=>p.Agent)
                .WithMany()
                .HasPrincipalKey(p => p.ContactId)
                .HasForeignKey(p => p.AgentId);
            builder.HasOne(p => p.DrivingLicence).WithOne();
        }
    }

    public class DrivingLicenceConfiguration : IEntityTypeConfiguration<DrivingLicence>
    {
        public void Configure(EntityTypeBuilder<DrivingLicence> builder)
        {
            builder.ToTable("DrivingLicence");
            builder.HasKey(p => p.DrivingLicenceId);
            builder.Property(p => p.CountryId).IsRequired();
            builder.Property(p => p.Number).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.Version).HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.IssueDate).IsRequired();
            builder.Property(p => p.ExpireDate).IsRequired();
            builder.Property(p => p.Conditions).HasMaxLength(StandardStringLengths.Comment);

            builder.HasOne(p => p.Customer)
                .WithOne(p => p.DrivingLicence)
                .HasPrincipalKey<DrivingLicence>(p => p.ContactId)
                .HasForeignKey<Customer>(p => p.ContactId);
        }
    }

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(p => p.AddressId);
            builder.Property(p => p.AddressLine).IsRequired().HasMaxLength(StandardStringLengths.LongString);
            builder.Property(p => p.ZipCode).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.City).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.CountryId).IsRequired();

            builder.HasOne(p => p.Contact);
        }
    }

    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.ToTable("Phone");
            builder.HasKey(p => p.PhoneId);
            builder.Property(p => p.Number).IsRequired().HasMaxLength(StandardStringLengths.LongString);
            builder.Property(p => p.PhoneType).IsRequired().HasConversion<string>();

            builder.HasOne(p => p.Contact)
                .WithMany(p => p.Phones)
                .HasForeignKey(p => p.ContactId);
        }
    }
}
