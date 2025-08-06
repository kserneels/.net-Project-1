using BookRecognitionApp.ViewModels;

namespace BookRecognitionApp.Views
{
    public partial class NewBookPage : ContentPage
    {
        public NewBookPage(NewBookPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;  // DI automatically provides the view model
        }
    }
}
