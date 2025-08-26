using BookRecognitionApp.Messages;
using BookRecognitionApp.Navigation;
using BookRecognitionApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BookRecognitionApp.ViewModels
{
    public partial class ReviewsPageViewModel : ObservableObject
    {
        private readonly ReviewService _reviewService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<BookReview> Reviews { get; } = new ObservableCollection<BookReview>();

        public IAsyncRelayCommand<BookReview> NavigateToDetailCommand { get; }
        public IAsyncRelayCommand BackCommand { get; }
        public IAsyncRelayCommand LoadReviewsCommand { get; }

        public ReviewsPageViewModel(ReviewService reviewService, INavigationService navigationService)
        {
            _reviewService = reviewService;
            _navigationService = navigationService;

            NavigateToDetailCommand = new AsyncRelayCommand<BookReview>(NavigateToDetailPageAsync);
            BackCommand = new AsyncRelayCommand(GoBackAsync);
            LoadReviewsCommand = new AsyncRelayCommand(LoadReviewsAsync);

            WeakReferenceMessenger.Default.Register<ReviewDeletedMessage>(this, (r, msg) =>
            {
                var reviewToRemove = Reviews.FirstOrDefault(x => x.Id == msg.Value);
                if (reviewToRemove != null)
                    Reviews.Remove(reviewToRemove);
            });

            WeakReferenceMessenger.Default.Register<ReviewAddedOrUpdatedMessage>(this, (r, msg) =>
            {
                var existing = Reviews.FirstOrDefault(x => x.Id == msg.Value.Id);
                if (existing != null)
                {
                    var index = Reviews.IndexOf(existing);
                    Reviews[index] = msg.Value;
                }
                else
                {
                    Reviews.Add(msg.Value);
                }
            });

            _ = LoadReviewsAsync();
        }

        private async Task LoadReviewsAsync()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();

            Reviews.Clear();
            foreach (var review in reviews)
            {
                review.CoverUrl = string.IsNullOrEmpty(review.CoverUrl) || review.CoverUrl == "Resources/Images/no_cover.jpg"
                    ? "no_cover.jpg"
                    : review.CoverUrl;

                Reviews.Add(review);
            }
        }

        private async Task NavigateToDetailPageAsync(BookReview review)
        {
            if (review != null)
            {
                await _navigationService.GoToAsync(nameof(BookDetailPage), new Dictionary<string, object>
                {
                    { "SelectedReview", review }
                });
            }
        }

        private Task GoBackAsync() => _navigationService.GoBackAsync();
    }
}
