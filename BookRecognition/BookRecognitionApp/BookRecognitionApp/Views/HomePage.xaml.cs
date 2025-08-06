using BookRecognitionApp.ViewModels;

namespace BookRecognitionApp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomePageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
