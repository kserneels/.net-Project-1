using BookScanner.Models;
using System.Windows.Input;

namespace BookScanner.ViewModels
{
    public interface IInfoViewModel
    {
        ICommand PickPhotoCommand { get; set; }
        ICommand TakePhotoCommand { get; set; }

        BigFiveAnimal ClassifiedAnimal { get; set; }

        bool IsRunning { get; set; }

        ImageSource Photo { get; set; }
    }
}
