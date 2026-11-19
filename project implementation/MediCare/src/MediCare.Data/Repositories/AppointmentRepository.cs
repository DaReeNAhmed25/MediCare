using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Appointment>> GetByDoctorAndDateAsync(
        int doctorId, DateOnly date, CancellationToken ct = default)
        => await _dbSet
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
            .OrderBy(a => a.StartTime)
            .ToListAsync(ct);

    public async Task<IEnumerable<Appointment>> GetByPatientAsync(
        int patientId, CancellationToken ct = default)
        => await _dbSet
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Specialization)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenByDescending(a => a.StartTime)
            .ToListAsync(ct);

    public async Task<IEnumerable<Appointment>> GetConflictingAsync(
        int doctorId, DateOnly date, TimeSpan startTime, TimeSpan endTime,
        int? excludeAppointmentId = null, CancellationToken ct = default)
        => await _dbSet
            .Where(a => a.DoctorId == doctorId
                     && a.AppointmentDate == date
                     && a.Status != AppointmentStatus.Cancelled
                     && a.Status != AppointmentStatus.Rejected
                     && (excludeAppointmentId == null || a.Id != excludeAppointmentId)
                     && a.StartTime < endTime
                     && a.EndTime > startTime)
            .ToListAsync(ct);

    public async Task<IEnumerable<Appointment>> GetFutureActiveForDoctorAsync(
        int doctorId, DateOnly fromDate, CancellationToken ct = default)
        => await _dbSet
            .Where(a => a.DoctorId == doctorId
                     && a.AppointmentDate >= fromDate
                     && (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed))
            .OrderBy(a => a.AppointmentDate).ThenBy(a => a.StartTime)
            .ToListAsync(ct);

    public async Task<Appointment?> GetWithDetailsAsync(int appointmentId, CancellationToken ct = default)
        => await _dbSet
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Specialization)
            .Include(a => a.MedicalRecord).ThenInclude(m => m!.Prescription).ThenInclude(p => p!.Items)
            .FirstOrDefaultAsync(a => a.Id == appointmentId, ct);

    public async Task<IEnumerable<Appointment>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
        => await _dbSet
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Where(a => a.AppointmentDate == date)
            .OrderBy(a => a.StartTime)
            .ToListAsync(ct);

    public async Task<IEnumerable<Appointment>> GetInDateRangeAsync(
        DateOnly from, DateOnly to, CancellationToken ct = default)
        => await _dbSet
            .Where(a => a.AppointmentDate >= from && a.AppointmentDate <= to)
            .ToListAsync(ct);
}
