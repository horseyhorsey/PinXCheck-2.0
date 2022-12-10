using System.IO;
using System.Windows.Forms;

namespace Hs.PinXCheck.Base.Services
{
    public class FileService : IFileService
    {
        public string[] GetFileNameDialog()
        {            
            var fileArray = new string[2];            

            var fileBrowserDialog = new OpenFileDialog();
            var result = fileBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileArray[0] = Path.GetDirectoryName(fileBrowserDialog.FileName);
                fileArray[1] = Path.GetFileName(fileBrowserDialog.FileName);                
            }

            return fileArray;
        }
    }
}
