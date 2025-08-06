public interface INavigationService
{
    Task NavigateToAsync(string route);
    Task NavigateToAsync(string route, IDictionary<string, object> parameters); // ✅ Voeg deze toe
    Task GoBackAsync();
}
