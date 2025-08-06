//using BookRecognitionApp.Messages;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Mvvm.Messaging;
//using System.Collections.ObjectModel;

//namespace BookRecognitionApp.ViewModels
//{
//    public partial class ReviewsPageViewModel : ObservableObject
//    {
//        private readonly ReviewService _reviewService;
//        private readonly INavigationService _navigationService;

//        [ObservableProperty]
//        private ObservableCollection<BookReview> reviews = new();

//        public ReviewsPageViewModel(ReviewService reviewService, INavigationService navigationService)
//        {
//            _reviewService = reviewService;
//            _navigationService = navigationService;
//        }

//        public ObservableCollection<ReviewDisplayDto> Reviews { get; set; } = new();

//        public async Task LoadReviewsAsync()
//        {
//            try
//            {
//                var reviewList = await _reviewService.GetAllReviewsAsync();
//                Reviews.Clear();
//                foreach (var review in reviewList)
//                {
//                    Reviews.Add(review);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Error loading reviews: {ex.Message}");
//            }
//        }

//        [RelayCommand]
//        private async Task LoadReviewsAsync()
//        {
//            var reviewList = await _reviewService.GetAllReviewsAsync();
//            if (reviewList == null) return;

//            Reviews.Clear();

//            foreach (var review in reviewList)
//            {
//                review.CoverUrl = string.IsNullOrEmpty(review.CoverUrl) || review.CoverUrl == "Resources/Images/no_cover.jpg"
//                    ? "no_cover.jpg"
//                    : review.CoverUrl;

//                Reviews.Add(review);
//            }
//        }

//        [RelayCommand]
//        private async Task NavigateToDetailAsync(BookReview review)
//        {
//            if (review == null) return;

//            WeakReferenceMessenger.Default.Send(new BookSelectedMessage(review));
//            await _navigationService.NavigateToAsync("BookDetailPage");
//        }

//        [RelayCommand]
//        private async Task GoBackAsync()
//        {
//            await _navigationService.NavigateToAsync("HomePage");
//        }
//    }
//}


//Hier boven vind je de oude code van ons eerste project, nu voor  of andere reden laden onze reviews niet meer
//Kijk zeker eens naar de dto's en de backend-api want ik denk dat ik daar mee de fout heb.
using BookRecognitionApp.Messages;
using BookRecognitionApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BookRecognitionApp.ViewModels;

public partial class ReviewsPageViewModel : ObservableObject
{
    private readonly ReviewService _reviewService;
    private readonly INavigationService _navigationService;

    public ReviewsPageViewModel(ReviewService reviewService, INavigationService navigationService)
    {
        _reviewService = reviewService;
        _navigationService = navigationService;

        Task.Run(async () => await LoadReviewsAsync());
    }

    public ObservableCollection<ReviewDisplayDto> Reviews { get; set; } = new();

    [RelayCommand]
    private async Task LoadReviewsAsync()
    {
        try
        {
            var reviewList = await _reviewService.GetAllReviewsAsync();
            if (reviewList == null) return;

            Reviews.Clear();

            foreach (var review in reviewList)
            {
                review.CoverUrl = string.IsNullOrEmpty(review.CoverUrl) || review.CoverUrl == "Resources/Images/no_cover.jpg"
                    ? "no_cover.jpg"
                    : review.CoverUrl;

                Reviews.Add(review);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error loading reviews: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NavigateToDetailAsync(ReviewDisplayDto review)
    {
        if (review == null) return;

        WeakReferenceMessenger.Default.Send(new BookSelectedMessage(review));
        await _navigationService.NavigateToAsync("BookDetailPage");
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await _navigationService.NavigateToAsync("HomePage");
    }
}
