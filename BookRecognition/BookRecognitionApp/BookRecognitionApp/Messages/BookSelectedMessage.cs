using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookRecognitionApp.Messages
{
    public class BookSelectedMessage : ValueChangedMessage<(BookInfo BookInfo, BookReview BookReview)>
    {
        public BookSelectedMessage(BookInfo bookInfo, BookReview bookReview)
            : base((bookInfo, bookReview))
        {
        }
    }
}
