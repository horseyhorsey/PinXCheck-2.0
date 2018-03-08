using Prism.Mvvm;
using System.ComponentModel;

namespace Hs.PinXCheck.Database.Editing.Models
{
    public class DbEditOption : BindableBase
    {        
        private ICollectionView executableList;
        public ICollectionView ExecutableList
        {
            get { return executableList; }
            set { SetProperty(ref executableList, value); }
        }

        public int RadioGroupSelected { get; set; }
    }
}
