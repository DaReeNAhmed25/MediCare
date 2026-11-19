using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class MedicalRecordRepository : Repository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(ApplicationDbContext context) : base(context) { }

    public async Task<MedicalRecord?> GetByAppointmentAsync(int appointmentId, CancellationToken ct = default)
        => await _dbSet
            .Include(m => m.Prescription).ThenInclude(p => p!.Items)
            .FirstOrDefaultAsync(m => m.AppointmentId == appointmentId, ct);

    public async Task<IEnumerable<MedicalRecord>> GetByPatientAsync(int patientId, CancellationToken ct = default)
        => await _dbSet
            .Include(m => m.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(m => m.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.Specialization)
            .Include(m => m.Prescription).ThenInclude(p => p!.Items)
            .Where(m => m.Appointment.PatientId == patientId)
            .OrderByDescending(m => m.Appointment.AppointmentDate)
            .ToListAsync(ct);

    public async Task<MedicalRecord?> GetWithDetailsAsync(int recordId, CancellationToken ct = default)
        => await _dbSet
            .Include(m => m.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(m => m.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(m => m.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.Specialization)
            .Include(m => m.Prescription).ThenInclude(p => p!.Items)
            .FirstOrDefaultAsync(m => m.Id == recordId, ct);
}
