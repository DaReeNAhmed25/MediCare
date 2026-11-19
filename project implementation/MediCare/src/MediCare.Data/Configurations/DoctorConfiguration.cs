using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.UserId)
            .IsRequired();

        builder.HasIndex(d => d.UserId)
            .IsUnique();

        builder.Property(d => d.Location)
            .HasMaxLength(200);

        builder.Property(d => d.ConsultationFee)
            .HasColumnType("decimal(10,2)");

        // Check constraint: fee >= 0
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Doctors_ConsultationFee",
            "[ConsultationFee] >= 0"));

        builder.Property(d => d.RoomNumber)
            .HasMaxLength(20);

        builder.Property(d => d.ProfileImage)
            .HasMaxLength(500);

        builder.Property(d => d.IsDeleted)
            .HasDefaultValue(false);

        // Index on SpecializationId for search performance
        builder.HasIndex(d => d.SpecializationId);

        // Relationships
        builder.HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Specialization)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Global query filter: exclude soft-deleted doctors
        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}
