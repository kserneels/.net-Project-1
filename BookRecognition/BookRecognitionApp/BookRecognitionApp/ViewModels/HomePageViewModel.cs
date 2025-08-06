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
        }

        //Na kijken: als ik probeer via de navigation service te navigeren, crasht het programma
        //Het heeft te maken denk ik met de navigatie methode zonder parameters die je kan vinden in de navigation service

        [RelayCommand]
        private async Task NavigateToNewBookPage()
        {
            //await _navigationService.NavigateToAsync("NewBookPage");

            await Shell.Current.GoToAsync("//NewBookPage");
        }

        [RelayCommand]
        private async Task NavigateToReviewsPage()
        {
            //await _navigationService.NavigateToAsync("reviewspage");
            await Shell.Current.GoToAsync(nameof(ReviewsPage));

        }

    }
}
