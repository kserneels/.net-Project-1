public class ReviewDisplayDto
{
    public int Id { get; set; }
    public string Rating { get; set; }
    public string Review { get; set; }
    public DateTime ReviewDate { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    public string Author { get; set; }
    public string Year { get; set; }
    public string ISBN { get; set; }
}
