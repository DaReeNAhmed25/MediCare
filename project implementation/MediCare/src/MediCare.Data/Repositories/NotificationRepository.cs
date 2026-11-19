using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Notification>> GetFailedAsync(CancellationToken ct = default)
        => await _dbSet
            .Where(n => n.Status == NotificationStatus.Failed)
            .OrderBy(n => n.CreatedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<Notification>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _dbSet
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(ct);
}
