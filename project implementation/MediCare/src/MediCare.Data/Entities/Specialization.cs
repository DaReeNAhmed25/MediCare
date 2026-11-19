namespace MediCare.Data.Entities;

/// <summary>Medical specialization (e.g. Cardiology, Dermatology).</summary>
public class Specialization
{
    public int Id { get; set; }

    /// <summary>Unique specialization name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Soft-delete. A specialization in use cannot be hard-deleted.</summary>
    public bool IsDeleted { get; set; }

    // Navigation
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
