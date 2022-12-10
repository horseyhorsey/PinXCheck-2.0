using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Prism.Events;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hs.PinXCheck.Media.Pane.ViewModels
{
    public class MediaPaneViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;

        #region Properties
        
        private ISelectedService _selectedService;

        private ImageSource wheelSource;
        public ImageSource WheelSource
        {
            get { return wheelSource; }
            set { SetProperty(ref wheelSource, value); }
        }

        private ImageSource publisherSource;
        public ImageSource PublisherSource
        {
            get { return publisherSource; }
            set { SetProperty(ref publisherSource, value); }
        }

        
        #endregion

        public MediaPaneViewModel(IEventAggregator ea, ISelectedService selected)
        {
            _eventAggregator = ea;
            _selectedService = selected;

            _eventAggregator.GetEvent<TableSelectedEvent>().Subscribe(SetWheelImage);
        }

        private void SetWheelImage(string systemMediaDirectory)
        {
            WheelSource = null;
            PublisherSource = null;

            var wheel = systemMediaDirectory + "//" + 
                _selectedService.CurrentSystem +  "//Wheel Images//" + _selectedService.SelectedDescription;
            var publisher = systemMediaDirectory + "//Company Logos//" + _selectedService.SelectedPublisher;

            try { WheelSource = SetBitmapFromUri(new Uri(wheel + ".png"));}
            catch (Exception) { }

            try { PublisherSource = SetBitmapFromUri(new Uri(publisher + ".png"));}
            catch (Exception) { }
           
        }

        /// <summary>
        /// Get imagesource from URI file link
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ImageSource SetBitmapFromUri(Uri source)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = source;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.EndInit();

                return bitmap;

            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
