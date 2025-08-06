public class NavigationDataService
{
    public BookInfo? BookInfo { get; set; }
    public BookReview? BookReview { get; set; }

    public void Clear()
    {
        BookInfo = null;
        BookReview = null;
    }
}
