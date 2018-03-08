using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hs.PinXCheck.Base.Services
{
    public class SelectedService : ISelectedService
    {
        public ImageSource SystemLogo { get; set; }

        public string CurrentSystem { get; set; }

        public string CurrentTablePath { get; set; }

        public int CurrentSystemType { get; set; }

        public string CurrentWorkingPath { get; set; }

        public string SelectedTableName { get; set; }

        public string SelectedDescription { get; set; }

        public string SelectedPublisher { get; set; }

        public string CurrentSystemDefaultExecutable { get; set; }

        /// <summary>
        /// Get imagesource from URI file link
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ImageSource SetBitmapFromUri(Uri source)
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
            catch (Exception) { return null; }

        }

    }
}
