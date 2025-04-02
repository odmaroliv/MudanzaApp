using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudanzaApp.Data.Models
{
    public enum MudanzaStatus
    {
        DraftOpen,           // Borrador abierto
        DraftClosed,         // Borrador cerrado
        InReview,            // En revisión
        WaitingForDocuments, // En espera de documentos
        ReceivedInWarehouseUS, // Recibido en almacén USA
        ReceivedInWarehouseMX, // Recibido en almacén México
        WaitingForCrossing,  // En espera para cruce
        WaitingForShipment,  // En espera para envío
        InTransit,           // En tránsito
        Delivered,           // Entregado
        Completed            // Completado
    }

    public enum BuildingType
    {
        House,              // Casa
        Apartment,          // Departamento
        Commercial,         // Local comercial
        Other               // Otro
    }

    public class Mudanza
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? CollaboratorName { get; set; }

        [Required]
        public string OriginLocation { get; set; }

        [Required]
        public string DestinationLocation { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public MudanzaStatus Status { get; set; } = MudanzaStatus.DraftOpen;

        // Código único para compartir
        [Required]
        public string SharingCode { get; set; } = Guid.NewGuid().ToString("N");

        // Detalles adicionales
        public bool NeedsPickup { get; set; }

        public BuildingType? OriginBuildingType { get; set; }

        public BuildingType? DestinationBuildingType { get; set; }

        public bool HasStairs { get; set; } = false;

        public bool NeedsCrane { get; set; } = false;

        public bool DirtRoad { get; set; } = false;

        public string? AccessCode { get; set; }

        public string? VehicleRestrictions { get; set; }

        // Información de contacto adicional
        public string? AlternateContact1Name { get; set; }

        public string? AlternateContact1Phone { get; set; }

        public string? AlternateContact1Email { get; set; }

        public string? AlternateContact2Name { get; set; }

        public string? AlternateContact2Phone { get; set; }

        public string? AlternateContact2Email { get; set; }

        // Información financiera
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EstimatedCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalCost { get; set; }

        public string? PaymentStatus { get; set; }

        // Colecciones
        public virtual ICollection<MudanzaItem> Items { get; set; } = new List<MudanzaItem>();

        public virtual ICollection<MudanzaComment> Comments { get; set; } = new List<MudanzaComment>();

        public virtual ICollection<MudanzaCollaborator> Collaborators { get; set; } = new List<MudanzaCollaborator>();

        public virtual ICollection<MudanzaStatusHistory> StatusHistory { get; set; } = new List<MudanzaStatusHistory>();
    }

    public class MudanzaItem
    {
        public int Id { get; set; }

        public int MudanzaId { get; set; }

        public virtual Mudanza Mudanza { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        [Range(0, double.MaxValue)]
        public double Weight { get; set; }

        public bool IsWeightTotal { get; set; } = false;

        public bool IsNew { get; set; } = false;

        public double? Length { get; set; }

        public double? Width { get; set; }

        public double? Height { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeclaredValue { get; set; }

        public bool IsValueTotal { get; set; } = false;

        public string Description { get; set; }

        public bool IsHazmat { get; set; } = false;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public string CreatedBy { get; set; }

        public string? PhotoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    public class MudanzaComment
    {
        public int Id { get; set; }

        public int MudanzaId { get; set; }

        public virtual Mudanza Mudanza { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsAdminComment { get; set; }
    }

    public class MudanzaCollaborator
    {
        public int Id { get; set; }

        public int MudanzaId { get; set; }

        public virtual Mudanza Mudanza { get; set; }

        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        [Required]
        public string CollaboratorName { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastAccess { get; set; }
    }

    public class MudanzaStatusHistory
    {
        public int Id { get; set; }

        public int MudanzaId { get; set; }

        public virtual Mudanza Mudanza { get; set; }

        [Required]
        public MudanzaStatus Status { get; set; }

        [Required]
        public string ChangedByUserId { get; set; }

        public virtual ApplicationUser ChangedByUser { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public string? Comments { get; set; }
    }

    public class PhotoCredit
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int AvailableCredits { get; set; } = 5; // 5 créditos gratuitos por defecto

        public DateTime LastPurchase { get; set; }
    }

    public class PhotoCreditPurchase
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int CreditsAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public string? StripeSessionId { get; set; }

        public string? StripePaymentIntentId { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }
    }

    // Extensión de ApplicationUser para Identity
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? WhatsAppNumber { get; set; }

        public bool IsAdmin { get; set; } = false;

        public string? PreferredLanguage { get; set; } = "en";

        public virtual ICollection<Mudanza> Mudanzas { get; set; } = new List<Mudanza>();

        public virtual PhotoCredit? PhotoCredit { get; set; }

        public virtual ICollection<PhotoCreditPurchase> PhotoCreditPurchases { get; set; } = new List<PhotoCreditPurchase>();
    }
}

