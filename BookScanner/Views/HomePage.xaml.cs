using Microsoft.Maui.Controls;

namespace BookScanner.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void OnNewBookClicked(object sender, EventArgs e)
        {
            // Navigate to New Book Page
            await Navigation.PushAsync(new NewBookPage());
        }

        private async void OnReviewsClicked(object sender, EventArgs e)
        {
            // Navigate to Reviews Page
            await Navigation.PushAsync(new ReviewsPage());
        }
    }
}
