namespace MediCare.Data.Entities;

/// <summary>Digital prescription attached to a medical record.</summary>
public class Prescription
{
    public int Id { get; set; }

    /// <summary>FK to MedicalRecord; unique (one prescription per record).</summary>
    public int MedicalRecordId { get; set; }

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public MedicalRecord MedicalRecord { get; set; } = null!;
    public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}
