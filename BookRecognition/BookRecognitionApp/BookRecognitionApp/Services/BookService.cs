using System.Net.Http.Json;
using System.Text.Json;

public class BookService
{
    private readonly HttpClient _httpClient;

    public BookService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Fetch book information from OpenLibrary API
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
                    // Haal titel op
                    string title = bookInfo.GetProperty("title").GetString();

                    // Haal auteur op
                    string author = bookInfo.GetProperty("authors")[0].GetProperty("name").GetString();

                    // Haal het jaar op. Als het een volledige datum is, pak alleen het jaar.
                    string year = bookInfo.GetProperty("publish_date").GetString();
                    if (year.Contains("-"))
                    {
                        year = year.Split('-')[0]; // Alleen het jaar nemen
                    }

                    // Haal de cover-URL op, gebruik een standaard afbeelding als deze niet beschikbaar is
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
                        // Log de fout of geef de waarde weer
                        Console.WriteLine("No cover image found, using default.");
                        coverImage = "Resources/Images/no_cover.jpg";
                    }

                    // Geef de boekinformatie terug
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


    // Submit a book review to the database
    public async Task<bool> AddReviewAsync(BookReview review)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Books/AddWithReview", review);

        return response.IsSuccessStatusCode;
    }
}
