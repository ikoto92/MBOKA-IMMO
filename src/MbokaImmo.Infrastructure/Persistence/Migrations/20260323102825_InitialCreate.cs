using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MBOKA_IMMO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prenom = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MotDePasse = table.Column<string>(type: "text", nullable: false),
                    Telephone = table.Column<string>(type: "text", nullable: true),
                    PaysResidence = table.Column<string>(type: "text", nullable: true),
                    VilleResidence = table.Column<string>(type: "text", nullable: true),
                    PieceIdentite = table.Column<string>(type: "text", nullable: true),
                    KycValide = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompteActif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    IdAgent = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    ZoneIntervention = table.Column<string>(type: "text", nullable: true),
                    NbVisites = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.IdAgent);
                    table.ForeignKey(
                        name: "FK_Agents_Utilisateurs_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Artisans",
                columns: table => new
                {
                    IdArtisan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    Specialite = table.Column<string>(type: "text", nullable: false),
                    ZoneIntervention = table.Column<string>(type: "text", nullable: true),
                    NoteMoyenne = table.Column<float>(type: "real", nullable: false),
                    NbInterventions = table.Column<int>(type: "integer", nullable: false),
                    Disponible = table.Column<bool>(type: "boolean", nullable: false),
                    TarifHoraire = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artisans", x => x.IdArtisan);
                    table.ForeignKey(
                        name: "FK_Artisans_Utilisateurs_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    IdConversation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Participant1Id = table.Column<int>(type: "integer", nullable: false),
                    Participant2Id = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DernierMessage = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.IdConversation);
                    table.ForeignKey(
                        name: "FK_Conversations_Utilisateurs_Participant1Id",
                        column: x => x.Participant1Id,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_Utilisateurs_Participant2Id",
                        column: x => x.Participant2Id,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locataires",
                columns: table => new
                {
                    IdLocataire = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    ScoreCreditworthiness = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locataires", x => x.IdLocataire);
                    table.ForeignKey(
                        name: "FK_Locataires_Utilisateurs_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    IdNotification = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Contenu = table.Column<string>(type: "text", nullable: false),
                    Lien = table.Column<string>(type: "text", nullable: true),
                    Lu = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.IdNotification);
                    table.ForeignKey(
                        name: "FK_Notifications_Utilisateurs_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proprietaires",
                columns: table => new
                {
                    IdProprio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    CompteBancaire = table.Column<string>(type: "text", nullable: true),
                    Iban = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proprietaires", x => x.IdProprio);
                    table.ForeignKey(
                        name: "FK_Proprietaires_Utilisateurs_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    IdMessage = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdConversation = table.Column<int>(type: "integer", nullable: false),
                    ExpediteurId = table.Column<int>(type: "integer", nullable: false),
                    Contenu = table.Column<string>(type: "text", nullable: false),
                    PieceJointe = table.Column<string>(type: "text", nullable: true),
                    Lu = table.Column<bool>(type: "boolean", nullable: false),
                    DateEnvoi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.IdMessage);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_IdConversation",
                        column: x => x.IdConversation,
                        principalTable: "Conversations",
                        principalColumn: "IdConversation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Utilisateurs_ExpediteurId",
                        column: x => x.ExpediteurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Biens",
                columns: table => new
                {
                    IdBien = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdProprietaire = table.Column<int>(type: "integer", nullable: false),
                    Titre = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Adresse = table.Column<string>(type: "text", nullable: false),
                    Ville = table.Column<string>(type: "text", nullable: false),
                    Quartier = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Surface = table.Column<int>(type: "integer", nullable: false),
                    Pieces = table.Column<int>(type: "integer", nullable: false),
                    LoyerMensuel = table.Column<decimal>(type: "numeric", nullable: false),
                    CautionMois = table.Column<int>(type: "integer", nullable: false),
                    Disponibilite = table.Column<DateOnly>(type: "date", nullable: true),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    Video = table.Column<string>(type: "text", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valide = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biens", x => x.IdBien);
                    table.ForeignKey(
                        name: "FK_Biens_Proprietaires_IdProprietaire",
                        column: x => x.IdProprietaire,
                        principalTable: "Proprietaires",
                        principalColumn: "IdProprio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Virements",
                columns: table => new
                {
                    IdVirement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdProprio = table.Column<int>(type: "integer", nullable: false),
                    IbanDestination = table.Column<string>(type: "text", nullable: false),
                    MontantNet = table.Column<decimal>(type: "numeric", nullable: false),
                    Commission = table.Column<decimal>(type: "numeric", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    DateVirement = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceBanque = table.Column<string>(type: "text", nullable: true),
                    Operateur = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Virements", x => x.IdVirement);
                    table.ForeignKey(
                        name: "FK_Virements_Proprietaires_IdProprio",
                        column: x => x.IdProprio,
                        principalTable: "Proprietaires",
                        principalColumn: "IdProprio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidatures",
                columns: table => new
                {
                    IdCandidature = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdBien = table.Column<int>(type: "integer", nullable: false),
                    IdLocataire = table.Column<int>(type: "integer", nullable: false),
                    MessageMotivation = table.Column<string>(type: "text", nullable: true),
                    RevenusMenuels = table.Column<decimal>(type: "numeric", nullable: false),
                    PieceIdentiteUrl = table.Column<string>(type: "text", nullable: true),
                    JustificatifUrl = table.Column<string>(type: "text", nullable: true),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    DateCandidature = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatures", x => x.IdCandidature);
                    table.ForeignKey(
                        name: "FK_Candidatures_Biens_IdBien",
                        column: x => x.IdBien,
                        principalTable: "Biens",
                        principalColumn: "IdBien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidatures_Locataires_IdLocataire",
                        column: x => x.IdLocataire,
                        principalTable: "Locataires",
                        principalColumn: "IdLocataire",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favoris",
                columns: table => new
                {
                    IdFavori = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    IdBien = table.Column<int>(type: "integer", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoris", x => x.IdFavori);
                    table.ForeignKey(
                        name: "FK_Favoris_Biens_IdBien",
                        column: x => x.IdBien,
                        principalTable: "Biens",
                        principalColumn: "IdBien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favoris_Utilisateurs_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    IdLocation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdBien = table.Column<int>(type: "integer", nullable: false),
                    IdLocataire = table.Column<int>(type: "integer", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "date", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "date", nullable: true),
                    LoyerMensuel = table.Column<decimal>(type: "numeric", nullable: false),
                    Caution = table.Column<decimal>(type: "numeric", nullable: false),
                    CautionPayee = table.Column<bool>(type: "boolean", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    BailUrl = table.Column<string>(type: "text", nullable: true),
                    DateSignature = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.IdLocation);
                    table.ForeignKey(
                        name: "FK_Locations_Biens_IdBien",
                        column: x => x.IdBien,
                        principalTable: "Biens",
                        principalColumn: "IdBien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Locataires_IdLocataire",
                        column: x => x.IdLocataire,
                        principalTable: "Locataires",
                        principalColumn: "IdLocataire",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visites",
                columns: table => new
                {
                    IdVisite = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdBien = table.Column<int>(type: "integer", nullable: false),
                    IdAgent = table.Column<int>(type: "integer", nullable: false),
                    IdLocataire = table.Column<int>(type: "integer", nullable: false),
                    DateVisite = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompteRendu = table.Column<string>(type: "text", nullable: true),
                    Statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visites", x => x.IdVisite);
                    table.ForeignKey(
                        name: "FK_Visites_Agents_IdAgent",
                        column: x => x.IdAgent,
                        principalTable: "Agents",
                        principalColumn: "IdAgent",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visites_Biens_IdBien",
                        column: x => x.IdBien,
                        principalTable: "Biens",
                        principalColumn: "IdBien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visites_Locataires_IdLocataire",
                        column: x => x.IdLocataire,
                        principalTable: "Locataires",
                        principalColumn: "IdLocataire",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interventions",
                columns: table => new
                {
                    IdIntervention = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdLocation = table.Column<int>(type: "integer", nullable: false),
                    IdArtisan = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Urgence = table.Column<string>(type: "text", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    DateSignalement = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateIntervention = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DevisMontant = table.Column<decimal>(type: "numeric", nullable: true),
                    DevisValide = table.Column<bool>(type: "boolean", nullable: false),
                    FactureUrl = table.Column<string>(type: "text", nullable: true),
                    NoteLocataire = table.Column<int>(type: "integer", nullable: true),
                    Commentaire = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interventions", x => x.IdIntervention);
                    table.ForeignKey(
                        name: "FK_Interventions_Artisans_IdArtisan",
                        column: x => x.IdArtisan,
                        principalTable: "Artisans",
                        principalColumn: "IdArtisan",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Interventions_Locations_IdLocation",
                        column: x => x.IdLocation,
                        principalTable: "Locations",
                        principalColumn: "IdLocation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Paiements",
                columns: table => new
                {
                    IdPaiement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdLocation = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Montant = table.Column<decimal>(type: "numeric", nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    Operateur = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    Commission = table.Column<decimal>(type: "numeric", nullable: false),
                    MontantNet = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiements", x => x.IdPaiement);
                    table.ForeignKey(
                        name: "FK_Paiements_Locations_IdLocation",
                        column: x => x.IdLocation,
                        principalTable: "Locations",
                        principalColumn: "IdLocation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    IdPhoto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntiteId = table.Column<int>(type: "integer", nullable: false),
                    EntiteType = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Ordre = table.Column<int>(type: "integer", nullable: false),
                    DateUpload = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BienIdBien = table.Column<int>(type: "integer", nullable: true),
                    InterventionIdIntervention = table.Column<int>(type: "integer", nullable: true),
                    VisiteIdVisite = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.IdPhoto);
                    table.ForeignKey(
                        name: "FK_Photos_Biens_BienIdBien",
                        column: x => x.BienIdBien,
                        principalTable: "Biens",
                        principalColumn: "IdBien");
                    table.ForeignKey(
                        name: "FK_Photos_Interventions_InterventionIdIntervention",
                        column: x => x.InterventionIdIntervention,
                        principalTable: "Interventions",
                        principalColumn: "IdIntervention");
                    table.ForeignKey(
                        name: "FK_Photos_Visites_VisiteIdVisite",
                        column: x => x.VisiteIdVisite,
                        principalTable: "Visites",
                        principalColumn: "IdVisite");
                });

            migrationBuilder.CreateTable(
                name: "PaiementsIntervention",
                columns: table => new
                {
                    IdPaiementInterv = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPaiement = table.Column<int>(type: "integer", nullable: false),
                    IdArtisan = table.Column<int>(type: "integer", nullable: false),
                    MontantArtisan = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaiementsIntervention", x => x.IdPaiementInterv);
                    table.ForeignKey(
                        name: "FK_PaiementsIntervention_Artisans_IdArtisan",
                        column: x => x.IdArtisan,
                        principalTable: "Artisans",
                        principalColumn: "IdArtisan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaiementsIntervention_Paiements_IdPaiement",
                        column: x => x.IdPaiement,
                        principalTable: "Paiements",
                        principalColumn: "IdPaiement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaiementsLoyer",
                columns: table => new
                {
                    IdPaiementLoyer = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPaiement = table.Column<int>(type: "integer", nullable: false),
                    MoisConcerne = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaiementsLoyer", x => x.IdPaiementLoyer);
                    table.ForeignKey(
                        name: "FK_PaiementsLoyer_Paiements_IdPaiement",
                        column: x => x.IdPaiement,
                        principalTable: "Paiements",
                        principalColumn: "IdPaiement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_IdUser",
                table: "Agents",
                column: "IdUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artisans_IdUser",
                table: "Artisans",
                column: "IdUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Biens_IdProprietaire",
                table: "Biens",
                column: "IdProprietaire");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_IdBien",
                table: "Candidatures",
                column: "IdBien");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_IdLocataire",
                table: "Candidatures",
                column: "IdLocataire");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_Participant1Id",
                table: "Conversations",
                column: "Participant1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_Participant2Id",
                table: "Conversations",
                column: "Participant2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Favoris_IdBien",
                table: "Favoris",
                column: "IdBien");

            migrationBuilder.CreateIndex(
                name: "IX_Favoris_IdUser",
                table: "Favoris",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_IdArtisan",
                table: "Interventions",
                column: "IdArtisan");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_IdLocation",
                table: "Interventions",
                column: "IdLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Locataires_IdUser",
                table: "Locataires",
                column: "IdUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_IdBien",
                table: "Locations",
                column: "IdBien");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_IdLocataire",
                table: "Locations",
                column: "IdLocataire");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ExpediteurId",
                table: "Messages",
                column: "ExpediteurId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IdConversation",
                table: "Messages",
                column: "IdConversation");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IdUser",
                table: "Notifications",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_IdLocation",
                table: "Paiements",
                column: "IdLocation");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementsIntervention_IdArtisan",
                table: "PaiementsIntervention",
                column: "IdArtisan");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementsIntervention_IdPaiement",
                table: "PaiementsIntervention",
                column: "IdPaiement",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaiementsLoyer_IdPaiement",
                table: "PaiementsLoyer",
                column: "IdPaiement",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_BienIdBien",
                table: "Photos",
                column: "BienIdBien");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_InterventionIdIntervention",
                table: "Photos",
                column: "InterventionIdIntervention");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_VisiteIdVisite",
                table: "Photos",
                column: "VisiteIdVisite");

            migrationBuilder.CreateIndex(
                name: "IX_Proprietaires_IdUser",
                table: "Proprietaires",
                column: "IdUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Virements_IdProprio",
                table: "Virements",
                column: "IdProprio");

            migrationBuilder.CreateIndex(
                name: "IX_Visites_IdAgent",
                table: "Visites",
                column: "IdAgent");

            migrationBuilder.CreateIndex(
                name: "IX_Visites_IdBien",
                table: "Visites",
                column: "IdBien");

            migrationBuilder.CreateIndex(
                name: "IX_Visites_IdLocataire",
                table: "Visites",
                column: "IdLocataire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidatures");

            migrationBuilder.DropTable(
                name: "Favoris");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PaiementsIntervention");

            migrationBuilder.DropTable(
                name: "PaiementsLoyer");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Virements");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Paiements");

            migrationBuilder.DropTable(
                name: "Interventions");

            migrationBuilder.DropTable(
                name: "Visites");

            migrationBuilder.DropTable(
                name: "Artisans");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Biens");

            migrationBuilder.DropTable(
                name: "Locataires");

            migrationBuilder.DropTable(
                name: "Proprietaires");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
