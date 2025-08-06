using BookRecognitionApp.ViewModels;
using Microsoft.Maui.Controls;

namespace BookRecognitionApp.Views
{
    public partial class BookDetailPage : ContentPage
    {
        public BookDetailPage(BookDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
