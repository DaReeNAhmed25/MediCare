using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class VacationDayConfiguration : IEntityTypeConfiguration<VacationDay>
{
    public void Configure(EntityTypeBuilder<VacationDay> builder)
    {
        builder.ToTable("VacationDays");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Reason)
            .HasMaxLength(500);

        // Unique: a doctor cannot have two vacation entries for the same date
        builder.HasIndex(v => new { v.DoctorId, v.VacationDate })
            .IsUnique();

        builder.HasOne(v => v.Doctor)
            .WithMany(d => d.VacationDays)
            .HasForeignKey(v => v.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
