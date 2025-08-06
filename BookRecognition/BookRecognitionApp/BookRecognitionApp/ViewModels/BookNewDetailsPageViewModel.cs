//using BookRecognitionApp.Messages;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Mvvm.Messaging;

//namespace BookRecognitionApp.ViewModels

//{
//    [QueryProperty(nameof(BookInfo), "BookInfo")]
//    [QueryProperty(nameof(BookReview), "BookReview")]
//    public partial class BookNewDetailsPageViewModel : ObservableObject
//    {
//        private readonly INavigationService _navigationService;
//        private readonly ReviewService _reviewService;



//        ReviewService reviewService,
// INavigationService navigationService,
//    NavigationDataService navigationDataService)
//{
//    _reviewService = reviewService;
//    _navigationService = navigationService;
//    _navigationDataService = navigationDataService;

//    // ✅ Haal BookInfo & BookReview op uit de service
//    BookInfo = _navigationDataService.BookInfo;
//    BookReview = _navigationDataService.BookReview;

//    // Eventueel ook:
//    Rating = BookReview?.Rating;
//    Review = BookReview?.Review;



//        public string[] Ratings { get; } = new[] { "Uitstekend", "Goed", "Gemiddeld", "Slecht" };

//        public string CoverUrl =>
//            string.IsNullOrEmpty(BookInfo?.CoverUrl) || BookInfo.CoverUrl == "Resources/Images/no_cover.jpg"
//            ? "no_cover.jpg"
//            : BookInfo.CoverUrl;

//        public string Title => BookInfo?.Title;
//        public string Author => BookInfo?.Author;
//        public string Year => BookInfo?.Year;
//        public string ISBN => BookInfo?.ISBN;

//        public BookNewDetailsPageViewModel(ReviewService reviewService, INavigationService navigationService)
//        {
//            _reviewService = reviewService;
//            _navigationService = navigationService;
//        }

//        partial void OnBookInfoChanged(BookInfo value)
//        {
//            BookReview = new BookReview
//            {
//                Title = value?.Title,
//                Author = value?.Author,
//                Year = value?.Year,
//                ISBN = value?.ISBN,
//                Rating = string.Empty,
//                Review = string.Empty
//            };

//            OnPropertyChanged(nameof(Title));
//            OnPropertyChanged(nameof(Author));
//            OnPropertyChanged(nameof(CoverUrl));
//            OnPropertyChanged(nameof(Year));
//            OnPropertyChanged(nameof(ISBN));
//        }

//        [RelayCommand]
//        private async Task AddReviewAsync()
//        {
//            var existingReviews = await _reviewService.GetAllReviewsAsync();
//            var existingReview = existingReviews.FirstOrDefault(r => r.ISBN == BookReview.ISBN);

//            if (existingReview != null)
//            {
//                bool isEditing = await Application.Current.MainPage.DisplayAlert("Waarschuwing",
//                    "Dit boek is al gerecenseerd. Wil je deze review bewerken?", "Ja", "Nee");

//                if (isEditing)
//                {
//                    WeakReferenceMessenger.Default.Send(new BookSelectedMessage(existingReview));
//                    await _navigationService.NavigateToAsync("BookDetailPage");
//                }
//                else
//                {
//                    await _navigationService.NavigateToAsync("HomePage");
//                }

//                return;
//            }

//            BookReview.Rating = Rating;
//            BookReview.Review = Review;

//            bool result = await _reviewService.AddReviewAsync(BookReview);

//            if (result)
//            {
//                await Application.Current.MainPage.DisplayAlert("Succes", "Je recensie is ingediend.", "OK");
//                await _navigationService.NavigateToAsync("HomePage");
//            }
//            else
//            {
//                await Application.Current.MainPage.DisplayAlert("Fout", "Het is niet gelukt om je recensie in te dienen.", "OK");
//            }
//        }
//    }
//}


using BookRecognitionApp.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BookRecognitionApp.ViewModels
{
    public partial class BookNewDetailsPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly ReviewService _reviewService;
        private readonly NavigationDataService _navigationDataService;

        [ObservableProperty]
        private BookInfo bookInfo;

        [ObservableProperty]
        private BookReview bookReview;

        [ObservableProperty]
        private string rating;

        [ObservableProperty]
        private string review;

        public string[] Ratings { get; } = new[] { "Uitstekend", "Goed", "Gemiddeld", "Slecht" };

        public string CoverUrl =>
            string.IsNullOrEmpty(BookInfo?.CoverUrl) || BookInfo.CoverUrl == "Resources/Images/no_cover.jpg"
            ? "no_cover.jpg"
            : BookInfo.CoverUrl;

        public string Title => BookInfo?.Title;
        public string Author => BookInfo?.Author;
        public string Year => BookInfo?.Year;
        public string ISBN => BookInfo?.ISBN;

        public BookNewDetailsPageViewModel(
            ReviewService reviewService,
            INavigationService navigationService,
            NavigationDataService navigationDataService)
        {
            _reviewService = reviewService;
            _navigationService = navigationService;
            _navigationDataService = navigationDataService;

            // ✅ Data ophalen uit NavigationDataService
            BookInfo = _navigationDataService.BookInfo;
            BookReview = _navigationDataService.BookReview;

            // ✅ Initialiseer inputvelden
            Rating = BookReview?.Rating ?? string.Empty;
            Review = BookReview?.Review ?? string.Empty;

            // ✅ Log eventueel
            if (BookInfo == null)
            {
                Console.WriteLine("⚠️ BookInfo is null!");
            }
        }

        partial void OnBookInfoChanged(BookInfo value)
        {
            BookReview = new BookReview
            {
                Title = value?.Title,
                Author = value?.Author,
                Year = value?.Year,
                ISBN = value?.ISBN,
                Rating = string.Empty,
                Review = string.Empty
            };

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Author));
            OnPropertyChanged(nameof(CoverUrl));
            OnPropertyChanged(nameof(Year));
            OnPropertyChanged(nameof(ISBN));
        }

        [RelayCommand]
        private async Task AddReviewAsync()
        {
            var existingReviews = await _reviewService.GetAllReviewsAsync();
            var existingReview = existingReviews.FirstOrDefault(r => r.ISBN == BookInfo.ISBN);

            if (existingReview != null)
            {
                bool isEditing = await Application.Current.MainPage.DisplayAlert("Waarschuwing",
                    "Dit boek is al gerecenseerd. Wil je deze review bewerken?", "Ja", "Nee");

                if (isEditing)
                {
                    WeakReferenceMessenger.Default.Send(new BookSelectedMessage(existingReview));
                    await _navigationService.NavigateToAsync("BookDetailPage");
                }
                else
                {
                    await _navigationService.NavigateToAsync("HomePage");
                }

                return;
            }

            // ✅ Construct the DTO with full book + review data
            var dto = new BookReviewDTO
            {
                Title = BookInfo.Title,
                Author = BookInfo.Author,
                Year = BookInfo.Year,
                ISBN = BookInfo.ISBN,
                CoverUrl = BookInfo.CoverUrl,
                Rating = Rating,
                Review = Review,
                ReviewDate = DateTime.Now
            };

            bool result = await _reviewService.AddReviewAsync(dto);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Succes", "Je recensie is ingediend.", "OK");
                await _navigationService.NavigateToAsync("HomePage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Het is niet gelukt om je recensie in te dienen.", "OK");
            }
        }
    }
}
