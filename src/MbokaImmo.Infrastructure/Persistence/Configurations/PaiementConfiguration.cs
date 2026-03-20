using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class PaiementConfiguration : IEntityTypeConfiguration<Paiement>
{
    public void Configure(EntityTypeBuilder<Paiement> builder)
    {
        builder.ToTable("paiements");
        builder.HasKey(p => p.IdPaiement);
        builder.Property(p => p.IdPaiement).HasColumnName("id_paiement").ValueGeneratedOnAdd();
        builder.Property(p => p.IdLocation).HasColumnName("id_location").IsRequired();
        builder.Property(p => p.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(p => p.Montant).HasColumnName("montant").HasPrecision(10, 2).IsRequired();
        builder.Property(p => p.DatePaiement).HasColumnName("date_paiement").HasDefaultValueSql("NOW()");
        builder.Property(p => p.Mode).HasColumnName("mode").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.Operateur).HasColumnName("operateur").HasMaxLength(50);
        builder.Property(p => p.Reference).HasColumnName("reference").HasMaxLength(100).IsRequired();
        builder.HasIndex(p => p.Reference).IsUnique();
        builder.Property(p => p.Statut).HasColumnName("statut")
               .HasConversion<string>().HasMaxLength(20).HasDefaultValue(StatutPaiementEnum.EnAttente);
        builder.Property(p => p.Commission).HasColumnName("commission").HasPrecision(10, 2).HasDefaultValue(0m);
        builder.Property(p => p.MontantNet).HasColumnName("montant_net").HasPrecision(10, 2);

        builder.HasIndex(p => p.IdLocation);
        builder.HasIndex(p => p.Statut);
        builder.HasIndex(p => p.DatePaiement);

        builder.HasOne(p => p.Location)
               .WithMany(l => l.Paiements)
               .HasForeignKey(p => p.IdLocation)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
