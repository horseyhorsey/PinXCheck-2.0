using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinXCheck.Base.Services
{
    public interface IFolderService
    {
        string SelectedFolder { get; set; }

        void setFolderDialog();
        
    }
}
