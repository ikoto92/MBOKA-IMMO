using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MbokaImmo.Infrastructure.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");
        builder.HasKey(c => c.IdConversation);
        builder.Property(c => c.IdConversation).HasColumnName("id_conversation").ValueGeneratedOnAdd();
        builder.Property(c => c.Participant1Id).HasColumnName("participant_1").IsRequired();
        builder.Property(c => c.Participant2Id).HasColumnName("participant_2").IsRequired();
        builder.HasIndex(c => new { c.Participant1Id, c.Participant2Id }).IsUnique();
        builder.Property(c => c.DateCreation).HasColumnName("date_creation").HasDefaultValueSql("NOW()");
        builder.Property(c => c.DernierMessage).HasColumnName("dernier_message");

        builder.HasOne(c => c.Participant1)
               .WithMany(u => u.Conversations1)
               .HasForeignKey(c => c.Participant1Id)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Participant2)
               .WithMany(u => u.Conversations2)
               .HasForeignKey(c => c.Participant2Id)
               .OnDelete(DeleteBehavior.Restrict);
    }
}