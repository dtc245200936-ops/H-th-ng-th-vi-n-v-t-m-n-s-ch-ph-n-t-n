using Library.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Shared.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), FullName = "Nguyen Van An", Email = "an.nv@student.edu.vn" },
            new User { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), FullName = "Tran Thi Binh", Email = "binh.tt@student.edu.vn" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = Guid.Parse("aaaa1111-1111-1111-1111-111111111111"),
                Title = "Phat trien ung dung phan tan voi .NET",
                Author = "Minh Duc",
                ISBN = "978-604-001",
                TotalCopies = 5,
                AvailableCopies = 5
            },
            new Book
            {
                Id = Guid.Parse("bbbb2222-2222-2222-2222-222222222222"),
                Title = "Kien truc Microservices va RabbitMQ",
                Author = "Martin Fowler",
                ISBN = "978-604-002",
                TotalCopies = 3,
                AvailableCopies = 3
            },
            new Book
            {
                Id = Guid.Parse("cccc3333-3333-3333-3333-333333333333"),
                Title = "Co so du lieu phan tan nang cao",
                Author = "Elmasri & Navathe",
                ISBN = "978-604-003",
                TotalCopies = 2,
                AvailableCopies = 0
            }
        );
    }
}