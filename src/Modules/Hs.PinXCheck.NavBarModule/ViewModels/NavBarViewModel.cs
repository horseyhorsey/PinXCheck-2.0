using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using System.Windows.Media;

namespace NavBarModule.ViewModels
{
    public class NavBarViewModel : ViewModelBase
    {
        #region Properties
        private ImageSource fieldName;        

        public ImageSource PropertyName
        {
            get { return fieldName; }
            set { SetProperty(ref fieldName, value); }
        }
        #endregion

        #region Services
        private ISelectedService _selectedService;
        #endregion

        #region Ctors
        public NavBarViewModel(ISelectedService selectedService)
        {
            _selectedService = selectedService;
        }
        #endregion

    }
}
