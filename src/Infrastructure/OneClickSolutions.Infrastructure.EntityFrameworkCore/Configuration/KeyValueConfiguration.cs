using System;
using OneClickSolutions.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Configuration
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyKeyValueConfiguration(this ModelBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new KeyValueConfiguration());
        }
    }

    public class KeyValueConfiguration : IEntityTypeConfiguration<KeyValue>
    {
        public void Configure(EntityTypeBuilder<KeyValue> builder)
        {
            builder.Property(v => v.Key).HasMaxLength(450).IsRequired();
            builder.Property(v => v.Value).IsRequired();

            builder.HasIndex(v => v.Key).HasDatabaseName("UIX_Values_Key").IsUnique();
            
            builder.ToTable("Values");
        }
    }
}