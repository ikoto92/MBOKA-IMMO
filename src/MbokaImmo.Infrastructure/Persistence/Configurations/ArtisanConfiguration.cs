using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class ArtisanConfiguration : IEntityTypeConfiguration<Artisan>
{
    public void Configure(EntityTypeBuilder<Artisan> builder)
    {
        builder.ToTable("artisans");
        builder.HasKey(a => a.IdArtisan);
        builder.Property(a => a.IdArtisan).HasColumnName("id_artisan").ValueGeneratedOnAdd();
        builder.Property(a => a.IdUser).HasColumnName("id_user").IsRequired();
        builder.HasIndex(a => a.IdUser).IsUnique();
        builder.Property(a => a.Specialite).HasColumnName("specialite").HasMaxLength(100).IsRequired();
        builder.Property(a => a.ZoneIntervention).HasColumnName("zone_intervention").HasMaxLength(100);
        builder.Property(a => a.NoteMoyenne).HasColumnName("note_moyenne").HasPrecision(3, 2).HasDefaultValue(0f);
        builder.Property(a => a.NbInterventions).HasColumnName("nb_interventions").HasDefaultValue(0);
        builder.Property(a => a.Disponible).HasColumnName("disponible").HasDefaultValue(true);
        builder.Property(a => a.TarifHoraire).HasColumnName("tarif_horaire").HasPrecision(10, 2);

        builder.HasOne(a => a.Utilisateur)
               .WithOne(u => u.Artisan)
               .HasForeignKey<Artisan>(a => a.IdUser)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
