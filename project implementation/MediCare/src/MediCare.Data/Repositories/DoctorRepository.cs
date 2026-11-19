using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Doctor>> SearchAsync(
        int? specializationId,
        string? locationKeyword,
        DateOnly? availableOn,
        CancellationToken ct = default)
    {
        var query = _dbSet
            .Include(d => d.User)
            .Include(d => d.Specialization)
            .Include(d => d.Schedules)
            .Include(d => d.VacationDays)
            .AsQueryable();

        if (specializationId.HasValue)
            query = query.Where(d => d.SpecializationId == specializationId.Value);

        if (!string.IsNullOrWhiteSpace(locationKeyword))
            query = query.Where(d => d.Location != null && d.Location.Contains(locationKeyword));

        if (availableOn.HasValue)
        {
            var dayOfWeek = (int)availableOn.Value.DayOfWeek;
            query = query
                .Where(d =>
                    d.Schedules.Any(s => s.DayOfWeek == dayOfWeek) &&
                    !d.VacationDays.Any(v => v.VacationDate == availableOn.Value));
        }

        return await query.ToListAsync(ct);
    }

    public async Task<Doctor?> GetWithDetailsAsync(int doctorId, CancellationToken ct = default)
        => await _dbSet
            .Include(d => d.User)
            .Include(d => d.Specialization)
            .Include(d => d.Schedules)
            .Include(d => d.VacationDays)
            .FirstOrDefaultAsync(d => d.Id == doctorId, ct);

    public async Task<Doctor?> GetByUserIdAsync(string userId, CancellationToken ct = default)
        => await _dbSet
            .Include(d => d.Specialization)
            .Include(d => d.Schedules)
            .Include(d => d.VacationDays)
            .FirstOrDefaultAsync(d => d.UserId == userId, ct);

    public async Task<IEnumerable<Doctor>> GetAllWithUsersAsync(CancellationToken ct = default)
        => await _dbSet
            .Include(d => d.User)
            .Include(d => d.Specialization)
            .OrderBy(d => d.User.FullName)
            .ToListAsync(ct);
}
