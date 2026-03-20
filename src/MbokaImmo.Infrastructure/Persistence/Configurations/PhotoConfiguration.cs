using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("photos");
        builder.HasKey(p => p.IdPhoto);
        builder.Property(p => p.IdPhoto).HasColumnName("id_photo").ValueGeneratedOnAdd();
        builder.Property(p => p.EntiteId).HasColumnName("entite_id").IsRequired();
        builder.Property(p => p.EntiteType).HasColumnName("entite_type")
               .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.Url).HasColumnName("url").HasMaxLength(255).IsRequired();
        builder.Property(p => p.Ordre).HasColumnName("ordre").HasDefaultValue(0);
        builder.Property(p => p.DateUpload).HasColumnName("date_upload").HasDefaultValueSql("NOW()");

        builder.HasIndex(p => new { p.EntiteId, p.EntiteType });
    }
}