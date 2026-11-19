namespace MediCare.Data.Entities;

/// <summary>A single medication line inside a digital prescription.</summary>
public class PrescriptionItem
{
    public int Id { get; set; }

    public int PrescriptionId { get; set; }

    public string Medication { get; set; } = string.Empty;

    public string Dosage { get; set; } = string.Empty;

    public string Frequency { get; set; } = string.Empty;

    // Navigation
    public Prescription Prescription { get; set; } = null!;
}
