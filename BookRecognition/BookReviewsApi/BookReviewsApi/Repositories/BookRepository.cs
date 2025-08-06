using BookReviewsApi.Data;
using BookReviewsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewsApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookReviewDbContext _context;

        public BookRepository(BookReviewDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync() =>
            await _context.Books.Include(b => b.Reviews).ToListAsync();

        public async Task<Book> GetByIdAsync(int id) =>
            await _context.Books.Include(b => b.Reviews).FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Book> AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var existing = await _context.Books.FindAsync(book.Id);
            if (existing == null) return null;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Year = book.Year;
            existing.ISBN = book.ISBN;
            existing.CoverUrl = book.CoverUrl;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
