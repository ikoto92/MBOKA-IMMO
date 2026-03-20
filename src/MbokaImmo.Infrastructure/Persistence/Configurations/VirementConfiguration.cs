using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class VirementConfiguration : IEntityTypeConfiguration<Virement>
{
    public void Configure(EntityTypeBuilder<Virement> builder)
    {
        builder.ToTable("virements");
        builder.HasKey(v => v.IdVirement);
        builder.Property(v => v.IdVirement).HasColumnName("id_virement").ValueGeneratedOnAdd();
        builder.Property(v => v.IdProprio).HasColumnName("id_proprio").IsRequired();
        builder.Property(v => v.IbanDestination).HasColumnName("iban_destination").HasMaxLength(34).IsRequired();
        builder.Property(v => v.MontantNet).HasColumnName("montant_net").HasPrecision(10, 2).IsRequired();
        builder.Property(v => v.Commission).HasColumnName("commission").HasPrecision(10, 2).HasDefaultValue(0m);
        builder.Property(v => v.Statut).HasColumnName("statut")
               .HasConversion<string>().HasMaxLength(20).HasDefaultValue(StatutVirementEnum.EnAttente);
        builder.Property(v => v.DateVirement).HasColumnName("date_virement");
        builder.Property(v => v.ReferenceBanque).HasColumnName("reference_banque").HasMaxLength(100);
        builder.HasIndex(v => v.ReferenceBanque).IsUnique().HasFilter("reference_banque IS NOT NULL");
        builder.Property(v => v.Operateur).HasColumnName("operateur").HasMaxLength(50);

        builder.HasIndex(v => v.IdProprio);
        builder.HasIndex(v => v.Statut);

        builder.HasOne(v => v.Proprietaire)
               .WithMany(p => p.Virements)
               .HasForeignKey(v => v.IdProprio)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
