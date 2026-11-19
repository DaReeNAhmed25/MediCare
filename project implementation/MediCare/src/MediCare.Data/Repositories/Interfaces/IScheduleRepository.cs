using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>Schedule and vacation-day queries.</summary>
public interface IScheduleRepository : IRepository<DoctorSchedule>
{
    /// <summary>Get all schedules for a doctor.</summary>
    Task<IEnumerable<DoctorSchedule>> GetByDoctorAsync(int doctorId, CancellationToken ct = default);

    /// <summary>Get all vacation days for a doctor in a date range.</summary>
    Task<IEnumerable<VacationDay>> GetVacationDaysAsync(int doctorId, DateOnly from, DateOnly to, CancellationToken ct = default);
}
