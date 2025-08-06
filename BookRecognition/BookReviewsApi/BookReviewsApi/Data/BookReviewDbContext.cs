using BookReviewsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewsApi.Data
{
    public class BookReviewDbContext : DbContext
    {
        public BookReviewDbContext(DbContextOptions<BookReviewDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
