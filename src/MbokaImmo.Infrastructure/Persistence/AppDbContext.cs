using Microsoft.EntityFrameworkCore;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;

namespace MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Proprietaire> Proprietaires { get; set; }
    public DbSet<Locataire> Locataires { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<Artisan> Artisans { get; set; }
    public DbSet<Bien> Biens { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Candidature> Candidatures { get; set; }
    public DbSet<Visite> Visites { get; set; }
    public DbSet<Favori> Favoris { get; set; }
    public DbSet<Paiement> Paiements { get; set; }
    public DbSet<PaiementLoyer> PaiementsLoyer { get; set; }
    public DbSet<PaiementIntervention> PaiementsIntervention { get; set; }
    public DbSet<Virement> Virements { get; set; }
    public DbSet<Intervention> Interventions { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Clés primaires ────────────────────────────────────────
        modelBuilder.Entity<Utilisateur>().HasKey(u => u.IdUser);
        modelBuilder.Entity<Proprietaire>().HasKey(p => p.IdProprio);
        modelBuilder.Entity<Locataire>().HasKey(l => l.IdLocataire);
        modelBuilder.Entity<Agent>().HasKey(a => a.IdAgent);
        modelBuilder.Entity<Artisan>().HasKey(a => a.IdArtisan);
        modelBuilder.Entity<Bien>().HasKey(b => b.IdBien);
        modelBuilder.Entity<Photo>().HasKey(p => p.IdPhoto);
        modelBuilder.Entity<Location>().HasKey(l => l.IdLocation);
        modelBuilder.Entity<Candidature>().HasKey(c => c.IdCandidature);
        modelBuilder.Entity<Visite>().HasKey(v => v.IdVisite);
        modelBuilder.Entity<Favori>().HasKey(f => f.IdFavori);
        modelBuilder.Entity<Paiement>().HasKey(p => p.IdPaiement);
        modelBuilder.Entity<PaiementLoyer>().HasKey(pl => pl.IdPaiementLoyer);
        modelBuilder.Entity<PaiementIntervention>().HasKey(pi => pi.IdPaiementInterv);
        modelBuilder.Entity<Virement>().HasKey(v => v.IdVirement);
        modelBuilder.Entity<Intervention>().HasKey(i => i.IdIntervention);
        modelBuilder.Entity<Conversation>().HasKey(c => c.IdConversation);
        modelBuilder.Entity<Message>().HasKey(m => m.IdMessage);
        modelBuilder.Entity<Notification>().HasKey(n => n.IdNotification);

        // ── Relations ─────────────────────────────────────────────

        // Utilisateur → spécialisations (1-1)
        modelBuilder.Entity<Proprietaire>()
            .HasOne(p => p.Utilisateur)
            .WithOne(u => u.Proprietaire)
            .HasForeignKey<Proprietaire>(p => p.IdUser)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Locataire>()
            .HasOne(l => l.Utilisateur)
            .WithOne(u => u.Locataire)
            .HasForeignKey<Locataire>(l => l.IdUser)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Agent>()
            .HasOne(a => a.Utilisateur)
            .WithOne(u => u.Agent)
            .HasForeignKey<Agent>(a => a.IdUser)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Artisan>()
            .HasOne(a => a.Utilisateur)
            .WithOne(u => u.Artisan)
            .HasForeignKey<Artisan>(a => a.IdUser)
            .OnDelete(DeleteBehavior.Restrict);

        // Bien → Proprietaire
        modelBuilder.Entity<Bien>()
            .HasOne(b => b.Proprietaire)
            .WithMany(p => p.Biens)
            .HasForeignKey(b => b.IdProprietaire)
            .OnDelete(DeleteBehavior.Restrict);

        // Location → Bien + Locataire
        modelBuilder.Entity<Location>()
            .HasOne(l => l.Bien)
            .WithMany(b => b.Locations)
            .HasForeignKey(l => l.IdBien)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Location>()
            .HasOne(l => l.Locataire)
            .WithMany(lo => lo.Locations)
            .HasForeignKey(l => l.IdLocataire)
            .OnDelete(DeleteBehavior.Restrict);

        // Candidature → Bien + Locataire
        modelBuilder.Entity<Candidature>()
            .HasOne(c => c.Bien)
            .WithMany(b => b.Candidatures)
            .HasForeignKey(c => c.IdBien)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Candidature>()
            .HasOne(c => c.Locataire)
            .WithMany(l => l.Candidatures)
            .HasForeignKey(c => c.IdLocataire)
            .OnDelete(DeleteBehavior.Cascade);

        // Visite → Bien + Agent + Locataire
        modelBuilder.Entity<Visite>()
            .HasOne(v => v.Bien)
            .WithMany(b => b.Visites)
            .HasForeignKey(v => v.IdBien)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Visite>()
            .HasOne(v => v.Agent)
            .WithMany(a => a.Visites)
            .HasForeignKey(v => v.IdAgent)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Visite>()
            .HasOne(v => v.Locataire)
            .WithMany(l => l.Visites)
            .HasForeignKey(v => v.IdLocataire)
            .OnDelete(DeleteBehavior.Restrict);

        // Favori → Utilisateur + Bien
        modelBuilder.Entity<Favori>()
            .HasOne(f => f.Utilisateur)
            .WithMany(u => u.Favoris)
            .HasForeignKey(f => f.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favori>()
            .HasOne(f => f.Bien)
            .WithMany(b => b.Favoris)
            .HasForeignKey(f => f.IdBien)
            .OnDelete(DeleteBehavior.Cascade);

        // Paiement → Location
        modelBuilder.Entity<Paiement>()
            .HasOne(p => p.Location)
            .WithMany(l => l.Paiements)
            .HasForeignKey(p => p.IdLocation)
            .OnDelete(DeleteBehavior.Restrict);

        // PaiementLoyer → Paiement (1-1)
        modelBuilder.Entity<PaiementLoyer>()
            .HasOne(pl => pl.Paiement)
            .WithOne(p => p.PaiementLoyer)
            .HasForeignKey<PaiementLoyer>(pl => pl.IdPaiement)
            .OnDelete(DeleteBehavior.Cascade);

        // PaiementIntervention → Paiement (1-1) + Artisan
        modelBuilder.Entity<PaiementIntervention>()
            .HasOne(pi => pi.Paiement)
            .WithOne(p => p.PaiementIntervention)
            .HasForeignKey<PaiementIntervention>(pi => pi.IdPaiement)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaiementIntervention>()
            .HasOne(pi => pi.Artisan)
            .WithMany(a => a.PaiementsIntervention)
            .HasForeignKey(pi => pi.IdArtisan)
            .OnDelete(DeleteBehavior.Restrict);

        // Virement → Proprietaire
        modelBuilder.Entity<Virement>()
            .HasOne(v => v.Proprietaire)
            .WithMany(p => p.Virements)
            .HasForeignKey(v => v.IdProprio)
            .OnDelete(DeleteBehavior.Restrict);

        // Intervention → Location + Artisan
        modelBuilder.Entity<Intervention>()
            .HasOne(i => i.Location)
            .WithMany(l => l.Interventions)
            .HasForeignKey(i => i.IdLocation)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Intervention>()
            .HasOne(i => i.Artisan)
            .WithMany(a => a.Interventions)
            .HasForeignKey(i => i.IdArtisan)
            .OnDelete(DeleteBehavior.SetNull);

        // Conversation → Participant1 + Participant2
        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.Participant1)
            .WithMany(u => u.Conversations1)
            .HasForeignKey(c => c.Participant1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.Participant2)
            .WithMany(u => u.Conversations2)
            .HasForeignKey(c => c.Participant2Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Message → Conversation + Expediteur
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.IdConversation)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Expediteur)
            .WithMany(u => u.MessagesEnvoyes)
            .HasForeignKey(m => m.ExpediteurId)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification → Utilisateur
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Utilisateur)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        // Enums stockés en string
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.Role).HasConversion<string>();
        modelBuilder.Entity<Bien>()
            .Property(b => b.Type).HasConversion<string>();
        modelBuilder.Entity<Bien>()
            .Property(b => b.Statut).HasConversion<string>();
        modelBuilder.Entity<Location>()
            .Property(l => l.Statut).HasConversion<string>();
        modelBuilder.Entity<Candidature>()
            .Property(c => c.Statut).HasConversion<string>();
        modelBuilder.Entity<Visite>()
            .Property(v => v.Statut).HasConversion<string>();
        modelBuilder.Entity<Paiement>()
            .Property(p => p.Type).HasConversion<string>();
        modelBuilder.Entity<Paiement>()
            .Property(p => p.Mode).HasConversion<string>();
        modelBuilder.Entity<Paiement>()
            .Property(p => p.Statut).HasConversion<string>();
        modelBuilder.Entity<Virement>()
            .Property(v => v.Statut).HasConversion<string>();
        modelBuilder.Entity<Intervention>()
            .Property(i => i.Urgence).HasConversion<string>();
        modelBuilder.Entity<Intervention>()
            .Property(i => i.Statut).HasConversion<string>();
        modelBuilder.Entity<Photo>()
            .Property(p => p.EntiteType).HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}