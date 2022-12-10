using Prism.Mvvm;

namespace Hs.PinXCheck.Base
{
    public class PinXCheckSettings : BindableBase
    {
        private string pinballXPath;
        public string PinballXPath
        {
            get { return pinballXPath; }
            set { SetProperty(ref pinballXPath, value); }
        }
    }
}
