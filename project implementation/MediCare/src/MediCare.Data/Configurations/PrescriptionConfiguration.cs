using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("Prescriptions");

        builder.HasKey(p => p.Id);

        // One prescription per medical record
        builder.HasIndex(p => p.MedicalRecordId)
            .IsUnique();

        builder.Property(p => p.IssuedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(p => p.MedicalRecord)
            .WithOne(m => m.Prescription)
            .HasForeignKey<Prescription>(p => p.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
