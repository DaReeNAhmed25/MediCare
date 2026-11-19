using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>Patient-specific queries.</summary>
public interface IPatientRepository : IRepository<Patient>
{
    /// <summary>Get patient by UserId (for the logged-in patient's own profile).</summary>
    Task<Patient?> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>Get patient with User navigation loaded.</summary>
    Task<Patient?> GetWithUserAsync(int patientId, CancellationToken ct = default);

    /// <summary>Get all patients visible to a specific doctor (have appointment with that doctor).</summary>
    Task<IEnumerable<Patient>> GetByDoctorAsync(int doctorId, CancellationToken ct = default);

    /// <summary>Get the next available PatientCode sequence number (for seeding/registration).</summary>
    Task<string> GeneratePatientCodeAsync(CancellationToken ct = default);

    /// <summary>Get all patients with their users for admin list.</summary>
    Task<IEnumerable<Patient>> GetAllWithUsersAsync(CancellationToken ct = default);
}
