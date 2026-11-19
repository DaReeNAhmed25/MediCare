using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>Notification queries for the email log.</summary>
public interface INotificationRepository : IRepository<Notification>
{
    /// <summary>Get all Failed notifications for retry.</summary>
    Task<IEnumerable<Notification>> GetFailedAsync(CancellationToken ct = default);

    /// <summary>Get notification history for a user.</summary>
    Task<IEnumerable<Notification>> GetByUserAsync(string userId, CancellationToken ct = default);
}
