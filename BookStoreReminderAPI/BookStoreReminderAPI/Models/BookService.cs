using Microsoft.EntityFrameworkCore;

namespace BookStoreReminderAPI.Models
{
    public class BookService
    {
        private readonly OnlineBookStoreDbContext _context;
        public BookService(OnlineBookStoreDbContext context) { 
            _context = context;
        }
        public async Task<List<Book>> GetBooksAsync() => await _context.Books.ToListAsync();

        public async Task BorrowBookAsync(int bookId, string borrowerEmail)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || !book.IsAvailable) throw new Exception("Book not available");

            var borrowedBook = new BorrowedBook
            {
                BookId = bookId,
                BorrowerEmail = borrowerEmail,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14)
            };

            book.IsAvailable = false;
            _context.BorrowedBooks.Add(borrowedBook);
            await _context.SaveChangesAsync();
        }
    }
}
