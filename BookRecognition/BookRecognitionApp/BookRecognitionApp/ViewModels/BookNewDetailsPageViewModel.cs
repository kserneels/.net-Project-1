using BookRecognitionApp.Messages;
using BookRecognitionApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;

namespace BookRecognitionApp.ViewModels
{
    public partial class BookNewDetailsPageViewModel : ObservableObject, IRecipient<BookSelectedMessage>
    {
        private readonly ReviewService _reviewService;
        private readonly ILogger<BookNewDetailsPageViewModel> _logger;

        [ObservableProperty] private BookInfo bookInfo;
        [ObservableProperty] private BookReview bookReview;
        [ObservableProperty] private string rating;
        [ObservableProperty] private string review;

        public string Title => BookInfo?.Title;
        public string Author => BookInfo?.Author;
        public string Year => BookInfo?.Year;
        public string ISBN => BookInfo?.ISBN;

        public string CoverUrl => string.IsNullOrEmpty((string?)(BookInfo?.CoverUrl)) || BookInfo.CoverUrl == "Resources/Images/no_cover.jpg"
            ? "no_cover.jpg"
            : BookInfo.CoverUrl;

        public IAsyncRelayCommand AddReviewCommand { get; }

        public BookNewDetailsPageViewModel(
            ReviewService reviewService,
            IMessenger messenger,
            ILogger<BookNewDetailsPageViewModel> logger)
        {
            _reviewService = reviewService;
            _logger = logger;

            messenger.Register<BookSelectedMessage>(this);
            _logger.LogInformation("📩 Registered BookNewDetailsPageViewModel for BookSelectedMessage");

            AddReviewCommand = new AsyncRelayCommand(OnAddReviewAsync);
        }
        public void Receive(BookSelectedMessage message)
        {
            if (message == null) return;

            BookInfo = message.BookInfo;

            BookReview = message.BookReview ?? new BookReview
            {
                Title = BookInfo.Title,
                Author = BookInfo.Author,
                Year = BookInfo.Year,
                ISBN = BookInfo.ISBN
            };

            Rating = BookReview.Rating;
            Review = BookReview.Review;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Author));
            OnPropertyChanged(nameof(Year));
            OnPropertyChanged(nameof(ISBN));
            OnPropertyChanged(nameof(CoverUrl));
        }

        private async Task OnAddReviewAsync()
        {
            try
            {
                BookReview.Title ??= BookInfo?.Title ?? "Unknown Title";
                BookReview.Author ??= BookInfo?.Author ?? "Unknown Author";
                BookReview.Year ??= BookInfo?.Year ?? "Unknown Year";
                BookReview.ISBN ??= BookInfo?.ISBN ?? "Unknown ISBN";
                BookReview.CoverUrl ??= BookInfo?.CoverUrl ?? "no_cover.jpg";
                BookReview.Rating ??= Rating ?? "0";
                BookReview.Review ??= Review ?? "";

                _logger.LogInformation("Sending BookReview to API: {@BookReview}", BookReview);

                var existingReviews = await _reviewService.GetAllReviewsAsync();
                bool isReviewExists = existingReviews.Exists(r => r.ISBN == BookReview.ISBN);

                if (isReviewExists)
                {
                    bool isEditing = await App.Current.MainPage.DisplayAlert(
                        "Waarschuwing",
                        "Dit boek is al gerecenseerd. Wil je deze review bewerken?", "Ja", "Nee");

                    if (isEditing)
                    {
                        var existingReview = existingReviews.FirstOrDefault(r => r.ISBN == BookReview.ISBN);
                        if (existingReview != null)
                        {
                            _logger.LogInformation("📖 Navigating to existing review for ISBN {ISBN}", existingReview.ISBN);
                            await Shell.Current.GoToAsync($"{nameof(BookDetailPage)}", true, new Dictionary<string, object>
                            {
                                ["SelectedReview"] = existingReview
                            });
                        }
                    }
                    else
                    {
                        _logger.LogInformation("🏠 User chose not to edit, returning to HomePage");
                        await Shell.Current.GoToAsync("///HomePage");
                    }
                    return;
                }

                bool result = await _reviewService.AddReviewAsync(BookReview);

                if (result)
                {
                    _logger.LogInformation("✅ Review for ISBN {ISBN} added successfully", BookReview.ISBN);
                    WeakReferenceMessenger.Default.Send(new ReviewAddedOrUpdatedMessage(BookReview));
                    await App.Current.MainPage.DisplayAlert("Succes", "Je recensie is ingediend.", "OK");
                    await Shell.Current.GoToAsync("///HomePage");
                }
                else
                {
                    _logger.LogError("❌ Failed to add review for ISBN {ISBN}.", BookReview.ISBN);
                    await App.Current.MainPage.DisplayAlert("Fout", "Het is niet gelukt om je recensie in te dienen.", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while adding review for ISBN {ISBN}", BookReview?.ISBN);
                await App.Current.MainPage.DisplayAlert("Fout", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }
    }
}
