using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.ToTable("OtpCodes");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.CodeHash)
            .IsRequired()
            .HasMaxLength(256);

        // Check constraint: FailedAttempts 0–5
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_OtpCodes_FailedAttempts",
            "[FailedAttempts] >= 0 AND [FailedAttempts] <= 5"));

        builder.Property(o => o.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Index for quick lookup of valid OTPs by user and expiry
        builder.HasIndex(o => new { o.UserId, o.ExpiresAt });

        builder.HasOne(o => o.User)
            .WithMany(u => u.OtpCodes)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
