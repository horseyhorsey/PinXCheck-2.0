using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinXCheck.UnusedTables.Services
{
    public class GetFilesService : IGetFilesService
    {
        public FileInfo[] GetAllFilesInDirectory(string tablePath)
        {
            var di = new DirectoryInfo(tablePath);

            var fiArray = di.GetFiles();

            return fiArray;
        }
    }
}
