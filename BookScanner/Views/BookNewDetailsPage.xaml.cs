using Microsoft.Maui.Controls;

namespace BookScanner.Views
{
    public partial class BookDetailsPage : ContentPage
    {
        public BookDetailsPage()
        {
            InitializeComponent();
        }

        private async void OnAddBookClicked(object sender, EventArgs e)
        {
            var bookTitle = TitleEntry.Text;
            var author = AuthorEntry.Text;
            var releaseDate = ReleaseDateEntry.Text;
            var review = ReviewEditor.Text;
            var selectedRating = RatingPicker.SelectedItem as string;

            // Display a summary of the entered details, including the rating
            await DisplayAlert("Book Added",
                $"Title: {bookTitle}\nAuthor: {author}\nRelease Date: {releaseDate}\n" +
                $"Rating: {selectedRating}\nReview: {review}", "OK");

            // Add logic to save the book details or perform additional actions if necessary
        }
    }
}
