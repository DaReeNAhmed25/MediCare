using Microsoft.AspNetCore.Identity;

namespace MediCare.Data.Entities;

/// <summary>
/// Extends ASP.NET Identity user with clinic-specific fields.
/// Soft delete: IsDeleted = true prevents login.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Full display name (first + last).</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>True after the user has successfully verified their OTP.</summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>Admin can suspend a user to block login without deleting.</summary>
    public bool IsSuspended { get; set; }

    /// <summary>Soft-delete flag; deleted users cannot log in.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>UTC timestamp of account creation.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
