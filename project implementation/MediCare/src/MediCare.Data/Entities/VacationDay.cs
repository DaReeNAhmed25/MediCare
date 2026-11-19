namespace MediCare.Data.Entities;

/// <summary>A single day on which a doctor is not available.</summary>
public class VacationDay
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    /// <summary>The date of the vacation (date only, clinic local).</summary>
    public DateOnly VacationDate { get; set; }

    public string? Reason { get; set; }

    // Navigation
    public Doctor Doctor { get; set; } = null!;
}
