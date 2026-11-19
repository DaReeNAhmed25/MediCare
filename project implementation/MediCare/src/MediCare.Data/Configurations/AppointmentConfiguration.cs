using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        // Status stored as a string (max 20 chars), default "Pending"
        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(AppointmentStatus.Pending);

        builder.Property(a => a.Notes)
            .HasMaxLength(1000);

        // Optimistic concurrency token for double-booking prevention
        builder.Property(a => a.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // ─── Indexes ───────────────────────────────────────────────────────────

        // Non-unique index on Status for fast status filtering
        builder.HasIndex(a => a.Status);

        // Non-unique index on (PatientId, AppointmentDate) for patient history queries
        builder.HasIndex(a => new { a.PatientId, a.AppointmentDate });

        // FILTERED UNIQUE INDEX: prevents double-booking.
        // Two active (non-Cancelled, non-Rejected) appointments for the same
        // doctor/date/start-time are impossible at the DB level.
        builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.StartTime })
            .IsUnique()
            .HasFilter("[Status] IN ('Pending','Confirmed','Completed')")
            .HasDatabaseName("IX_Appointments_DoctorId_AppointmentDate_StartTime_Active");

        // ─── Check constraints ─────────────────────────────────────────────────

        // Valid status values
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Appointments_Status",
            "[Status] IN ('Pending','Confirmed','Rejected','Cancelled','Completed')"));

        // ─── Relationships (all Restrict to avoid cascades on audit trail) ─────

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
