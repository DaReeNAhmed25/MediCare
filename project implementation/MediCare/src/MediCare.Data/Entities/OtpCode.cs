namespace MediCare.Data.Entities;

/// <summary>
/// A one-time-use OTP code for email verification.
/// Stored as a BCrypt/SHA256 hash – never the raw code.
/// Locks after 5 failed attempts.
/// </summary>
public class OtpCode
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    /// <summary>Hashed OTP (never stored in plain text).</summary>
    public string CodeHash { get; set; } = string.Empty;

    /// <summary>UTC expiry time (10 minutes from creation).</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>Number of wrong attempts; max 5 before lock.</summary>
    public int FailedAttempts { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ApplicationUser User { get; set; } = null!;
}
