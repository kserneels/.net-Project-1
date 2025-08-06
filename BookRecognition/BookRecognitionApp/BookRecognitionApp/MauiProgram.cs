using BookRecognitionApp.ViewModels;
using BookRecognitionApp.Views;
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

            // Register pages and view models
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<NewBookPageViewModel>();
            builder.Services.AddTransient<BookNewDetailsPageViewModel>();
            builder.Services.AddTransient<ReviewsPageViewModel>();
            builder.Services.AddTransient<BookDetailViewModel>();

            // Register pages
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<NewBookPage>();
            builder.Services.AddTransient<BookNewDetailsPage>();
            builder.Services.AddTransient<ReviewsPage>();
            builder.Services.AddTransient<BookDetailPage>();

            // Register services
            builder.Services.AddTransient<BookService>();
            builder.Services.AddTransient<ReviewService>();
            builder.Services.AddSingleton<CustomVisionService>(); // Register as singleton if it holds state or settings
            builder.Services.AddTransient<INavigationService, NavigationService>();
            builder.Services.AddSingleton<NavigationDataService>();


            // Register HttpClient for network calls
            builder.Services.AddHttpClient();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
