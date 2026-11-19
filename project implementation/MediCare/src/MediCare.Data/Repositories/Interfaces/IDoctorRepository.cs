using MediCare.Data.Entities;

namespace MediCare.Data.Repositories.Interfaces;

/// <summary>Doctor-specific queries.</summary>
public interface IDoctorRepository : IRepository<Doctor>
{
    /// <summary>Search doctors by specialization, location keyword and/or availability on a date.</summary>
    Task<IEnumerable<Doctor>> SearchAsync(
        int? specializationId,
        string? locationKeyword,
        DateOnly? availableOn,
        CancellationToken ct = default);

    /// <summary>Get doctor with Specialization, Schedules and VacationDays.</summary>
    Task<Doctor?> GetWithDetailsAsync(int doctorId, CancellationToken ct = default);

    /// <summary>Get doctor by UserId (for the logged-in doctor's own profile).</summary>
    Task<Doctor?> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>Get all doctors with their users (for the directory / admin list).</summary>
    Task<IEnumerable<Doctor>> GetAllWithUsersAsync(CancellationToken ct = default);
}
