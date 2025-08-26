using BookReviewsApi.Models;

public interface IBookReviewRepository
{
    Task<IEnumerable<BookReview>> GetAllAsync();
    Task<BookReview?> GetByIdAsync(int id);
    Task AddAsync(BookReview review);
    Task UpdateAsync(BookReview review);
    Task DeleteAsync(int id);
}
