using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookRecognitionApp.ViewModels
{
    public partial class NewBookPageViewModel : ObservableObject
    {
        private readonly CustomVisionService _customVisionService;
        private readonly BookService _bookService;
        private readonly INavigationService _navigationService;
        private readonly NavigationDataService _navigationDataService;

        [ObservableProperty]
        private ImageSource bookCoverImageSource;

        public NewBookPageViewModel(
            CustomVisionService customVisionService,
            BookService bookService,
            INavigationService navigationService,
            NavigationDataService navigationDataService)
        {
            _customVisionService = customVisionService;
            _bookService = bookService;
            _navigationService = navigationService;
            _navigationDataService = navigationDataService;
        }

        [RelayCommand]
        private async Task TakePictureAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                    await ProcessImageAsync(photo);
            }
        }

        [RelayCommand]
        private async Task ChooseFromGalleryAsync()
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
                await ProcessImageAsync(photo);
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private async Task ProcessImageAsync(FileResult photo)
        {
            try
            {
                using var stream = await photo.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                BookCoverImageSource = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                byte[] imageData = memoryStream.ToArray();

                string isbn = await _customVisionService.ExtractISBN(imageData)
                             ?? await _customVisionService.ExtractISBNFromOCR(imageData);

                if (!string.IsNullOrEmpty(isbn))
                {
                    await FetchAndDisplayBookInfoAsync(isbn);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "ISBN not detected in the photo. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task FetchAndDisplayBookInfoAsync(string isbn)
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

                await Application.Current.MainPage.DisplayAlert("Success", "ISBN Found", "OK");

                _navigationDataService.BookInfo = bookInfo;
                _navigationDataService.BookReview = bookReview;

                await _navigationService.NavigateToAsync("BookNewDetailsPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Book information not found.", "OK");
            }
        }
    }
}

