using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class ScheduleRepository : Repository<DoctorSchedule>, IScheduleRepository
{
    public ScheduleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<DoctorSchedule>> GetByDoctorAsync(int doctorId, CancellationToken ct = default)
        => await _dbSet
            .Where(s => s.DoctorId == doctorId)
            .OrderBy(s => s.DayOfWeek)
            .ToListAsync(ct);

    public async Task<IEnumerable<VacationDay>> GetVacationDaysAsync(
        int doctorId, DateOnly from, DateOnly to, CancellationToken ct = default)
        => await _context.VacationDays
            .Where(v => v.DoctorId == doctorId && v.VacationDate >= from && v.VacationDate <= to)
            .ToListAsync(ct);
}
