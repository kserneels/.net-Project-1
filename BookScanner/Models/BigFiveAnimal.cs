using CommunityToolkit.Mvvm.ComponentModel;

namespace BookScanner.Models
{
    public class BigFiveAnimal : ObservableObject
    {
        private string tag = string.Empty;
        public string Tag
        {
            get => tag;
            set => SetProperty(ref tag, value);
        }

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string description = string.Empty;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private SizeInfo size = new SizeInfo();
        public SizeInfo Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }

        private string socialStructure = string.Empty;
        public string SocialStructure
        {
            get => socialStructure;
            set => SetProperty(ref socialStructure, value);
        }

        private string diet = string.Empty;
        public string Diet
        {
            get => diet;
            set => SetProperty(ref diet, value);
        }

        private string territorialBehavior = string.Empty;
        public string TerritorialBehavior
        {
            get => territorialBehavior;
            set => SetProperty(ref territorialBehavior, value);
        }

        private string lifespan = string.Empty;
        public string Lifespan
        {
            get => lifespan;
            set => SetProperty(ref lifespan, value);
        }
    }
}
