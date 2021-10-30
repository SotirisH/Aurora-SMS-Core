using Aurora.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurora.Insurance.Company.Infrastructure.Persistence
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Domain.Models.Entities.Company>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Entities.Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().HasMaxLength(StandardStringLengths.Code);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(StandardStringLengths.DefaultString);
            builder.Property(p => p.LogoData).IsUnicode(false);
        }
    }
}
