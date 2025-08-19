using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookRecognitionApp.Messages
{
    public class SelectedReviewMessage : ValueChangedMessage<BookReview>
    {
        public SelectedReviewMessage(BookReview value) : base(value) { }
    }
}
