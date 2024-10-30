using CommunityToolkit.Mvvm.ComponentModel;

namespace BookScanner.Models
{
    public class SizeInfo : ObservableObject
    {
        private string weight = string.Empty;
        public string Weight
        {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        private string height = string.Empty;
        public string Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }
    }
}
