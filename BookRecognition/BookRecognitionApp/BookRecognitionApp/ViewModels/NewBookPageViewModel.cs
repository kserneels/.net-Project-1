using BookRecognitionApp.Navigation;
using BookRecognitionApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;

namespace BookRecognitionApp.ViewModels
{
    public partial class NewBookPageViewModel : ObservableObject
    {
        private readonly CustomVisionService _customVisionService;
        private readonly BookService _bookService;
        private readonly INavigationService _navigationService;
        private readonly IMessenger _messenger;
        private readonly ILogger<NewBookPageViewModel> _logger;

        [ObservableProperty]
        private ImageSource _bookCoverImageSource;

        public IAsyncRelayCommand TakePictureCommand { get; }
        public IAsyncRelayCommand ChooseFromGalleryCommand { get; }
        public IAsyncRelayCommand BackCommand { get; }

        public NewBookPageViewModel(
            CustomVisionService customVisionService,
            BookService bookService,
            INavigationService navigationService,
            IMessenger messenger,
            ILogger<NewBookPageViewModel> logger)
        {
            _customVisionService = customVisionService;
            _bookService = bookService;
            _navigationService = navigationService;
            _messenger = messenger;
            _logger = logger;

            TakePictureCommand = new AsyncRelayCommand(TakePicture);
            ChooseFromGalleryCommand = new AsyncRelayCommand(ChooseFromGallery);
            BackCommand = new AsyncRelayCommand(GoBackAsync);

            _logger.LogInformation("✅ NewBookPageViewModel created");
        }

        private async Task TakePicture()
        {
            if (!MediaPicker.Default.IsCaptureSupported) return;

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo != null) await ProcessImage(photo);
        }

        private async Task ChooseFromGallery()
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null) await ProcessImage(photo);
        }

        private async Task ProcessImage(FileResult photo)
        {
            try
            {
                using var stream = await photo.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                BookCoverImageSource = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                var imageData = memoryStream.ToArray();

                string isbn = await _customVisionService.ExtractISBN(imageData)
                               ?? await _customVisionService.ExtractISBNFromOCR(imageData);

                if (!string.IsNullOrEmpty(isbn))
                    await FetchAndSendBookInfo(isbn);
                else
                    await App.Current.MainPage.DisplayAlert("Error", "ISBN not detected. Try again.", "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error while processing image");
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task FetchAndSendBookInfo(string isbn)
        {
            var bookInfo = await _bookService.GetBookDetailsFromOpenLibrary(isbn);
            if (bookInfo != null)
            {
                var bookReview = new BookReview
                {
                    Title = bookInfo.Title,
                    Author = bookInfo.Author,
                    CoverUrl = bookInfo.CoverUrl,
                    Year = bookInfo.Year,
                    ISBN = bookInfo.ISBN
                };

                _logger.LogInformation("📤 Navigating to BookNewDetailsPage first...");
                await _navigationService.GoToAsync(nameof(BookNewDetailsPage));

                _logger.LogInformation("📤 Sending BookSelectedMessage for ISBN {ISBN}", bookInfo.ISBN);
                _messenger.Send(new BookSelectedMessage(bookInfo, bookReview));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Book information not found.", "OK");
            }
        }


        private Task GoBackAsync() =>
            _navigationService.GoBackAsync();
    }

    public record BookSelectedMessage(BookInfo BookInfo, BookReview BookReview);
}
