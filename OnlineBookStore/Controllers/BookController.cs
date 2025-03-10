using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

public class BookController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BookController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("BookStoreAPI");
        var response = await client.GetAsync("books"); // Calls GET /api/books

        if (!response.IsSuccessStatusCode)
        {
            return View("Error");
        }

        var books = await response.Content.ReadAsAsync<List<Book>>(); // Ensure you have a Book model
        return View(books);
    }
}
