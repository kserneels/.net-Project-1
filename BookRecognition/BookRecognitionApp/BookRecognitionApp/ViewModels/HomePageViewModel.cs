using BookRecognitionApp.Navigation;
using BookRecognitionApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookRecognitionApp.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public HomePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OnNewBookClicked = new AsyncRelayCommand(OnNewBook);
            OnReviewsClicked = new AsyncRelayCommand(OnReviews);
        }

        public IAsyncRelayCommand OnNewBookClicked { get; }
        public IAsyncRelayCommand OnReviewsClicked { get; }

        private Task OnNewBook() =>
            _navigationService.GoToAsync(nameof(NewBookPage));

        private Task OnReviews() =>
            _navigationService.GoToAsync(nameof(ReviewsPage));
    }
}
