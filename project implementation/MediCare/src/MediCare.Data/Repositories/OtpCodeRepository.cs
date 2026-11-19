using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class OtpCodeRepository : Repository<OtpCode>, IOtpCodeRepository
{
    public OtpCodeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<OtpCode?> GetActiveAsync(string userId, CancellationToken ct = default)
        => await _dbSet
            .Where(o => o.UserId == userId
                     && !o.IsUsed
                     && o.ExpiresAt > DateTime.UtcNow
                     && o.FailedAttempts < 5)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(ct);

    public async Task InvalidateAllForUserAsync(string userId, CancellationToken ct = default)
    {
        var codes = await _dbSet
            .Where(o => o.UserId == userId && !o.IsUsed)
            .ToListAsync(ct);

        foreach (var code in codes)
            code.IsUsed = true;
    }
}
