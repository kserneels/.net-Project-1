using Microsoft.AspNetCore.Mvc;
using BookReviewsApi.Data;
using BookReviewsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/BookReviews
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] BookReview bookReview)
        {
            if (bookReview == null || string.IsNullOrWhiteSpace(bookReview.Review))
            {
                return BadRequest("Invalid review data.");
            }

            // Set de review datum naar de current datum
            bookReview.ReviewDate = DateTime.Now;

            // Voeg de review toe aan de db
            _context.BookReviews.Add(bookReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = bookReview.Id }, bookReview);
        }

        // GET: api/BookReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReview>> GetReview(int id)
        {
            var review = await _context.BookReviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // GET: api/BookReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReview>>> GetAllReviews()
        {
            var reviews = await _context.BookReviews.ToListAsync();
            return Ok(reviews);
        }

        // PUT: api/BookReviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] BookReview updatedReview)
        {
            if (id != updatedReview.Id || updatedReview == null || string.IsNullOrWhiteSpace(updatedReview.Review))
            {
                return BadRequest("Invalid review data.");
            }

            var existingReview = await _context.BookReviews.FindAsync(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            // Update de review fields
            existingReview.Title = updatedReview.Title;
            existingReview.Author = updatedReview.Author;
            existingReview.Review = updatedReview.Review;
            existingReview.Rating = updatedReview.Rating;
            existingReview.ReviewDate = updatedReview.ReviewDate;

            // Mark als modified
            _context.Entry(existingReview).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(existingReview);  // code 200
        }

        // DELETE: api/BookReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.BookReviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.BookReviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
