using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>OTP code queries.</summary>
public interface IOtpCodeRepository : IRepository<OtpCode>
{
    /// <summary>Get the latest unused, non-expired OTP for a user.</summary>
    Task<OtpCode?> GetActiveAsync(string userId, CancellationToken ct = default);

    /// <summary>Invalidate all unused OTPs for a user (before issuing a new one).</summary>
    Task InvalidateAllForUserAsync(string userId, CancellationToken ct = default);
}
