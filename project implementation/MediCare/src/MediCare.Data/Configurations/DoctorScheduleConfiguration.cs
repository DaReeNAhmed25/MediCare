using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedules");

        builder.HasKey(s => s.Id);

        // DayOfWeek must be 0–6
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_DoctorSchedules_DayOfWeek",
            "[DayOfWeek] >= 0 AND [DayOfWeek] <= 6"));

        // EndTime must be after StartTime
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_DoctorSchedules_Times",
            "[EndTime] > [StartTime]"));

        builder.Property(s => s.SlotMinutes)
            .IsRequired();

        builder.HasOne(s => s.Doctor)
            .WithMany(d => d.Schedules)
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
