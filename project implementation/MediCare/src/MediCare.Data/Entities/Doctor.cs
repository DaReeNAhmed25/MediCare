namespace MediCare.Data.Entities;

/// <summary>Doctor profile linked to an ApplicationUser account.</summary>
public class Doctor
{
    public int Id { get; set; }

    /// <summary>FK to AspNetUsers. One user, one doctor profile.</summary>
    public string UserId { get; set; } = string.Empty;

    public int SpecializationId { get; set; }

    /// <summary>City / area where the doctor practices.</summary>
    public string? Location { get; set; }

    /// <summary>Consultation fee in EGP. Must be >= 0.</summary>
    public decimal ConsultationFee { get; set; }

    public string? RoomNumber { get; set; }

    /// <summary>Relative path or URL of the profile image.</summary>
    public string? ProfileImage { get; set; }

    /// <summary>Soft-delete flag.</summary>
    public bool IsDeleted { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public Specialization Specialization { get; set; } = null!;
    public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    public ICollection<VacationDay> VacationDays { get; set; } = new List<VacationDay>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
