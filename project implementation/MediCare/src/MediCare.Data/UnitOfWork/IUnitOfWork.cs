using MediCare.Data.Repositories.Interfaces;

namespace MediCare.Data.UnitOfWork;

/// <summary>
/// Unit of Work interface that bundles all repositories and coordinates saves/transactions.
/// Services depend on this interface, not the concrete implementation.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IAppointmentRepository Appointments { get; }
    IDoctorRepository Doctors { get; }
    IPatientRepository Patients { get; }
    IOtpCodeRepository OtpCodes { get; }
    INotificationRepository Notifications { get; }
    IMedicalRecordRepository MedicalRecords { get; }
    IScheduleRepository Schedules { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
