//using System.Text;
//using System.Text.Json;

//public class ReviewService
//{
//    private readonly HttpClient _httpClient;
//    private const string ApiBaseUrl = "https://localhost:7242/api/Books/AddWithReview";


//    public ReviewService(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//        _httpClient.Timeout = TimeSpan.FromSeconds(30);
//    }

//    // ✅ Voeg een review toe
//    public async Task<bool> AddReviewAsync(BookReview bookReview)
//    {
//        try
//        {
//            var jsonContent = new StringContent(JsonSerializer.Serialize(bookReview), Encoding.UTF8, "application/json");
//            Console.WriteLine($"Sending review data: {await jsonContent.ReadAsStringAsync()}");

//            var response = await _httpClient.PostAsync(ApiBaseUrl, jsonContent);

//            if (response.IsSuccessStatusCode)
//                return true;

//            await LogErrorResponse(response);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception while adding review: {ex.Message}");
//        }

//        return false;
//    }

//    // ✅ Haal alle reviews op
//    public async Task<List<BookReview>> GetAllReviewsAsync()
//    {
//        try
//        {
//            var response = await _httpClient.GetAsync(ApiBaseUrl);

//            if (!response.IsSuccessStatusCode)
//            {
//                await LogErrorResponse(response);
//                return new List<BookReview>();
//            }

//            var json = await response.Content.ReadAsStringAsync();
//            if (string.IsNullOrWhiteSpace(json))
//            {
//                Console.WriteLine("No reviews found in response.");
//                return new List<BookReview>();
//            }

//            var reviews = JsonSerializer.Deserialize<List<BookReview>>(json);
//            if (reviews == null)
//            {
//                Console.WriteLine("Failed to deserialize reviews.");
//                return new List<BookReview>();
//            }

//            Console.WriteLine($"Fetched {reviews.Count} reviews.");
//            return reviews;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception while fetching reviews: {ex.Message}");
//            return new List<BookReview>();
//        }
//    }

//    // ✅ Update een review
//    public async Task<BookReview?> UpdateReviewAsync(int reviewId, BookReview updatedReview)
//    {
//        try
//        {
//            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedReview), Encoding.UTF8, "application/json");
//            var response = await _httpClient.PutAsync($"{ApiBaseUrl}/{reviewId}", jsonContent);

//            if (!response.IsSuccessStatusCode)
//            {
//                await LogErrorResponse(response);
//                return null;
//            }

//            var json = await response.Content.ReadAsStringAsync();
//            var review = JsonSerializer.Deserialize<BookReview>(json);

//            if (review == null)
//            {
//                Console.WriteLine("Failed to deserialize updated review.");
//                return null;
//            }

//            Console.WriteLine($"Review updated: {review.Title}");
//            return review;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception while updating review: {ex.Message}");
//            return null;
//        }
//    }

//    // ✅ Haal één review op
//    public async Task<BookReview?> GetReviewAsync(int reviewId)
//    {
//        try
//        {
//            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{reviewId}");

//            if (!response.IsSuccessStatusCode)
//            {
//                await LogErrorResponse(response);
//                return null;
//            }

//            var json = await response.Content.ReadAsStringAsync();
//            var review = JsonSerializer.Deserialize<BookReview>(json);

//            if (review == null)
//            {
//                Console.WriteLine("Failed to deserialize review.");
//                return null;
//            }

//            Console.WriteLine($"Review fetched: {review.Title}, Author: {review.Author}");
//            return review;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception while fetching review: {ex.Message}");
//            return null;
//        }
//    }

//    // ✅ Verwijder een review
//    public async Task<bool> DeleteReviewAsync(int reviewId)
//    {
//        try
//        {
//            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{reviewId}");

//            if (response.IsSuccessStatusCode)
//            {
//                Console.WriteLine($"Review with ID {reviewId} deleted.");
//                return true;
//            }

//            await LogErrorResponse(response);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception while deleting review: {ex.Message}");
//        }

//        return false;
//    }

//    // 🔧 Log foutresponse
//    private async Task LogErrorResponse(HttpResponseMessage response)
//    {
//        var error = await response.Content.ReadAsStringAsync();
//        Console.WriteLine($"Error: {response.StatusCode}, Response: {error}");
//    }
//}
using BookRecognitionApp.Models;
using System.Text;
using System.Text.Json;

public class ReviewService
{
    private readonly HttpClient _httpClient;

    private const string AddReviewUrl = "https://localhost:7242/api/Books/AddWithReview";
    private const string ReviewApiBaseUrl = "https://localhost:7242/api/Books/Reviews";

    public ReviewService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<List<ReviewDisplayDto>> GetAllReviewsAsync()
    {
        var response = await _httpClient.GetAsync("https://yourapi.com/api/reviews");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ReviewDisplayDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }


    // ✅ Add book + review
    public async Task<bool> AddReviewAsync(BookReviewDTO bookReviewDto)
    {
        try
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(bookReviewDto),
                Encoding.UTF8,
                "application/json");

            Console.WriteLine($"📤 Sending review data: {await jsonContent.ReadAsStringAsync()}");

            var response = await _httpClient.PostAsync(AddReviewUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Review submitted successfully.");
                return true;
            }

            await LogErrorResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception while adding review: {ex.Message}");
        }

        return false;
    }



    // ✅ Get one review by ID
    public async Task<BookReview?> GetReviewAsync(int reviewId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ReviewApiBaseUrl}/{reviewId}");

            if (!response.IsSuccessStatusCode)
            {
                await LogErrorResponse(response);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BookReview>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception while fetching review: {ex.Message}");
            return null;
        }
    }

    // ✅ Update review
    public async Task<BookReview?> UpdateReviewAsync(int reviewId, BookReview updatedReview)
    {
        try
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(updatedReview),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"{ReviewApiBaseUrl}/{reviewId}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                await LogErrorResponse(response);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BookReview>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception while updating review: {ex.Message}");
            return null;
        }
    }

    // ✅ Delete review
    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ReviewApiBaseUrl}/{reviewId}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"✅ Review with ID {reviewId} deleted.");
                return true;
            }

            await LogErrorResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception while deleting review: {ex.Message}");
        }

        return false;
    }

    // 🔧 Log errors
    private async Task LogErrorResponse(HttpResponseMessage response)
    {
        var error = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"❌ Error: {response.StatusCode}, Response: {error}");
    }
}

