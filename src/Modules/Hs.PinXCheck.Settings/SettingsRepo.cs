using Hs.PinXCheck.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hs.PinXCheck.Setting;

namespace Hs.PinXCheck.Settings
{
    public class SettingsRepo : ISettingsRepo
    {
        private Setting.Settings pinXCheckSettings = new Setting.Settings();
        public Setting.Settings PinXCheckSettings
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
