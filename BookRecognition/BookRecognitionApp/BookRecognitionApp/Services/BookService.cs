using System.Net.Http.Json;
using System.Text.Json;

public class BookService
{
    private readonly HttpClient _httpClient;

    public BookService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BookInfo> GetBookDetailsFromOpenLibrary(string isbn)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var bookData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                if (bookData.TryGetProperty($"ISBN:{isbn}", out JsonElement bookInfo))
                {
                    string title = bookInfo.GetProperty("title").GetString();

                    string author = bookInfo.GetProperty("authors")[0].GetProperty("name").GetString();

                    string year = bookInfo.GetProperty("publish_date").GetString();
                    if (year.Contains("-"))
                    {
                        year = year.Split('-')[0];
                    }

                    string coverImage = null;
                    if (bookInfo.TryGetProperty("cover", out JsonElement cover))
                    {
                        if (cover.TryGetProperty("large", out JsonElement largeCover))
                        {
                            coverImage = largeCover.GetString();
                        }
                    }

                    if (string.IsNullOrEmpty(coverImage))
                    {
                        Console.WriteLine("No cover image found, using default.");
                        coverImage = "Resources/Images/no_cover.jpg";
                    }

                    return new BookInfo
                    {
                        Title = title,
                        Author = author,
                        Year = year,
                        CoverUrl = coverImage,
                        ISBN = isbn
                    };
                }
            }

            return null;
        }
    }

    public async Task<bool> AddReviewAsync(BookReview review)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reviews", review);
        return response.IsSuccessStatusCode;
    }
}


//using System.Net.Http.Json;
//using System.Text.Json;

//public class BookService
//{
//    private readonly HttpClient _httpClient;

//    public BookService(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//    }

//    // Fetch book info from OpenLibrary API
//    public async Task<BookInfo> GetBookDetailsFromOpenLibrary(string isbn)
//    {
//        using (HttpClient client = new HttpClient())
//        {
//            string url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
//            HttpResponseMessage response = await client.GetAsync(url);

//            if (!response.IsSuccessStatusCode)
//                return null;

//            string jsonResponse = await response.Content.ReadAsStringAsync();
//            var bookData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

//            if (!bookData.TryGetProperty($"ISBN:{isbn}", out JsonElement bookInfo))
//                return null;

//            string title = bookInfo.GetProperty("title").GetString();
//            string author = bookInfo.GetProperty("authors")[0].GetProperty("name").GetString();
//            string year = bookInfo.GetProperty("publish_date").GetString();
//            if (year.Contains("-"))
//                year = year.Split('-')[0];

//            string coverUrl = "Resources/Images/no_cover.jpg";
//            if (bookInfo.TryGetProperty("cover", out JsonElement cover))
//            {
//                if (cover.TryGetProperty("large", out JsonElement largeCover))
//                    coverUrl = largeCover.GetString();
//            }

//            return new Bookinfo
//            {
//                Title = title,
//                Author = author,
//                Year = year,
//                CoverUrl = coverUrl,
//                ISBN = isbn
//            };
//        }
//    }

//    // Add a new book to backend
//    public async Task<Book> AddBookAsync(Book book)
//    {
//        var response = await _httpClient.PostAsJsonAsync("api/books", book);
//        response.EnsureSuccessStatusCode();
//        return await response.Content.ReadFromJsonAsync<Book>();
//    }

//    // Add a new review to backend
//    public async Task<bool> AddReviewAsync(BookReview review)
//    {
//        var response = await _httpClient.PostAsJsonAsync("api/bookreviews", review);
//        return response.IsSuccessStatusCode;
//    }

//    // Optionally, fetch all reviews for a book
//    public async Task<List<BookReview>> GetReviewsByBookIdAsync(int bookId)
//    {
//        var response = await _httpClient.GetAsync($"api/bookreviews?bookId={bookId}");
//        if (!response.IsSuccessStatusCode)
//            return new List<BookReview>();

//        return await response.Content.ReadFromJsonAsync<List<BookReview>>();
//    }
//}
