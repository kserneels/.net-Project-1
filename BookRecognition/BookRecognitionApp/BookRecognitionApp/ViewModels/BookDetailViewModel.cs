using BookRecognitionApp.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BookRecognitionApp.ViewModels;

public partial class BookDetailViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ReviewService _reviewService;

    [ObservableProperty]
    private BookReview? bookReview;

    [ObservableProperty]
    private string? editableRating;

    [ObservableProperty]
    private string? editableReview;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private bool isBusy;

    public string[] Ratings { get; } = new[] { "Uitstekend", "Goed", "Gemiddeld", "Slecht" };

    public string EditButtonText => IsEditing ? "Opslaan" : "Bewerken";
    public bool IsNotEditing => !IsEditing;

    public BookDetailViewModel(ReviewService reviewService, INavigationService navigationService)
    {
        _reviewService = reviewService;
        _navigationService = navigationService;

        WeakReferenceMessenger.Default.Register<BookSelectedMessage>(this, OnBookSelected);
    }

    private void OnBookSelected(object recipient, BookSelectedMessage message)
    {
        var dto = message.Value;

        // ✅ Map DTO to BookReview
        BookReview = new BookReview
        {
            Id = dto.Id,
            Title = dto.Title,
            Rating = dto.Rating,
            Review = dto.Review,
            ReviewDate = dto.ReviewDate,
            CoverUrl = dto.CoverUrl,
            ISBN = dto.ISBN ?? "Onbekend", // fallback if null
            Author = dto.Author ?? "Onbekend",
            Year = dto.Year ?? "Onbekend"
        };

        EditableRating = BookReview.Rating;
        EditableReview = BookReview.Review;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        IsBusy = true;
        await _navigationService.GoBackAsync();
        IsBusy = false;
    }

    [RelayCommand]
    private void EditOrSave()
    {
        if (IsEditing)
            SaveChangesCommand.Execute(null);
        else
            StartEditing();
    }

    private void StartEditing()
    {
        IsEditing = true;
        EditableRating = BookReview?.Rating;
        EditableReview = BookReview?.Review;
        OnPropertyChanged(nameof(EditButtonText));
        OnPropertyChanged(nameof(IsNotEditing));
    }

    [RelayCommand]
    private void CancelEditing()
    {
        IsEditing = false;
        OnPropertyChanged(nameof(EditButtonText));
        OnPropertyChanged(nameof(IsNotEditing));
    }

    [RelayCommand]
    private async Task SaveChangesAsync()
    {
        if (BookReview == null) return;

        IsBusy = true;
        IsEditing = false;
        OnPropertyChanged(nameof(EditButtonText));
        OnPropertyChanged(nameof(IsNotEditing));

        BookReview.Rating = EditableRating;
        BookReview.Review = EditableReview;

        var updatedReview = await _reviewService.UpdateReviewAsync(BookReview.Id, BookReview);

        if (updatedReview == null)
        {
            IsBusy = false;
            await Application.Current.MainPage.DisplayAlert("Error", "Het is niet gelukt om de review op te slaan.", "OK");
            return;
        }

        await Application.Current.MainPage.DisplayAlert("Success", "Review succesvol geüpdatet!", "OK");

        var refreshedReview = await _reviewService.GetReviewAsync(updatedReview.Id);
        if (refreshedReview != null)
        {
            BookReview = refreshedReview;
            EditableRating = refreshedReview.Rating;
            EditableReview = refreshedReview.Review;
        }

        IsBusy = false;
    }

    [RelayCommand]
    private async Task DeleteReviewAsync()
    {
        if (BookReview == null) return;

        IsBusy = true;

        bool confirm = await Application.Current.MainPage.DisplayAlert("Bevestigen", "Weet je zeker dat je deze review wilt verwijderen?", "Ja", "Nee");
        if (!confirm)
        {
            IsBusy = false;
            return;
        }

        bool success = await _reviewService.DeleteReviewAsync(BookReview.Id);
        if (success)
        {
            await GoBackAsync();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Verwijderen van de review is mislukt.", "OK");
        }

        IsBusy = false;
    }
}
