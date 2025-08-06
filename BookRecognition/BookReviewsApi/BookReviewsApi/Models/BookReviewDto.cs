using System.ComponentModel.DataAnnotations;

public class BookReviewDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Author { get; set; }

    [Required]
    public string Year { get; set; }

    [Required]
    public string ISBN { get; set; }

    public string CoverUrl { get; set; }

    [Required]
    public string Rating { get; set; }

    public string Review { get; set; }

    [Required]
    public DateTime ReviewDate { get; set; }
}
