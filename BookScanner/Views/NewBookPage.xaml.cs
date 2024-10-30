using Microsoft.Maui.Controls;

namespace BookScanner.Views
{
    public partial class NewBookPage : ContentPage
    {
        public NewBookPage()
        {
            InitializeComponent();
        }

        // Event handler for the "Take Picture" button click
        private async void OnTakePictureClicked(object sender, EventArgs e)
        {
            // You can add logic here to take a picture if needed

            // For now, just navigate to the Book Details Page
            await Navigation.PushAsync(new BookDetailsPage());
        }
    }
}
