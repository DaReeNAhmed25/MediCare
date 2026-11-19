using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>
/// Appointment-specific query methods used by AppointmentService and ScheduleService.
/// </summary>
public interface IAppointmentRepository : IRepository<Appointment>
{
    /// <summary>Get all appointments for a doctor on a specific date, including Patient and User.</summary>
    Task<IEnumerable<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateOnly date, CancellationToken ct = default);

    /// <summary>Get all appointments for a patient (all statuses), newest first.</summary>
    Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId, CancellationToken ct = default);

    /// <summary>
    /// Find active (non-Cancelled, non-Rejected) appointments for a doctor that
    /// overlap with the given time range on the given date – used for conflict detection.
    /// </summary>
    Task<IEnumerable<Appointment>> GetConflictingAsync(
        int doctorId,
        DateOnly date,
        TimeSpan startTime,
        TimeSpan endTime,
        int? excludeAppointmentId = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get future Pending or Confirmed appointments for a doctor in a date range.
    /// Used by BR-06 (schedule change conflict check).
    /// </summary>
    Task<IEnumerable<Appointment>> GetFutureActiveForDoctorAsync(int doctorId, DateOnly fromDate, CancellationToken ct = default);

    /// <summary>Get appointment with full navigation (Patient.User, Doctor.User, MedicalRecord).</summary>
    Task<Appointment?> GetWithDetailsAsync(int appointmentId, CancellationToken ct = default);

    /// <summary>Get appointments for the daily report on a given date.</summary>
    Task<IEnumerable<Appointment>> GetByDateAsync(DateOnly date, CancellationToken ct = default);

    /// <summary>Get appointments in a date range for monthly chart data.</summary>
    Task<IEnumerable<Appointment>> GetInDateRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
}
