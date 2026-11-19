using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>Medical record queries.</summary>
public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    /// <summary>Get the medical record for an appointment, including Prescription and Items.</summary>
    Task<MedicalRecord?> GetByAppointmentAsync(int appointmentId, CancellationToken ct = default);

    /// <summary>Get all medical records for a patient (via their appointments).</summary>
    Task<IEnumerable<MedicalRecord>> GetByPatientAsync(int patientId, CancellationToken ct = default);

    /// <summary>Get a medical record with full navigation (Appointment, Doctor, Prescription items).</summary>
    Task<MedicalRecord?> GetWithDetailsAsync(int recordId, CancellationToken ct = default);
}
