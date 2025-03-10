using Microsoft.EntityFrameworkCore;

namespace BookStoreReminderAPI.Models
{
    public class OnlineBookStoreDbContext : DbContext
    {
        public OnlineBookStoreDbContext(DbContextOptions<OnlineBookStoreDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
    }
}
