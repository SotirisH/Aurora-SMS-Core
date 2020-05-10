/* Data configuration of all 
 * EF models using Fluent API.
 * Why?: Everything what you can configure with DataAnnotations is also possible with the Fluent API. 
 * The reverse is not true. So, from the viewpoint of configuration options and flexibility the Fluent API is "better".
 * Also the validation rules are out of the POCO class
 * Help:https://msdn.microsoft.com/en-us/data/jj591617.aspx#PropertyIndex
 */

using Aurora.SMS.EFModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurora.SMS.Data
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.ToTable("Provider");
            builder.HasKey(p => p.Name);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Url).IsRequired().HasMaxLength(255);
            builder.Property(p => p.UserName).HasMaxLength(50);
            builder.Property(p => p.PassWord).HasMaxLength(50);
        }
    }

    public class SMSHistoryConfiguration : IEntityTypeConfiguration<SMSHistory>
    {
        public void Configure(EntityTypeBuilder<SMSHistory> builder)
        {
            builder.ToTable("SMSHistory");
            builder.Property(p => p.SessionName).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Message).IsRequired();
            builder.Property(p => p.MobileNumber).HasMaxLength(50);
            builder.Property(p => p.ProviderMsgId).HasMaxLength(255);
            builder.Property(p => p.TemplateId).IsRequired();
            // Create ForeignKey using fluent API on Property 
            // http://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx 
            builder.Property(p => p.TemplateId).IsRequired();
            //https://docs.microsoft.com/en-us/ef/core/modeling/relationships
            builder.HasOne(p => p.Provider)
                .WithMany(s => s.SMSHistory)
                .HasForeignKey(s => s.ProviderName)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.ToTable("Template");
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            //https://docs.microsoft.com/en-us/ef/core/modeling/indexes
            builder.HasIndex(p => p.Name).IsUnique();
            //ef 6     .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() { IsUnique = true }));
            builder.Property(p => p.Description).HasMaxLength(255);
            builder.Property(p => p.Text).IsRequired();
        }
    }

    public class TemplateFieldConfiguration : IEntityTypeConfiguration<TemplateField>
    {
        public void Configure(EntityTypeBuilder<TemplateField> builder)
        {
            builder.ToTable("TemplateField");
            builder.HasKey(t => t.Name);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(255);
            builder.Property(p => p.GroupName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.DataFormat).HasMaxLength(50);
        }
    }
}
