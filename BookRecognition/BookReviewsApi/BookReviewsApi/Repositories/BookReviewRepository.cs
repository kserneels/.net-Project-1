using BookReviewsApi.Data;
using BookReviewsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewsApi.Repositories
{
    public class BookReviewRepository : IBookReviewRepository
    {
        private readonly BookReviewDbContext _context;

        public BookReviewRepository(BookReviewDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookReview>> GetAllAsync() =>
            await _context.BookReviews.Include(r => r.Book).ToListAsync();

        public async Task<BookReview?> GetByIdAsync(int id) =>
            await _context.BookReviews.Include(r => r.Book).FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<BookReview>> GetByBookIdAsync(int bookId) =>
            await _context.BookReviews.Where(r => r.BookId == bookId).ToListAsync();

        public async Task<BookReview> AddAsync(BookReview review)
        {
            review.ReviewDate = DateTime.Now;
            _context.BookReviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<BookReview?> UpdateAsync(BookReview review)
        {
            var existing = await _context.BookReviews.FindAsync(review.Id);
            if (existing == null) return null;

            existing.Rating = review.Rating;
            existing.Review = review.Review;
            existing.ReviewDate = review.ReviewDate;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.BookReviews.FindAsync(id);
            if (review == null) return false;

            _context.BookReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
