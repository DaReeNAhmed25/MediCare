using MediCare.Data.Entities;
using MediCare.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Patient?> GetByUserIdAsync(string userId, CancellationToken ct = default)
        => await _dbSet
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

    public async Task<Patient?> GetWithUserAsync(int patientId, CancellationToken ct = default)
        => await _dbSet
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == patientId, ct);

    public async Task<IEnumerable<Patient>> GetByDoctorAsync(int doctorId, CancellationToken ct = default)
        => await _dbSet
            .Include(p => p.User)
            .Where(p => p.Appointments.Any(a => a.DoctorId == doctorId))
            .Distinct()
            .ToListAsync(ct);

    public async Task<string> GeneratePatientCodeAsync(CancellationToken ct = default)
    {
        // Ignore global query filter to find highest code even among deleted patients
        var maxCode = await _dbSet.IgnoreQueryFilters()
            .OrderByDescending(p => p.Id)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(ct);

        return $"P-{(maxCode + 1):D4}";
    }

    public async Task<IEnumerable<Patient>> GetAllWithUsersAsync(CancellationToken ct = default)
        => await _dbSet
            .Include(p => p.User)
            .OrderBy(p => p.User.FullName)
            .ToListAsync(ct);
}
