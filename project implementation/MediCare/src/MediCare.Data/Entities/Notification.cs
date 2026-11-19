namespace MediCare.Data.Entities;

/// <summary>Notification status values.</summary>
public enum NotificationStatus
{
    Pending,
    Sent,
    Failed
}

/// <summary>
/// Every email sent via SendGrid is logged here (FR-28 / BR-10).
/// </summary>
public class Notification
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    /// <summary>e.g. "OtpCode", "AppointmentBooked", "AppointmentConfirmed", etc.</summary>
    public string Type { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? SentAt { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
}
