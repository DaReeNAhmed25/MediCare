namespace MediCare.Data.Entities;

/// <summary>
/// Medical record for a Confirmed appointment.
/// Saving a record marks the appointment Completed in the same transaction.
/// No signing/locking/addenda per SPEC.md.
/// </summary>
public class MedicalRecord
{
    public int Id { get; set; }

    /// <summary>FK to Appointment; unique (one record per appointment).</summary>
    public int AppointmentId { get; set; }

    /// <summary>e.g. "120/80 mmHg"</summary>
    public string? BloodPressure { get; set; }

    /// <summary>Beats per minute. Constraint: 20–300.</summary>
    public int? HeartRate { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentNotes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Appointment Appointment { get; set; } = null!;
    public Prescription? Prescription { get; set; }
}
