using System.Text;
using System.Text.Json;

public class ReviewService
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "https://localhost:7242/api/BookReviews";

    public ReviewService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    // Method to add a review
    public async Task<bool> AddReviewAsync(BookReview bookReview)
    {
        try
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(bookReview), Encoding.UTF8, "application/json");

            string jsonToSend = await jsonContent.ReadAsStringAsync();
            Console.WriteLine($"Sending review data: {jsonToSend}");

            var response = await _httpClient.PostAsync(ApiBaseUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await LogErrorResponse(response);
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while submitting review: {ex.Message}");
            return false;
        }
    }

    public async Task<List<BookReview>> GetAllReviewsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiBaseUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var reviews = JsonSerializer.Deserialize<List<BookReview>>(json);
                    if (reviews != null)
                    {
                        Console.WriteLine("Reviews fetched successfully.");
                        foreach (var review in reviews)
                        {
                            Console.WriteLine($"Review: {review.Title}, {review.Author}, {review.Rating}");
                        }
                        return reviews;
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize reviews.");
                    }
                }
                else
                {
                    Console.WriteLine("No reviews found in response.");
                }
            }
            else
            {
                Console.WriteLine($"Error fetching reviews: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        return new List<BookReview>();
    }

    public async Task<BookReview> UpdateReviewAsync(int reviewId, BookReview updatedReview)
    {
        try
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedReview), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{ApiBaseUrl}/{reviewId}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {json}");

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var review = JsonSerializer.Deserialize<BookReview>(json);
                    if (review != null)
                    {
                        Console.WriteLine($"Review updated: {review.Title}");
                        return review;
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize updated review.");
                    }
                }
                else
                {
                    Console.WriteLine("Empty response received.");
                }
            }
            else
            {
                await LogErrorResponse(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while updating review: {ex.Message}");
        }

        return null; // Als er iets misgaat, return null
    }


    public async Task<BookReview> GetReviewAsync(int reviewId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{reviewId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var review = JsonSerializer.Deserialize<BookReview>(json);
                    if (review != null)
                    {
                        Console.WriteLine($"Review fetched: {review.Title}, Author: {review.Author}, ReviewDate: {review.ReviewDate}");
                        return review;
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize the review.");
                    }
                }
                else
                {
                    Console.WriteLine("Empty response received.");
                }
            }
            else
            {
                await LogErrorResponse(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while fetching review: {ex.Message}");
        }

        return null; // Return null if something went wrong
    }


    // Method to delete a review
    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{reviewId}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Review with ID {reviewId} deleted successfully.");
                return true;
            }
            else
            {
                await LogErrorResponse(response);
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while deleting review: {ex.Message}");
            return false;
        }
    }

    // Helper method to log error responses
    private async Task LogErrorResponse(HttpResponseMessage response)
    {
        var errorResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error: {response.StatusCode}, Response: {errorResponse}");
    }
}
