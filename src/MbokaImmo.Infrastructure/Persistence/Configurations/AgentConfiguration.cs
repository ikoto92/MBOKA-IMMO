using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("agents");
        builder.HasKey(a => a.IdAgent);
        builder.Property(a => a.IdAgent).HasColumnName("id_agent").ValueGeneratedOnAdd();
        builder.Property(a => a.IdUser).HasColumnName("id_user").IsRequired();
        builder.HasIndex(a => a.IdUser).IsUnique();
        builder.Property(a => a.ZoneIntervention).HasColumnName("zone_intervention").HasMaxLength(100);
        builder.Property(a => a.NbVisites).HasColumnName("nb_visites").HasDefaultValue(0);

        builder.HasOne(a => a.Utilisateur)
               .WithOne(u => u.Agent)
               .HasForeignKey<Agent>(a => a.IdUser)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
