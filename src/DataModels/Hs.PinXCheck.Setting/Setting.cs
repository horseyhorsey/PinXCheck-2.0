using Prism.Mvvm;

namespace Hs.PinXCheck.Setting
{
    public class Settings : BindableBase
    {
        private string pinballXPath;
        public string PinballXPath
        {
            get { return pinballXPath; }
            set { SetProperty(ref pinballXPath, value); }
        }
    }
}
