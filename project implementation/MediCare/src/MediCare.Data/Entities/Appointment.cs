namespace MediCare.Data.Entities;

/// <summary>
/// An appointment between a patient and a doctor.
/// Dates/times are clinic local time.
/// RowVersion provides optimistic concurrency for double-booking prevention.
/// </summary>
public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    /// <summary>Clinic local date.</summary>
    public DateOnly AppointmentDate { get; set; }

    /// <summary>Clinic local start time.</summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>Clinic local end time.</summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>Appointment status; default Pending.</summary>
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    /// <summary>Optional patient/admin notes.</summary>
    public string? Notes { get; set; }

    /// <summary>Concurrency token – prevents double-booking race conditions.</summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    /// <summary>UTC creation timestamp.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public MedicalRecord? MedicalRecord { get; set; }
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
