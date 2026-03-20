using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class PaiementInterventionConfiguration : IEntityTypeConfiguration<PaiementIntervention>
{
    public void Configure(EntityTypeBuilder<PaiementIntervention> builder)
    {
        builder.ToTable("paiements_intervention");
        builder.HasKey(pi => pi.IdPaiementInterv);
        builder.Property(pi => pi.IdPaiementInterv).HasColumnName("id_paiement_interv").ValueGeneratedOnAdd();
        builder.Property(pi => pi.IdPaiement).HasColumnName("id_paiement").IsRequired();
        builder.HasIndex(pi => pi.IdPaiement).IsUnique();
        builder.Property(pi => pi.IdArtisan).HasColumnName("id_artisan").IsRequired();
        builder.Property(pi => pi.MontantArtisan).HasColumnName("montant_artisan").HasPrecision(10, 2).IsRequired();

        builder.HasOne(pi => pi.Paiement)
               .WithOne(p => p.PaiementIntervention)
               .HasForeignKey<PaiementIntervention>(pi => pi.IdPaiement)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pi => pi.Artisan)
               .WithMany(a => a.PaiementsIntervention)
               .HasForeignKey(pi => pi.IdArtisan)
               .OnDelete(DeleteBehavior.Restrict);
    }
}