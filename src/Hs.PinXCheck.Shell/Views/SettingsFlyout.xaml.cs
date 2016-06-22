using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Interfaces;
using System.Windows.Controls;

namespace Hs.PinXCheck.Shell.Views
{
    /// <summary>
    /// Interaction logic for SettingsFlyout
    /// </summary>
    public partial class SettingsFlyout : IFlyoutView
    {
        public SettingsFlyout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The flyout name
        /// </summary>
        public string FlyoutName
        {
            get { return FlyoutNames.SettingsFlyout; }
        }
    }
}
