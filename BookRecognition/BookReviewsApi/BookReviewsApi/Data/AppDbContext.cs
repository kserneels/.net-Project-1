using BookReviewsApi.Models;
using Microsoft.EntityFrameworkCore;  // Ensure you're using the BookReview model

namespace BookReviewsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // enkel deze DbSet is nodig
        public DbSet<BookReview> BookReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
