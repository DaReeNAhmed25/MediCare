using MediCare.Data.Entities;
using MediCare.Data.Repositories;
using MediCare.Data.Repositories.Interfaces;
using MediCare.Data.Seed;
using MediCare.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediCare.Data.Extensions;

/// <summary>
/// DI registration extension for the Data layer.
/// Call this from MediCare.Web's Program.cs.
/// NOTE: Identity registration (AddIdentity) must be done in MediCare.Web's Program.cs
/// because it depends on ASP.NET Core web framework types. This method registers the
/// DbContext, repositories, Unit of Work, and seeder only.
/// </summary>
public static class DataLayerExtensions
{
    public static IServiceCollection AddDataLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ─── DbContext ────────────────────────────────────────────────────────
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly("MediCare.Data")));

        // ─── Repositories ────────────────────────────────────────────────────
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();

        // ─── Unit of Work ────────────────────────────────────────────────────
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // ─── Seeder ──────────────────────────────────────────────────────────
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
