using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookRecognitionApp.Messages
{
    public class ReviewAddedOrUpdatedMessage : ValueChangedMessage<BookReview>
    {
        public ReviewAddedOrUpdatedMessage(BookReview review) : base(review) { }
    }
}
