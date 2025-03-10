using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


public class Index1Model : PageModel
{
    private readonly BookService _bookService;

    public List<Book> Books { get; set; }

    public Index1Model(BookService bookService)
    {
        _bookService = bookService;
    }

    public async Task OnGetAsync()
    {
        Books = await _bookService.GetBooksAsync();
    }

    public async Task<IActionResult> OnPostBorrowAsync(int bookId, string email)
    {
        var result = await _bookService.BorrowBookAsync(bookId, email);
        TempData["Message"] = result;
        return RedirectToPage();
    }
}