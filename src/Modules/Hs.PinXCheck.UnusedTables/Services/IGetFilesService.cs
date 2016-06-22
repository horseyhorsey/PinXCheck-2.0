using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinXCheck.UnusedTables.Services
{
    public interface IGetFilesService
    {
        FileInfo[] GetAllFilesInDirectory(string tablePath);
    }
}
