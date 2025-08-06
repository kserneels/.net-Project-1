using BookRecognitionApp.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookRecognitionApp.Messages
{
    public class BookSelectedMessage : ValueChangedMessage<ReviewDisplayDto>
    {
        public BookSelectedMessage(ReviewDisplayDto value) : base(value) { }
    }
}
