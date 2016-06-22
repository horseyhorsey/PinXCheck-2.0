using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Interfaces;
using System.Windows.Controls;

namespace Hs.PinXCheck.Shell.Views
{
    /// <summary>
    /// Interaction logic for SettingsFlyout
    /// </summary>
    public partial class SystemSettingsFlyout : IFlyoutView
    {
        public SystemSettingsFlyout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The flyout name
        /// </summary>
        public string FlyoutName
        {
            get { return FlyoutNames.SystemSettingsFlyout; }
        }
    }
}
