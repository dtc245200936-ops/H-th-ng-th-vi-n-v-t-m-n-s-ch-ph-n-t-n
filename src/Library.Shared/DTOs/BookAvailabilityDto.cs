namespace Library.Shared.DTOs;

public record BookDetailDto(
    Guid Id,
    string Title,
    string Author,
    string ISBN,
    int TotalCopies,
    int AvailableCopies,
    int ReservedOrBorrowedCopies,
    bool IsAvailable,
    string AvailabilityStatus
);

public record BookAvailabilityResponse(
    Guid BookId,
    string Title,
    int TotalCopies,
    int AvailableCopies,
    int ReservedOrBorrowedCopies,
    bool IsAvailable,
    string StatusMessage
);