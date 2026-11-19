using MediCare.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MediCare.Data.Seed;

/// <summary>
/// Idempotent database seeder. Run only in Development.
/// Creates roles, admin account, 10 specializations, 6 doctors, 10 patients.
/// All passwords are read from configuration (Seed:SamplePassword, Admin:Email, Admin:Password).
/// No real patient data.
/// </summary>
public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedAdminAsync();
            await SeedSpecializationsAsync();
            await SeedDoctorsAsync();
            await SeedPatientsAsync();
            await _context.SaveChangesAsync();
            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Roles
    // ─────────────────────────────────────────────────────────────────────────

    private async Task SeedRolesAsync()
    {
        string[] roles = ["Patient", "Doctor", "Admin"];
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
                _logger.LogInformation("Created role: {Role}", role);
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Admin
    // ─────────────────────────────────────────────────────────────────────────

    private async Task SeedAdminAsync()
    {
        var email = _configuration["Admin:Email"]
            ?? throw new InvalidOperationException("Admin:Email must be set in user-secrets or environment variables.");
        var password = _configuration["Admin:Password"]
            ?? throw new InvalidOperationException("Admin:Password must be set in user-secrets or environment variables.");

        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null) return;

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = "Clinic Administrator",
            IsEmailVerified = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(admin, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(admin, "Admin");
            _logger.LogInformation("Admin account created: {Email}", email);
        }
        else
        {
            _logger.LogWarning("Failed to create admin: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Specializations
    // ─────────────────────────────────────────────────────────────────────────

    private async Task SeedSpecializationsAsync()
    {
        string[] names =
        [
            "Cardiology", "Dermatology", "Pediatrics", "Orthopedics",
            "Dentistry", "Neurology", "Ophthalmology", "Gynecology",
            "Gastroenterology", "Psychiatry"
        ];

        foreach (var name in names)
        {
            if (!await _context.Specializations.IgnoreQueryFilters().AnyAsync(s => s.Name == name))
            {
                _context.Specializations.Add(new Specialization { Name = name });
                _logger.LogInformation("Added specialization: {Name}", name);
            }
        }

        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Doctors
    // ─────────────────────────────────────────────────────────────────────────

    private async Task SeedDoctorsAsync()
    {
        var samplePassword = _configuration["Seed:SamplePassword"]
            ?? throw new InvalidOperationException("Seed:SamplePassword must be set in user-secrets or environment variables.");

        var specs = await _context.Specializations.IgnoreQueryFilters().ToListAsync();

        var doctorData = new[]
        {
            new { Email = "mariam.ahmed@medicare.eg",    Name = "Dr. Mariam Ahmed",     SpecName = "Cardiology",    Location = "New Cairo", Fee = 500m, Room = "02" },
            new { Email = "omar.hassan@medicare.eg",     Name = "Dr. Omar Hassan",      SpecName = "Dermatology",   Location = "Maadi",     Fee = 450m, Room = "05" },
            new { Email = "salma.mahmoud@medicare.eg",  Name = "Dr. Salma Mahmoud",    SpecName = "Pediatrics",    Location = "Nasr City", Fee = 400m, Room = "08" },
            new { Email = "ahmed.mostafa@medicare.eg",  Name = "Dr. Ahmed Mostafa",    SpecName = "Orthopedics",   Location = "Heliopolis",Fee = 650m, Room = "03" },
            new { Email = "nour.ali@medicare.eg",       Name = "Dr. Nour El Din Ali",  SpecName = "Dentistry",     Location = "New Cairo", Fee = 350m, Room = "10" },
            new { Email = "hana.ibrahim@medicare.eg",   Name = "Dr. Hana Ibrahim",     SpecName = "Neurology",     Location = "Zamalek",   Fee = 600m, Room = "07" },
        };

        // Mon–Sat schedules (1–6)
        var weekDays = new[] { 1, 2, 3, 4, 5, 6 };

        foreach (var d in doctorData)
        {
            if (await _userManager.FindByEmailAsync(d.Email) is not null) continue;

            var spec = specs.FirstOrDefault(s => s.Name == d.SpecName);
            if (spec is null) continue;

            var user = new ApplicationUser
            {
                UserName = d.Email,
                Email = d.Email,
                FullName = d.Name,
                IsEmailVerified = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, samplePassword);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to create doctor {Name}: {Errors}", d.Name,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                continue;
            }

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                UserId = user.Id,
                SpecializationId = spec.Id,
                Location = d.Location,
                ConsultationFee = d.Fee,
                RoomNumber = d.Room,
            };

            // Create a Mon–Sat schedule with 30-min slots 09:00–15:00
            foreach (var dow in weekDays)
            {
                doctor.Schedules.Add(new DoctorSchedule
                {
                    DayOfWeek = dow,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0),
                    SlotMinutes = 30
                });
            }

            _context.Doctors.Add(doctor);
            _logger.LogInformation("Added doctor: {Name}", d.Name);
        }

        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Patients
    // ─────────────────────────────────────────────────────────────────────────

    private async Task SeedPatientsAsync()
    {
        var samplePassword = _configuration["Seed:SamplePassword"]
            ?? throw new InvalidOperationException("Seed:SamplePassword must be set in user-secrets or environment variables.");

        // Use (string?) cast on first element to establish nullable type for the anonymous array
        var patientData = new[]
        {
            new { Email = "ahmed.tarek@gmail.com",    Name = "Ahmed Tarek",    Phone = "+20 100 123 4567", DOB = new DateTime(1988, 5, 14), Blood = "O+",  History = "Essential hypertension", Allergies = (string?)"Penicillin" },
            new { Email = "menna.adel@gmail.com",     Name = "Menna Adel",     Phone = "+20 101 246 8101", DOB = new DateTime(1997, 3, 22), Blood = "A+",  History = "None",                    Allergies = (string?)null },
            new { Email = "youssef.hany@gmail.com",   Name = "Youssef Hany",   Phone = "+20 102 336 7741", DOB = new DateTime(2018, 7, 5),  Blood = "B+",  History = "None",                    Allergies = (string?)null },
            new { Email = "farida.m@gmail.com",       Name = "Farida Mohamed", Phone = "+20 100 778 4432", DOB = new DateTime(1972, 11, 3), Blood = "AB+", History = "Type 2 diabetes",          Allergies = (string?)"Aspirin" },
            new { Email = "khaled.ibrahim@gmail.com", Name = "Khaled Ibrahim", Phone = "+20 109 446 2210", DOB = new DateTime(1983, 9, 17), Blood = "O-",  History = "None",                    Allergies = (string?)null },
            new { Email = "reem.hussein@gmail.com",   Name = "Reem Hussein",   Phone = "+20 101 776 5502", DOB = new DateTime(1990, 1, 30), Blood = "A-",  History = "Asthma",                  Allergies = (string?)"NSAIDs" },
            new { Email = "dina.ashraf@gmail.com",    Name = "Dina Ashraf",    Phone = "+20 102 667 3320", DOB = new DateTime(1978, 6, 12), Blood = "B-",  History = "None",                    Allergies = (string?)null },
            new { Email = "mahmoud.saad@gmail.com",   Name = "Mahmoud Saad",   Phone = "+20 100 443 8861", DOB = new DateTime(1963, 4, 8),  Blood = "O+",  History = "Coronary artery disease",  Allergies = (string?)"Sulfa" },
            new { Email = "sara.hassan@gmail.com",    Name = "Sara Hassan",    Phone = "+20 103 554 9912", DOB = new DateTime(1995, 8, 20), Blood = "AB-", History = "None",                    Allergies = (string?)null },
            new { Email = "omar.fathy@gmail.com",     Name = "Omar Fathy",     Phone = "+20 109 112 3344", DOB = new DateTime(2001, 12, 1), Blood = "A+",  History = "None",                    Allergies = (string?)null },
        };

        // Get the max patient ID to generate codes
        var maxId = await _context.Patients.IgnoreQueryFilters()
            .OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefaultAsync();

        int codeCounter = maxId + 1;

        foreach (var pd in patientData)
        {
            if (await _userManager.FindByEmailAsync(pd.Email) is not null) continue;

            var user = new ApplicationUser
            {
                UserName = pd.Email,
                Email = pd.Email,
                FullName = pd.Name,
                IsEmailVerified = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, samplePassword);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to create patient {Name}: {Errors}", pd.Name,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                continue;
            }

            await _userManager.AddToRoleAsync(user, "Patient");

            var patient = new Patient
            {
                UserId = user.Id,
                PatientCode = $"P-{codeCounter:D4}",
                PhoneNumber = pd.Phone,
                DateOfBirth = pd.DOB,
                BloodGroup = pd.Blood,
                MedicalHistory = pd.History,
                Allergies = pd.Allergies
            };

            _context.Patients.Add(patient);
            codeCounter++;
            _logger.LogInformation("Added patient: {Name} ({Code})", pd.Name, patient.PatientCode);
        }

        await _context.SaveChangesAsync();
    }
}
