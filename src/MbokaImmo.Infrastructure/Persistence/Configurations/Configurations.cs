using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class UtilisateurConfiguration : IEntityTypeConfiguration<Utilisateur>
{
    public void Configure(EntityTypeBuilder<Utilisateur> builder)
    {
        builder.ToTable("utilisateurs");
        builder.HasKey(u => u.IdUser);
        builder.Property(u => u.IdUser).HasColumnName("id_user").ValueGeneratedOnAdd();
        builder.Property(u => u.Nom).HasColumnName("nom").HasMaxLength(100).IsRequired();
        builder.Property(u => u.Prenom).HasColumnName("prenom").HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.MotDePasse).HasColumnName("mot_de_passe").HasMaxLength(255).IsRequired();
        builder.Property(u => u.Telephone).HasColumnName("telephone").HasMaxLength(20);
        builder.Property(u => u.PaysResidence).HasColumnName("pays_residence").HasMaxLength(100);
        builder.Property(u => u.VilleResidence).HasColumnName("ville_residence").HasMaxLength(100);
        builder.Property(u => u.PieceIdentite).HasColumnName("piece_identite").HasMaxLength(255);
        builder.Property(u => u.KycValide).HasColumnName("kyc_valide").HasDefaultValue(false);
        builder.Property(u => u.Role).HasColumnName("role")
               .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(u => u.DateInscription).HasColumnName("date_inscription").IsRequired();
        builder.Property(u => u.CompteActif).HasColumnName("compte_actif").HasDefaultValue(true);
    }
}
