using BookRecognitionApp.Navigation;
using BookRecognitionApp.ViewModels;
using BookRecognitionApp.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;

namespace BookRecognitionApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ViewModels
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<NewBookPageViewModel>();
            builder.Services.AddTransient<BookNewDetailsPageViewModel>();
            builder.Services.AddTransient<ReviewsPageViewModel>();
            builder.Services.AddTransient<BookDetailViewModel>();

            // Pages
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<NewBookPage>();
            builder.Services.AddTransient<BookNewDetailsPage>();
            builder.Services.AddTransient<ReviewsPage>();
            builder.Services.AddTransient<BookDetailPage>();

            // Services
            builder.Services.AddTransient<BookService>();
            builder.Services.AddTransient<ReviewService>();
            builder.Services.AddSingleton<CustomVisionService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            // Messenger (shared instance)
            builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            // Networking
            builder.Services.AddHttpClient();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
