using BookReviewsApi.Models;
using BookReviewsApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IBookReviewRepository _reviewRepository;

        public BooksController(IBookRepository repository, IBookReviewRepository reviewRepository)
        {
            _repository = repository;
            _reviewRepository = reviewRepository;
        }

        // ✅ Upload book + review together
        [HttpPost("AddWithReview")]
        public async Task<IActionResult> AddBookWithReview([FromBody] BookReviewDto dto)
        {
            var existingBook = await _repository.GetAllAsync();
            var book = existingBook.FirstOrDefault(b => b.ISBN == dto.ISBN);

            if (book == null)
            {
                book = new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    Year = dto.Year,
                    ISBN = dto.ISBN,
                    CoverUrl = dto.CoverUrl
                };
                book = await _repository.AddAsync(book);
            }

            var review = new BookReview
            {
                Rating = dto.Rating,
                Review = dto.Review,
                ReviewDate = dto.ReviewDate,
                BookId = book.Id
            };

            await _reviewRepository.AddAsync(review);
            return Ok();
        }

        // ✅ Get all reviews
        [HttpGet("Reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDisplayDto>>> GetAllReviews()
        {
            var reviews = await _reviewRepository.GetAllAsync();

            var dtoList = reviews.Select(r => new ReviewDisplayDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Review = r.Review,
                ReviewDate = r.ReviewDate,
                Title = r.Book.Title,
                CoverUrl = r.Book.CoverUrl,
                Author = r.Book.Author,
                Year = r.Book.Year,
                ISBN = r.Book.ISBN
            }).ToList();

            return Ok(dtoList);
        }


        // ✅ Get review by ID
        [HttpGet("Reviews/{id}")]
        public async Task<ActionResult<BookReview>> GetReview(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        // ✅ Update review
        [HttpPut("Reviews/{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] BookReview updatedReview)
        {
            if (id != updatedReview.Id || !ModelState.IsValid) return BadRequest();
            var result = await _reviewRepository.UpdateAsync(updatedReview);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // ✅ Delete review
        [HttpDelete("Reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var success = await _reviewRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
