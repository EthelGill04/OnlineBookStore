using BookStoreReminderAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreReminderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
            private readonly BookService _bookService;

            public BooksController(BookService bookService)
            {
                _bookService = bookService;
            }

            [HttpGet]
            public async Task<IActionResult> GetBooks()
            {
                var books = await _bookService.GetBooksAsync();
                return Ok(books);
            }

            [HttpPost("borrow")]
            public async Task<IActionResult> BorrowBook(int bookId, string email)
            {
                try
                {
                    await _bookService.BorrowBookAsync(bookId, email);
                    return Ok("Book borrowed successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        

    }
}
