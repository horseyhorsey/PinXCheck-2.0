namespace Hs.PinXCheck.Base.Interfaces
{
    public interface ISettingsRepo
    {
        PinXCheckSettings PinXCheckSettings { get; set; }

        void LoadPinXCheckSettings();

        void SavePinXCheckSettings();
    }
}
