using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace BookScanner.Views
{
    public partial class ReviewsPage : ContentPage
    {
        private List<Book> books = new List<Book>
        {
            new Book { BookTitle = "Book Title 1", BookAuthor = "Author 1", BookCoverImage = "book1.jpg", BookRating = "4.5/5", BookReview = "This is a great book!" },
            new Book { BookTitle = "Book Title 2", BookAuthor = "Author 2", BookCoverImage = "book2.jpg", BookRating = "4.0/5", BookReview = "Enjoyed reading this one." },
            new Book { BookTitle = "Book Title 3", BookAuthor = "Author 3", BookCoverImage = "book3.jpg", BookRating = "3.5/5", BookReview = "It was okay." },
            new Book { BookTitle = "Book Title 4", BookAuthor = "Author 4", BookCoverImage = "book4.jpg", BookRating = "5.0/5", BookReview = "Highly recommended!" },
            new Book { BookTitle = "Book Title 5", BookAuthor = "Author 5", BookCoverImage = "book5.jpg", BookRating = "4.2/5", BookReview = "An interesting read." },
            new Book { BookTitle = "Book Title 6", BookAuthor = "Author 6", BookCoverImage = "book6.jpg", BookRating = "4.8/5", BookReview = "Loved every page!" },
        };

        public ReviewsPage()
        {
            InitializeComponent();
            PopulateBooks();
        }

        // Populate the book grid with book buttons
        private void PopulateBooks()
        {
            for (int i = 0; i < books.Count; i++)
            {
                var book = books[i];

                // Create a border for the book cover
                var bookCoverFrame = new Frame
                {
                    Padding = 0,
                    Margin = 10,
                    BorderColor = Colors.Gray, // Changed from Color.Gray to Colors.Gray
                    BackgroundColor = Colors.White,
                    CornerRadius = 10,
                    Content = new ImageButton
                    {
                        Source = book.BookCoverImage,
                        BackgroundColor = Colors.Transparent
                    }
                };

                // Set the image button click event
                ((ImageButton)bookCoverFrame.Content).Clicked += OnBookClicked;

                var titleLabel = new Label
                {
                    Text = book.BookTitle,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    BackgroundColor = Color.FromHex("#e0e0e0"),
                    Padding = new Thickness(5),
                    Margin = new Thickness(0, 5, 0, 0),
                    TextColor = Color.FromHex("#2c2c2c"),
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                };

                // Set grid position
                Grid.SetRow(bookCoverFrame, i / 3);
                Grid.SetColumn(bookCoverFrame, i % 3);
                Grid.SetRow(titleLabel, i / 3);
                Grid.SetColumn(titleLabel, i % 3);

                BookGrid.Children.Add(bookCoverFrame);
                BookGrid.Children.Add(titleLabel);
            }
        }

        // Event handler for when a book is clicked
        private async void OnBookClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            // Get the index of the clicked button
            int index = Grid.GetRow(button) * 3 + Grid.GetColumn(button);
            var selectedBook = books[index];

            // Navigate to the BookDetailPage with the selected book
            await Navigation.PushAsync(new BookDetailPage(selectedBook));
        }
    }

    // Book class definition
    public class Book
    {
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookCoverImage { get; set; }
        public string BookRating { get; set; }
        public string BookReview { get; set; }
    }
}
