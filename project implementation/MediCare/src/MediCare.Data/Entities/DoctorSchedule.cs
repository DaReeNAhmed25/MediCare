namespace MediCare.Data.Entities;

/// <summary>
/// Weekly recurring schedule entry for a doctor.
/// EndTime must be after StartTime; DayOfWeek 0 = Sunday … 6 = Saturday.
/// </summary>
public class DoctorSchedule
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    /// <summary>0 = Sunday, 1 = Monday, …, 6 = Saturday.</summary>
    public int DayOfWeek { get; set; }

    /// <summary>Clinic local time (time-of-day only).</summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>Clinic local time (time-of-day only). Must be > StartTime.</summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>Duration of one appointment slot in minutes.</summary>
    public int SlotMinutes { get; set; }

    // Navigation
    public Doctor Doctor { get; set; } = null!;
}
