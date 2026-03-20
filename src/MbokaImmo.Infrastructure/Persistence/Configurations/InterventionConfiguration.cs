using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class InterventionConfiguration : IEntityTypeConfiguration<Intervention>
{
    public void Configure(EntityTypeBuilder<Intervention> builder)
    {
        builder.ToTable("interventions");
        builder.HasKey(i => i.IdIntervention);
        builder.Property(i => i.IdIntervention).HasColumnName("id_intervention").ValueGeneratedOnAdd();
        builder.Property(i => i.IdLocation).HasColumnName("id_location").IsRequired();
        builder.Property(i => i.IdArtisan).HasColumnName("id_artisan");
        builder.Property(i => i.Type).HasColumnName("type").HasMaxLength(100).IsRequired();
        builder.Property(i => i.Description).HasColumnName("description").IsRequired();
        builder.Property(i => i.Urgence).HasColumnName("urgence")
               .HasConversion<string>().HasMaxLength(10).HasDefaultValue(UrgenceEnum.Moyenne);
        builder.Property(i => i.Statut).HasColumnName("statut")
               .HasConversion<string>().HasMaxLength(20).HasDefaultValue(StatutInterventionEnum.Signale);
        builder.Property(i => i.DateSignalement).HasColumnName("date_signalement").HasDefaultValueSql("NOW()");
        builder.Property(i => i.DateIntervention).HasColumnName("date_intervention");
        builder.Property(i => i.DevisMontant).HasColumnName("devis_montant").HasPrecision(10, 2);
        builder.Property(i => i.DevisValide).HasColumnName("devis_valide").HasDefaultValue(false);
        builder.Property(i => i.FactureUrl).HasColumnName("facture_url").HasMaxLength(255);
        builder.Property(i => i.NoteLocataire).HasColumnName("note_locataire");
        builder.Property(i => i.Commentaire).HasColumnName("commentaire");

        builder.HasIndex(i => i.IdLocation);
        builder.HasIndex(i => i.IdArtisan);
        builder.HasIndex(i => new { i.Statut, i.Urgence });

        builder.HasOne(i => i.Location)
               .WithMany(l => l.Interventions)
               .HasForeignKey(i => i.IdLocation)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Artisan)
               .WithMany(a => a.Interventions)
               .HasForeignKey(i => i.IdArtisan)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
