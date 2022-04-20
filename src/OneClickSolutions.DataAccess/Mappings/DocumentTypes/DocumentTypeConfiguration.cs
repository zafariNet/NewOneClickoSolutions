using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneClickSolutions.Domain.DocumentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneClickSolutions.DataAccess.Mappings.DocumentTypes
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.ToTable("DocumentTypes", "Doc");
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x=>x.Name).IsUnicode().HasMaxLength(50);
            builder.Property(x=>x.Active).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.DefaultDocumentType).IsRequired().HasDefaultValue(false);
            builder.Property(x=>x.RegularExpression).IsUnicode().IsRequired().HasMaxLength(500);
    }
    }
}
