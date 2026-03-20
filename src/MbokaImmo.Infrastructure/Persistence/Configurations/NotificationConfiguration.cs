using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.IdNotification);
        builder.Property(n => n.IdNotification).HasColumnName("id_notification").ValueGeneratedOnAdd();
        builder.Property(n => n.IdUser).HasColumnName("id_user").IsRequired();
        builder.Property(n => n.Type).HasColumnName("type").HasMaxLength(50).IsRequired();
        builder.Property(n => n.Contenu).HasColumnName("contenu").IsRequired();
        builder.Property(n => n.Lien).HasColumnName("lien").HasMaxLength(255);
        builder.Property(n => n.Lu).HasColumnName("lu").HasDefaultValue(false);
        builder.Property(n => n.DateCreation).HasColumnName("date_creation").HasDefaultValueSql("NOW()");

        builder.HasIndex(n => n.IdUser);
        builder.HasIndex(n => n.Lu);
        builder.HasIndex(n => n.DateCreation);

        builder.HasOne(n => n.Utilisateur)
               .WithMany(u => u.Notifications)
               .HasForeignKey(n => n.IdUser)
               .OnDelete(DeleteBehavior.Cascade);
    }
}