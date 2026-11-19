namespace MediCare.Data.Entities;

/// <summary>
/// Patient profile data. The patient's name comes from ApplicationUser.FullName.
/// UserId is required (no walk-in patients).
/// </summary>
public class Patient
{
    public int Id { get; set; }

    /// <summary>FK to AspNetUsers. Required – no walk-in patients.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Human-readable patient code, e.g. "P-1042".</summary>
    public string PatientCode { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    /// <summary>One of: A+, A-, B+, B-, AB+, AB-, O+, O-</summary>
    public string? BloodGroup { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }

    public bool IsDeleted { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
