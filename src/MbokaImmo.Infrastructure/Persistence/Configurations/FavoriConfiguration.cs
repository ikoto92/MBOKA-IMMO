using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class FavoriConfiguration : IEntityTypeConfiguration<Favori>
{
    public void Configure(EntityTypeBuilder<Favori> builder)
    {
        builder.ToTable("favoris");
        builder.HasKey(f => f.IdFavori);
        builder.Property(f => f.IdFavori).HasColumnName("id_favori").ValueGeneratedOnAdd();
        builder.Property(f => f.IdUser).HasColumnName("id_user").IsRequired();
        builder.Property(f => f.IdBien).HasColumnName("id_bien").IsRequired();
        builder.HasIndex(f => new { f.IdUser, f.IdBien }).IsUnique();
        builder.Property(f => f.DateAjout).HasColumnName("date_ajout").HasDefaultValueSql("NOW()");

        builder.HasOne(f => f.Utilisateur)
               .WithMany(u => u.Favoris)
               .HasForeignKey(f => f.IdUser)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Bien)
               .WithMany(b => b.Favoris)
               .HasForeignKey(f => f.IdBien)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
