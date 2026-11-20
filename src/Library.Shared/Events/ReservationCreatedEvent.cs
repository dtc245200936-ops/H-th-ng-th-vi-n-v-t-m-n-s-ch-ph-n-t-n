namespace Library.Shared.Events;

public record ReservationCreatedEvent(
    Guid ReservationId,
    Guid BookId,
    Guid UserId,
    DateTime CreatedAt,
    string CorrelationId
);