using BookReviewsApi.Models;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(int id);
    Task<Book> AddAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
}