using System.Windows.Media;

namespace Hs.PinXCheck.Base.Services
{
    public interface ISelectedService
    {
        ImageSource SystemLogo { get; set; }

        string CurrentSystem { get; set; }

        int CurrentSystemType { get; set; }

        string CurrentTablePath { get; set; }

        string CurrentWorkingPath { get; set; }

        string SelectedTableName { get; set; }

        string SelectedDescription { get; set; }

        string SelectedPublisher { get; set; }
    }
}
