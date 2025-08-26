using BookRecognitionApp.ViewModels;

namespace BookRecognitionApp.Views
{
    public partial class ReviewsPage : ContentPage
    {
        public ReviewsPage(ReviewsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
