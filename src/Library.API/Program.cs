using Library.Shared.Data;
using Library.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", service = "Library.API" }));

// Chức năng 1 & 2: Tra cứu danh sách sách (hỗ trợ lọc ?availableOnly=true) kèm trạng thái khả dụng
app.MapGet("/books", async (bool? availableOnly, LibraryDbContext db, ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching books (availableOnly: {AvailableOnly})", availableOnly);

    var query = db.Books.AsNoTracking();
    if (availableOnly == true)
    {
        query = query.Where(b => b.AvailableCopies > 0);
    }

    var books = await query
        .Select(b => new BookDetailDto(
            b.Id,
            b.Title,
            b.Author,
            b.ISBN,
            b.TotalCopies,
            b.AvailableCopies,
            b.TotalCopies - b.AvailableCopies,
            b.AvailableCopies > 0,
            b.AvailableCopies > 0 ? $"Con {b.AvailableCopies} ban kha dung" : "Het sach"
        ))
        .ToListAsync();

    return Results.Ok(books);
});

// Chức năng 1: Tra cứu chi tiết sách kèm thông tin số bản khả dụng
app.MapGet("/books/{id:guid}", async (Guid id, LibraryDbContext db, ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching book details for BookId: {BookId}", id);
    var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    if (book is null)
        return Results.NotFound(new { message = "Khong tim thay sach" });

    var result = new BookDetailDto(
        book.Id,
        book.Title,
        book.Author,
        book.ISBN,
        book.TotalCopies,
        book.AvailableCopies,
        book.TotalCopies - book.AvailableCopies,
        book.AvailableCopies > 0,
        book.AvailableCopies > 0 ? $"Con {book.AvailableCopies} ban kha dung" : "Het sach"
    );

    return Results.Ok(result);
});

// Chức năng 2: Tra cứu nhanh số bản đang khả dụng của một đầu sách
app.MapGet("/books/{id:guid}/availability", async (Guid id, LibraryDbContext db, ILogger<Program> logger) =>
{
    logger.LogInformation("Checking available copies for BookId: {BookId}", id);
    var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    if (book is null)
        return Results.NotFound(new { message = "Khong tim thay sach" });

    var response = new BookAvailabilityResponse(
        book.Id,
        book.Title,
        book.TotalCopies,
        book.AvailableCopies,
        book.TotalCopies - book.AvailableCopies,
        book.AvailableCopies > 0,
        book.AvailableCopies > 0
            ? $"San sang dat muon ({book.AvailableCopies}/{book.TotalCopies} ban kha dung)"
            : "Tam het sach (0 ban kha dung)"
    );

    return Results.Ok(response);
});

app.Run();