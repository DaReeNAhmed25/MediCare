using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.ToTable("MedicalRecords");

        builder.HasKey(m => m.Id);

        // One record per appointment
        builder.HasIndex(m => m.AppointmentId)
            .IsUnique();

        builder.Property(m => m.BloodPressure)
            .HasMaxLength(20);

        // Check constraint: HeartRate 20–300
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_MedicalRecords_HeartRate",
            "[HeartRate] IS NULL OR ([HeartRate] >= 20 AND [HeartRate] <= 300)"));

        builder.Property(m => m.Symptoms)
            .HasMaxLength(2000);

        builder.Property(m => m.Diagnosis)
            .HasMaxLength(2000);

        builder.Property(m => m.TreatmentNotes)
            .HasMaxLength(4000);

        builder.Property(m => m.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(m => m.Appointment)
            .WithOne(a => a.MedicalRecord)
            .HasForeignKey<MedicalRecord>(m => m.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
