namespace MediCare.Data.Entities;

/// <summary>
/// Audit entry recorded for Confirm, Reject and Cancel actions only (BR-08).
/// UserId may be null if the acting user was deleted.
/// </summary>
public class AuditLog
{
    public int Id { get; set; }

    /// <summary>Nullable – actor may have been soft-deleted.</summary>
    public string? UserId { get; set; }

    /// <summary>One of: "Confirm", "Reject", "Cancel".</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>e.g. "Appointment".</summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>PK of the affected entity as a string.</summary>
    public string EntityId { get; set; } = string.Empty;

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ApplicationUser? User { get; set; }
}
