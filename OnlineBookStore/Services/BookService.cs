using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BookService
{
    private readonly HttpClient _httpClient;

    public BookService(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("OnlineBookStoreAPI");
    }

    public async Task<List<Book>> GetBooksAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Book>>("api/books");
        return await response.Content.ReadAsListAsync();
    }

    public async Task<string> BorrowBookAsync(int bookId, string email)
    {
        var response = await _httpClient.PostAsync($"api/books/borrow?bookId={bookId}&email={email}", null);
        return await response.Content.ReadAsStringAsync();
    }
}
