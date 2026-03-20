using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class PaiementLoyerConfiguration : IEntityTypeConfiguration<PaiementLoyer>
{
    public void Configure(EntityTypeBuilder<PaiementLoyer> builder)
    {
        builder.ToTable("paiements_loyer");
        builder.HasKey(pl => pl.IdPaiementLoyer);
        builder.Property(pl => pl.IdPaiementLoyer).HasColumnName("id_paiement_loyer").ValueGeneratedOnAdd();
        builder.Property(pl => pl.IdPaiement).HasColumnName("id_paiement").IsRequired();
        builder.HasIndex(pl => pl.IdPaiement).IsUnique();
        builder.Property(pl => pl.MoisConcerne).HasColumnName("mois_concerne").IsRequired();
        builder.HasIndex(pl => pl.MoisConcerne);

        builder.HasOne(pl => pl.Paiement)
               .WithOne(p => p.PaiementLoyer)
               .HasForeignKey<PaiementLoyer>(pl => pl.IdPaiement)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
