using BookRecognitionApp.ViewModels;
using Microsoft.Maui.Controls;

namespace BookRecognitionApp.Views
{
    public partial class ReviewsPage : ContentPage
    {
        public ReviewsPage(ReviewsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "PageAppeared");
        }
    }
}
