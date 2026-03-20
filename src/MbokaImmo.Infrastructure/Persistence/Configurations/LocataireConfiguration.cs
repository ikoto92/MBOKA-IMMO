using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class LocataireConfiguration : IEntityTypeConfiguration<Locataire>
{
    public void Configure(EntityTypeBuilder<Locataire> builder)
    {
        builder.ToTable("locataires");
        builder.HasKey(l => l.IdLocataire);
        builder.Property(l => l.IdLocataire).HasColumnName("id_locataire").ValueGeneratedOnAdd();
        builder.Property(l => l.IdUser).HasColumnName("id_user").IsRequired();
        builder.HasIndex(l => l.IdUser).IsUnique();
        builder.Property(l => l.ScoreCreditworthiness).HasColumnName("score_creditworthiness").HasDefaultValue(0);

        builder.HasOne(l => l.Utilisateur)
               .WithOne(u => u.Locataire)
               .HasForeignKey<Locataire>(l => l.IdUser)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
