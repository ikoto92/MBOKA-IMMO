using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        builder.HasKey(m => m.IdMessage);
        builder.Property(m => m.IdMessage).HasColumnName("id_message").ValueGeneratedOnAdd();
        builder.Property(m => m.IdConversation).HasColumnName("id_conversation").IsRequired();
        builder.Property(m => m.ExpediteurId).HasColumnName("expediteur_id").IsRequired();
        builder.Property(m => m.Contenu).HasColumnName("contenu").IsRequired();
        builder.Property(m => m.PieceJointe).HasColumnName("piece_jointe").HasMaxLength(255);
        builder.Property(m => m.Lu).HasColumnName("lu").HasDefaultValue(false);
        builder.Property(m => m.DateEnvoi).HasColumnName("date_envoi").HasDefaultValueSql("NOW()");

        builder.HasIndex(m => m.IdConversation);
        builder.HasIndex(m => m.ExpediteurId);
        builder.HasIndex(m => new { m.Lu, m.DateEnvoi });

        builder.HasOne(m => m.Conversation)
               .WithMany(c => c.Messages)
               .HasForeignKey(m => m.IdConversation)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Expediteur)
               .WithMany(u => u.MessagesEnvoyes)
               .HasForeignKey(m => m.ExpediteurId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
