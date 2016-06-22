using System.Runtime.InteropServices;
using System.Text;

namespace Hs.PinXCheck.Services.IniFile
{
    public class IniFileReader
    {
        public string Path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="iniPath"></PARAM>
        public IniFileReader(string iniPath)
        {
            Path = iniPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="section"></PARAM>
        /// section name
        /// <PARAM name="key"></PARAM>
        /// key Name
        /// <PARAM name="value"></PARAM>
        /// value Name
        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }
        /// <summary>
        /// Read Data value From the Ini File
        /// </summary>
        /// <PARAM name="section"></PARAM>
        /// <PARAM name="key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string section, string Key)
        {
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(section, Key, "", temp,
                255, Path);
            return temp.ToString();

        }
    }
}
