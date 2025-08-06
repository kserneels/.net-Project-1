using System.ComponentModel.DataAnnotations;

namespace BookReviewsApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$")]
        public string Year { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}(\d{3})?$")]
        public string ISBN { get; set; }

        [Url]
        public string CoverUrl { get; set; }

        // ✅ Navigatie-eigenschap: één boek heeft meerdere reviews
        public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
    }
}
