namespace MediCare.Data.Entities;

/// <summary>
/// Appointment status values stored as a string (max 20 chars).
/// Default: Pending.
/// </summary>
public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Rejected,
    Cancelled,
    Completed
}
