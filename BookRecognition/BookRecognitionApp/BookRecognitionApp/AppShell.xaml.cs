using BookRecognitionApp.Views;
using Microsoft.Maui.Controls;

namespace BookRecognitionApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(NewBookPage), typeof(NewBookPage));
            Routing.RegisterRoute(nameof(ReviewsPage), typeof(ReviewsPage));
            Routing.RegisterRoute(nameof(BookNewDetailsPage), typeof(BookNewDetailsPage)); // Register the BookNewDetailsPage route
            Routing.RegisterRoute(nameof(BookDetailPage), typeof(BookDetailPage)); // Register the BookDetailPage route
        }
    }
}
