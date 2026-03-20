using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class BienConfiguration : IEntityTypeConfiguration<Bien>
{
    public void Configure(EntityTypeBuilder<Bien> builder)
    {
        builder.ToTable("biens");
        builder.HasKey(b => b.IdBien);
        builder.Property(b => b.IdBien).HasColumnName("id_bien").ValueGeneratedOnAdd();
        builder.Property(b => b.IdProprietaire).HasColumnName("id_proprietaire").IsRequired();
        builder.Property(b => b.Titre).HasColumnName("titre").HasMaxLength(200).IsRequired();
        builder.Property(b => b.Description).HasColumnName("description");
        builder.Property(b => b.Adresse).HasColumnName("adresse").HasMaxLength(255).IsRequired();
        builder.Property(b => b.Ville).HasColumnName("ville").HasMaxLength(100).IsRequired();
        builder.Property(b => b.Quartier).HasColumnName("quartier").HasMaxLength(100);
        builder.Property(b => b.Type).HasColumnName("type")
               .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(b => b.Surface).HasColumnName("surface").IsRequired();
        builder.Property(b => b.Pieces).HasColumnName("pieces").IsRequired();
        builder.Property(b => b.LoyerMensuel).HasColumnName("loyer_mensuel").HasPrecision(10, 2).IsRequired();
        builder.Property(b => b.CautionMois).HasColumnName("caution_mois").HasDefaultValue(2);
        builder.Property(b => b.Disponibilite).HasColumnName("disponibilite");
        builder.Property(b => b.Statut).HasColumnName("statut")
               .HasConversion<string>().HasMaxLength(20).HasDefaultValue(StatutBienEnum.Libre);
        builder.Property(b => b.Video).HasColumnName("video").HasMaxLength(255);
        builder.Property(b => b.DateCreation).HasColumnName("date_creation").HasDefaultValueSql("NOW()");
        builder.Property(b => b.Valide).HasColumnName("valide").HasDefaultValue(false);

        builder.HasIndex(b => new { b.Ville, b.Type });
        builder.HasIndex(b => b.Statut);
        builder.HasIndex(b => b.LoyerMensuel);

        builder.HasOne(b => b.Proprietaire)
               .WithMany(p => p.Biens)
               .HasForeignKey(b => b.IdProprietaire)
               .OnDelete(DeleteBehavior.Restrict);
    }
}