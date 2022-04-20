using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneClickSolutions.Domain.Identity;

namespace OneClickSolutions.DataAccess.Mappings.Identity;
public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.HasDiscriminator<string>("EntityName");
        builder.Property<string>("EntityName").HasMaxLength(50);

        builder.Property(a => a.Type).IsRequired().HasMaxLength(Claim.TypeLength);
        builder.Property(a => a.Value).IsRequired();

        builder.HasIndex("EntityName").HasDatabaseName("IX_Claim_EntityName");

        builder.ToTable(nameof(Claim));

    }
}