using Library.Shared.Enums;

namespace Library.Shared.DTOs;

public record CreateReservationRequest(Guid UserId, Guid BookId);

public record ReservationResponse(
    Guid Id,
    Guid UserId,
    Guid BookId,
    string BookTitle,
    ReservationStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);