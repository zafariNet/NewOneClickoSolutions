using OneClickSolutions.Infrastructure.GuardToolkit;
using OneClickSolutions.Infrastructure.Numbering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.SqlServer.Numbering
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyNumberedEntityConfiguration(this ModelBuilder builder)
        {
            Guard.ArgumentNotNull(builder, nameof(builder));

            builder.ApplyConfiguration(new NumberedEntityConfiguration());
        }
    }

    public class NumberedEntityConfiguration : IEntityTypeConfiguration<NumberedEntity>
    {
        public void Configure(EntityTypeBuilder<NumberedEntity> builder)
        {
            builder.Property(a => a.EntityName).HasMaxLength(256).IsRequired().IsUnicode(false);

            builder.HasIndex(a => a.EntityName).HasDatabaseName("UIX_NumberedEntity_EntityName").IsUnique();

            builder.ToTable(nameof(NumberedEntity));
        }
    }
}