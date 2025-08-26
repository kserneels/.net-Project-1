using BookReviewsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookReviewsController : ControllerBase
    {
        private readonly IBookReviewRepository _repository;

        public BookReviewsController(IBookReviewRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _repository.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] BookReview review)
        {
            if (review == null || string.IsNullOrWhiteSpace(review.Review))
                return BadRequest("Invalid review data.");

            await _repository.AddAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] BookReview review)
        {
            if (id != review.Id) return BadRequest("Id mismatch");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Title = review.Title;
            existing.Author = review.Author;
            existing.ISBN = review.ISBN;
            existing.CoverUrl = review.CoverUrl;
            existing.Rating = review.Rating;
            existing.Review = review.Review;
            existing.ReviewDate = DateTime.Now;

            await _repository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
