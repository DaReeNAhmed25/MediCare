using MediCare.Data.Repositories;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace MediCare.Data.UnitOfWork;

/// <summary>
/// Concrete Unit of Work. All repository instances share the same DbContext
/// so they participate in the same transaction and EF identity map.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    // Lazily-initialized repositories (created once per UoW lifetime)
    private AppointmentRepository? _appointments;
    private DoctorRepository? _doctors;
    private PatientRepository? _patients;
    private OtpCodeRepository? _otpCodes;
    private NotificationRepository? _notifications;
    private MedicalRecordRepository? _medicalRecords;
    private ScheduleRepository? _schedules;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IAppointmentRepository Appointments
        => _appointments ??= new AppointmentRepository(_context);

    public IDoctorRepository Doctors
        => _doctors ??= new DoctorRepository(_context);

    public IPatientRepository Patients
        => _patients ??= new PatientRepository(_context);

    public IOtpCodeRepository OtpCodes
        => _otpCodes ??= new OtpCodeRepository(_context);

    public INotificationRepository Notifications
        => _notifications ??= new NotificationRepository(_context);

    public IMedicalRecordRepository MedicalRecords
        => _medicalRecords ??= new MedicalRecordRepository(_context);

    public IScheduleRepository Schedules
        => _schedules ??= new ScheduleRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
