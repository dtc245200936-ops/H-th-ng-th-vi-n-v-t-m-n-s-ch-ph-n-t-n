using Library.Shared.Enums;

namespace Library.Shared.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User? User { get; set; }
    public Book? Book { get; set; }
}