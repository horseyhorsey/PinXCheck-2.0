using Hs.PinXCheck.Base;
using Hs.PinXCheck.Base.Interfaces;

namespace Hs.PinXCheck.Settings
{
    public class SettingsRepo : ISettingsRepo
    {
        private PinXCheckSettings pinXCheckSettings = new PinXCheckSettings();
        public PinXCheckSettings PinXCheckSettings
        {
            get { return pinXCheckSettings; }
            set { pinXCheckSettings = value; }
        }
        
        public void LoadPinXCheckSettings()
        {
            PinXCheckSettings.PinballXPath = Properties.Settings.Default.PinballXPath;               
        }   
        
        public void SavePinXCheckSettings()
        {
            Properties.Settings.Default.PinballXPath  = PinXCheckSettings.PinballXPath;            

            Properties.Settings.Default.Save();
        }     

    }
}
