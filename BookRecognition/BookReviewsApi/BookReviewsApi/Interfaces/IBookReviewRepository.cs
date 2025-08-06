using BookReviewsApi.Models;

namespace BookReviewsApi.Repositories
{
    public interface IBookReviewRepository
    {
        Task<List<BookReview>> GetAllAsync();
        Task<BookReview?> GetByIdAsync(int id);
        Task<List<BookReview>> GetByBookIdAsync(int bookId);
        Task<BookReview> AddAsync(BookReview review);
        Task<BookReview?> UpdateAsync(BookReview review);
        Task<bool> DeleteAsync(int id);
    }
}
