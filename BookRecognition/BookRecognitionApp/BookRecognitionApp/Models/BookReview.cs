using System.Text.Json.Serialization;

public class BookReview
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("year")]
    public string Year { get; set; }

    [JsonPropertyName("isbn")]
    public string ISBN { get; set; }

    [JsonPropertyName("coverUrl")]
    public string CoverUrl { get; set; }

    [JsonPropertyName("rating")]
    public string Rating { get; set; }

    [JsonPropertyName("review")]
    public string Review { get; set; }

    [JsonPropertyName("reviewDate")]
    public DateTime ReviewDate { get; set; }

    public override bool Equals(object obj)
    {
        return ISBN == (obj as BookReview)?.ISBN;
    }
}
