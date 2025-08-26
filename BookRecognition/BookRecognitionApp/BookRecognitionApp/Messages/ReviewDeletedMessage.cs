using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookRecognitionApp.Messages
{
    public class ReviewDeletedMessage : ValueChangedMessage<int>
    {
        public ReviewDeletedMessage(int reviewId) : base(reviewId) { }
    }
}
