using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class ProprietaireConfiguration : IEntityTypeConfiguration<Proprietaire>
{
    public void Configure(EntityTypeBuilder<Proprietaire> builder)
    {
        builder.ToTable("proprietaires");
        builder.HasKey(p => p.IdProprio);
        builder.Property(p => p.IdProprio).HasColumnName("id_proprio").ValueGeneratedOnAdd();
        builder.Property(p => p.IdUser).HasColumnName("id_user").IsRequired();
        builder.HasIndex(p => p.IdUser).IsUnique();
        builder.Property(p => p.CompteBancaire).HasColumnName("compte_bancaire").HasMaxLength(50);
        builder.Property(p => p.Iban).HasColumnName("iban").HasMaxLength(34);

        builder.HasOne(p => p.Utilisateur)
               .WithOne(u => u.Proprietaire)
               .HasForeignKey<Proprietaire>(p => p.IdUser)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
