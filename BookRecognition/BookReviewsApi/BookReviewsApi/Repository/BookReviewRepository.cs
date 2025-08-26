using BookReviewsApi.Data;
using BookReviewsApi.Models;
using Microsoft.EntityFrameworkCore;

public class BookReviewRepository : IBookReviewRepository
{
    private readonly ApplicationDbContext _context;

    public BookReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookReview>> GetAllAsync() =>
        await _context.BookReviews.ToListAsync();

    public async Task<BookReview?> GetByIdAsync(int id) =>
        await _context.BookReviews.FindAsync(id);

    public async Task AddAsync(BookReview review)
    {
        review.ReviewDate = DateTime.Now;
        _context.BookReviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BookReview review)
    {
        _context.BookReviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var review = await _context.BookReviews.FindAsync(id);
        if (review != null)
        {
            _context.BookReviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
