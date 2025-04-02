using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudanzaApp.Data.Models;


namespace MudanzaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Mudanza> Mudanzas { get; set; }
        public DbSet<MudanzaItem> MudanzaItems { get; set; }
        public DbSet<MudanzaComment> MudanzaComments { get; set; }
        public DbSet<MudanzaCollaborator> MudanzaCollaborators { get; set; }
        public DbSet<MudanzaStatusHistory> MudanzaStatusHistory { get; set; }
        public DbSet<PhotoCredit> PhotoCredits { get; set; }
        public DbSet<PhotoCreditPurchase> PhotoCreditPurchases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de relaciones y restricciones
            builder.Entity<Mudanza>()
                .HasOne(m => m.User)
                .WithMany(u => u.Mudanzas)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MudanzaItem>()
                .HasOne(i => i.Mudanza)
                .WithMany(m => m.Items)
                .HasForeignKey(i => i.MudanzaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MudanzaComment>()
                .HasOne(c => c.Mudanza)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MudanzaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MudanzaComment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MudanzaCollaborator>()
                .HasOne(c => c.Mudanza)
                .WithMany(m => m.Collaborators)
                .HasForeignKey(c => c.MudanzaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MudanzaCollaborator>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.Entity<MudanzaStatusHistory>()
                .HasOne(h => h.Mudanza)
                .WithMany(m => m.StatusHistory)
                .HasForeignKey(h => h.MudanzaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MudanzaStatusHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PhotoCredit>()
            .HasOne(p => p.User)
                .WithOne(u => u.PhotoCredit)
                .HasForeignKey<PhotoCredit>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PhotoCreditPurchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.PhotoCreditPurchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.Entity<Mudanza>()
                .HasIndex(m => m.SharingCode)
                .IsUnique();

            builder.Entity<Mudanza>()
                .HasIndex(m => m.Status);

            builder.Entity<Mudanza>()
                .HasIndex(m => m.UserId);

            builder.Entity<MudanzaItem>()
                .HasIndex(i => i.MudanzaId);

            builder.Entity<MudanzaComment>()
                .HasIndex(c => c.MudanzaId);

            builder.Entity<MudanzaCollaborator>()
                .HasIndex(c => c.MudanzaId);

            builder.Entity<MudanzaCollaborator>()
                 .HasIndex(c => new { c.MudanzaId, c.Email })
                 .IsUnique()
                 .HasFilter("\"Email\" IS NOT NULL");

            builder.Entity<PhotoCreditPurchase>()
                .HasIndex(p => p.StripeSessionId)
                .IsUnique()
                .HasFilter("\"StripeSessionId\" IS NOT NULL");

        }
    }
}
