namespace BookRecognitionApp.Navigation
{
    public class NavigationService : INavigationService
    {
        public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            if (parameters is not null)
                return Shell.Current.GoToAsync(route, true, parameters);

            return Shell.Current.GoToAsync(route);
        }

        public Task GoBackAsync() =>
            Shell.Current.GoToAsync("..");
    }

}
