using Microsoft.Maui.Controls;

namespace BookScanner.Views
{
    public partial class BookDetailPage : ContentPage
    {
        public BookDetailPage(Book book)
        {
            InitializeComponent();
            BindingContext = book; // Set the BindingContext to the book passed
        }
    }
}
