using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewsApi.Models
{
    public class BookReview
    {
        public int Id { get; set; }

        [Required]
        public string Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Review { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        // ✅ Foreign key naar Book
        [ForeignKey("Book")]
        public int BookId { get; set; }

        // ✅ Navigatie-eigenschap
        public Book Book { get; set; }
    }
}
