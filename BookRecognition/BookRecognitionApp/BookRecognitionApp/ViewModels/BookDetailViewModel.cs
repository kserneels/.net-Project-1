using BookRecognitionApp.Messages;
using BookRecognitionApp.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BookRecognitionApp.ViewModels
{
    public partial class BookDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ReviewService _reviewService;
        private readonly INavigationService _navigationService;

        public BookDetailViewModel(INavigationService navigationService, ReviewService reviewService)
        {
            _navigationService = navigationService;
            _reviewService = reviewService;
        }

        [ObservableProperty] private BookReview? bookReview;

        [ObservableProperty] private bool isEditing;
        [ObservableProperty] private string editableRating = "Gemiddeld";
        [ObservableProperty] private string editableReview = "";

        public string[] Ratings { get; } = { "Uitstekend", "Goed", "Gemiddeld", "Slecht" };

        public bool IsNotEditing => !IsEditing;
        public string EditButtonText => IsEditing ? "Opslaan" : "Bewerken";

        public IAsyncRelayCommand BackCommand => new AsyncRelayCommand(GoBackAsync);
        public IAsyncRelayCommand DeleteCommand => new AsyncRelayCommand(DeleteReviewAsync);
        public IAsyncRelayCommand EditCommand => new AsyncRelayCommand(EditOrSaveAsync);
        public RelayCommand CancelCommand => new RelayCommand(CancelEditing);

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedReview", out var reviewObj) && reviewObj is BookReview review)
            {
                BookReview = review;
                EditableRating = review.Rating ?? "Gemiddeld";
                EditableReview = review.Review ?? "";
            }
        }

        private void StartEditing()
        {
            IsEditing = true;
            EditableRating = BookReview?.Rating ?? "Gemiddeld";
            EditableReview = BookReview?.Review ?? "";
            OnPropertyChanged(nameof(IsNotEditing));
            OnPropertyChanged(nameof(EditButtonText));
        }

        private void CancelEditing()
        {
            IsEditing = false;
            OnPropertyChanged(nameof(IsNotEditing));
            OnPropertyChanged(nameof(EditButtonText));
        }

        private async Task EditOrSaveAsync()
        {
            if (IsEditing)
                await SaveChangesAsync();
            else
                StartEditing();
        }

        private async Task SaveChangesAsync()
        {
            if (BookReview == null) return;

            IsEditing = false;
            OnPropertyChanged(nameof(IsNotEditing));
            OnPropertyChanged(nameof(EditButtonText));

            BookReview.Rating = EditableRating ?? "Gemiddeld";
            BookReview.Review = EditableReview ?? "";

            var updatedReview = await _reviewService.UpdateReviewAsync(BookReview.Id, BookReview);

            if (updatedReview == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Het is niet gelukt om de review op te slaan.", "OK");
            }
            else
            {
                BookReview = updatedReview;
                EditableRating = updatedReview.Rating ?? "Gemiddeld";
                EditableReview = updatedReview.Review ?? "";

                OnPropertyChanged(nameof(BookReview));
                OnPropertyChanged(nameof(EditableRating));
                OnPropertyChanged(nameof(EditableReview));

                WeakReferenceMessenger.Default.Send(new ReviewAddedOrUpdatedMessage(updatedReview));

                await App.Current.MainPage.DisplayAlert("Success", "Review succesvol geüpdatet!", "OK");
            }
        }


        private async Task DeleteReviewAsync()
        {
            if (BookReview == null) return;

            bool confirm = await App.Current.MainPage.DisplayAlert(
                "Bevestigen",
                "Weet je zeker dat je deze review wilt verwijderen?",
                "Ja",
                "Nee");

            if (!confirm) return;

            bool success = await _reviewService.DeleteReviewAsync(BookReview.Id);
            if (success)
            {
                WeakReferenceMessenger.Default.Send(new ReviewDeletedMessage(BookReview.Id));
                await GoBackAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Fout", "Verwijderen van de review is mislukt.", "OK");
            }
        }

        private Task GoBackAsync() =>
            _navigationService.GoBackAsync();
    }
}
