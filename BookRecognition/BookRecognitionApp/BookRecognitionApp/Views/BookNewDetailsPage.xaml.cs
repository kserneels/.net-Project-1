using BookRecognitionApp.ViewModels;

namespace BookRecognitionApp.Views
{
    public partial class BookNewDetailsPage : ContentPage
    {
        public BookNewDetailsPage(BookNewDetailsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
