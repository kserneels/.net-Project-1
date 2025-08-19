// Messages/ReviewDeletedMessage.cs
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookRecognitionApp.Messages
{
    public class ReviewDeletedMessage : ValueChangedMessage<int>
    {
        // The Value here is the ID of the deleted review
        public ReviewDeletedMessage(int reviewId) : base(reviewId) { }
    }
}
