using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        builder.HasKey(l => l.IdLocation);
        builder.Property(l => l.IdLocation).HasColumnName("id_location").ValueGeneratedOnAdd();
        builder.Property(l => l.IdBien).HasColumnName("id_bien").IsRequired();
        builder.Property(l => l.IdLocataire).HasColumnName("id_locataire").IsRequired();
        builder.Property(l => l.DateDebut).HasColumnName("date_debut").IsRequired();
        builder.Property(l => l.DateFin).HasColumnName("date_fin");
        builder.Property(l => l.LoyerMensuel).HasColumnName("loyer_mensuel").HasPrecision(10, 2).IsRequired();
        builder.Property(l => l.Caution).HasColumnName("caution").HasPrecision(10, 2).IsRequired();
        builder.Property(l => l.CautionPayee).HasColumnName("caution_payee").HasDefaultValue(false);
        builder.Property(l => l.Statut).HasColumnName("statut")
               .HasConversion<string>().HasMaxLength(20).HasDefaultValue(StatutLocationEnum.Active);
        builder.Property(l => l.BailUrl).HasColumnName("bail_url").HasMaxLength(255);
        builder.Property(l => l.DateSignature).HasColumnName("date_signature");

        builder.HasIndex(l => l.IdBien);
        builder.HasIndex(l => l.IdLocataire);
        builder.HasIndex(l => l.Statut);

        builder.HasOne(l => l.Bien)
               .WithMany(b => b.Locations)
               .HasForeignKey(l => l.IdBien)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Locataire)
               .WithMany(lo => lo.Locations)
               .HasForeignKey(l => l.IdLocataire)
               .OnDelete(DeleteBehavior.Restrict);
    }
}