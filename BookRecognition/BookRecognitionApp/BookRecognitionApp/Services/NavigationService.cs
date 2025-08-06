public class NavigationService : INavigationService
{
    public NavigationService() { } // ✅ Parameterloze constructor

    public Task NavigateToAsync(string route) =>
        Shell.Current.GoToAsync(route);

    public Task NavigateToAsync(string route, IDictionary<string, object> parameters) =>
        Shell.Current.GoToAsync(route, parameters);

    public Task GoBackAsync() =>
        Shell.Current.GoToAsync("..");
}
