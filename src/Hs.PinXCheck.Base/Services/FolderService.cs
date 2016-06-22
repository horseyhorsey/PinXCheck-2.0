using System.Windows.Forms;

namespace Hs.PinXCheck.Base.Services
{
    public class FolderService : IFolderService
    {
        public string SelectedFolder { get; set; }

        public void setFolderDialog()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
                SelectedFolder = folderBrowserDialog.SelectedPath;
        }
    }
}
