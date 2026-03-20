using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class VisiteConfiguration : IEntityTypeConfiguration<Visite>
{
    public void Configure(EntityTypeBuilder<Visite> builder)
    {
        builder.ToTable("visites");
        builder.HasKey(v => v.IdVisite);
        builder.Property(v => v.IdVisite).HasColumnName("id_visite").ValueGeneratedOnAdd();
        builder.Property(v => v.IdBien).HasColumnName("id_bien").IsRequired();
        builder.Property(v => v.IdAgent).HasColumnName("id_agent").IsRequired();
        builder.Property(v => v.IdLocataire).HasColumnName("id_locataire").IsRequired();
        builder.Property(v => v.DateVisite).HasColumnName("date_visite").IsRequired();
        builder.Property(v => v.CompteRendu).HasColumnName("compte_rendu");
        builder.Property(v => v.Statut).HasColumnName("statut")
               .HasConversion<string>().HasMaxLength(20).HasDefaultValue(StatutVisiteEnum.Planifiee);

        builder.HasIndex(v => v.IdBien);
        builder.HasIndex(v => v.IdAgent);
        builder.HasIndex(v => v.DateVisite);

        builder.HasOne(v => v.Bien)
               .WithMany(b => b.Visites)
               .HasForeignKey(v => v.IdBien)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Agent)
               .WithMany(a => a.Visites)
               .HasForeignKey(v => v.IdAgent)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Locataire)
               .WithMany(l => l.Visites)
               .HasForeignKey(v => v.IdLocataire)
               .OnDelete(DeleteBehavior.Restrict);
    }
}