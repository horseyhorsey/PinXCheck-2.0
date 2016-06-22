using Hs.PinXCheck.Setting;

namespace Hs.PinXCheck.Base.Interfaces
{
    public interface ISettingsRepo
    {
        Settings PinXCheckSettings { get; set; }

        void LoadPinXCheckSettings();

        void SavePinXCheckSettings();
    }
}
