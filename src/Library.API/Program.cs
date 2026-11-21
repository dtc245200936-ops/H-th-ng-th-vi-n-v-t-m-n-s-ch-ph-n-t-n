using Library.Shared.Data;
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

app.MapGet("/books", async (LibraryDbContext db, ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching all books from database");
    var books = await db.Books.AsNoTracking().ToListAsync();
    return Results.Ok(books);
});

app.MapGet("/books/{id:guid}", async (Guid id, LibraryDbContext db, ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching book details for BookId: {BookId}", id);
    var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    return book is not null ? Results.Ok(book) : Results.NotFound(new { message = "Khong tim thay sach" });
});

app.Run();