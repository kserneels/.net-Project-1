using BookRecognitionApp.Messages;
using BookRecognitionApp.Navigation;
using BookRecognitionApp.Views;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BookRecognitionApp.ViewModels
{
    public class ReviewsPageViewModel : INotifyPropertyChanged
    {
        private readonly ReviewService _reviewService;
        private readonly INavigationService _navigationService;

        // ObservableCollection of reviews
        public ObservableCollection<BookReview> Reviews { get; } = new ObservableCollection<BookReview>();

        // Commands for navigation
        public ICommand NavigateToDetailCommand { get; }
        public ICommand BackCommand { get; }

        public ReviewsPageViewModel(ReviewService reviewService, INavigationService navigationService)
        {
            _reviewService = reviewService;
            _navigationService = navigationService;

            // Command initialization
            NavigateToDetailCommand = new Command<BookReview>(async (review) => await NavigateToDetailPageAsync(review));
            BackCommand = new Command(async () => await GoBackAsync());

            // Subscribe to deletion messages
            WeakReferenceMessenger.Default.Register<ReviewDeletedMessage>(this, (r, msg) =>
            {
                var reviewToRemove = Reviews.FirstOrDefault(x => x.Id == msg.Value);
                if (reviewToRemove != null)
                    Reviews.Remove(reviewToRemove);
            });

            // Subscribe to page appearance event to load reviews
            MessagingCenter.Subscribe<ReviewsPage>(this, "PageAppeared", async (sender) =>
            {
                await LoadReviewsAsync();
            });
        }

        // Method to load reviews
        public async Task LoadReviewsAsync()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();

            if (reviews != null)
            {
                Reviews.Clear();
                foreach (var review in reviews)
                {
                    string coverImage = "no_cover.jpg"; // default image
                    if (!string.IsNullOrEmpty(review.CoverUrl) && review.CoverUrl != "Resources/Images/no_cover.jpg")
                        coverImage = review.CoverUrl;

                    review.CoverUrl = coverImage;
                    Reviews.Add(review);
                }
            }
        }

        // Navigation to detail page
        private async Task NavigateToDetailPageAsync(BookReview review)
        {
            if (review != null)
            {
                await _navigationService.GoToAsync(nameof(BookDetailPage), new Dictionary<string, object>
        {
            { "BookReview", review }
        });
            }
        }




        // Go back using navigation service
        private Task GoBackAsync() =>
            _navigationService.GoBackAsync();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}




