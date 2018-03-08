using AutoHotkey.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hs.Services.VisualPinball
{
    public interface IVisualPinball
    {
        void LaunchVp(string tPath, string tName, string vpExe, string workingPath, bool editor = false, bool script = false);

        string GetInfoVisualPinball(int sysType, string tableName, string tablePath, string xmlElement);
    }

    public class VisualPinball : IVisualPinball
    {
        public const string SevenZipExe = @"7za.exe";
        private AutoHotkeyEngine _ahk;

        public static string SevenZipPath { get; set; }

        public VisualPinball()
        {
            _ahk = AutoHotkeyEngine.Instance;
            _ahk.LoadFile("launch.ahk");
        }

        /// <summary>
        /// Get table info from a Visual Pinball table
        /// </summary>
        /// <param name="tableFile"></param>
        /// <param name="tablename"></param>
        /// <param name="system"></param>
        /// <param name="xmlElement"></param>
        public void GetTableInfo(string tableFile, string tablename, int sysType, string xmlElement)
        {            

            var di = new DirectoryInfo("Temp");
            var arguments = string.Empty;

            deleteOldData();            

            if (sysType == 1 || sysType == 2)
                switch (xmlElement)
                {
                    case "Author":
                        if (sysType == 2)
                            arguments = @"e " + tableFile + " o" + di + " " + "\"" + "Future Pinball\\Table Data" + "\"" + " -y";
                        else
                            arguments = @"e " + tableFile + " o" + di + " TableInfo\\AuthorName -y";
                        break;
                    case "Rom":
                        arguments = @"e " + tableFile + " o" + di + " GameStg\\GameData -y";
                        break;
                    default:

                        break;
                }

            var si = new ProcessStartInfo(SevenZipPath, arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(si);
            if (process != null) process.WaitForExit();
        }

        private void deleteOldData()
        {
            try
            {
                File.Delete("Table Data");
                File.Delete("AuthorName");
                File.Delete("GameData");
            }
            catch (Exception) { }
            
        }

        public string GetInfoVisualPinball(int sysType, string tableName, string tablePath, string xmlElement)
        {
            var returnString = "";

            try
            {
                var tableFile = tablePath + "\\" + tableName + ".vpt";

                if (!File.Exists(tableFile))
                {
                    tableFile = tablePath + "\\" + tableName + ".vpx";

                    if (!File.Exists(tableFile)) return "";
                }

                tableFile = "\"" + tableFile + "\"";

                GetTableInfo(tableFile, tableName, sysType, xmlElement);

                if (xmlElement == "Rom")
                {
                    returnString = ParseGameData();
                }
                else if (xmlElement == "Author")
                {
                    returnString = ParseForAuthor();
                }
            }
            catch (Exception)
            {
                
            }
            

            return returnString;

        }

        public string GetInfoFuturePinball(int sysType, string tableName, string tablePath)
        {
            var returnstring = "";

            var tableFile = tablePath + "\\" + tableName + ".fpt";

            if (!File.Exists(tableFile)) return returnstring;

            tableFile = "\"" + tableFile + "\"";

            GetTableInfo(tableFile, tableName, sysType, "Author");

            return ParseForAuthorFuturePinball();
        }

        private string ParseGameData()
        {
            var romName = "";

            if (File.Exists("GameData"))
            {
                var file = new StreamReader("GameData", Encoding.UTF8);

                var lines = file.ReadLine();                

                if (lines != null || lines != string.Empty)
                {
                    file.Close();
                    file = new StreamReader("GameData", Encoding.UTF8);
                    lines = file.ReadToEnd();

                    var myRegex = new Regex(@"cGameName *= *""(\w+)""");
                    romName = matchedRom(lines, myRegex);

                    if (romName == string.Empty)
                    {
                        myRegex = new Regex(@"Const RomSet1 = ""(\w+)""");
                        romName = matchedRom(lines, myRegex);
                    }
                    else if (romName == string.Empty)
                    {
                        myRegex = new Regex(@"Const Romset1.*=.*""(\w+)""");
                        romName = matchedRom(lines, myRegex);                        
                    }
                    else if (romName == string.Empty)
                    {
                        myRegex = new Regex(@"Const RS1.*=.*""(\w+)""");
                        romName = matchedRom(lines, myRegex);
                    }
                    else if (romName == string.Empty)
                    {
                        myRegex = new Regex(@"GameName=""(\w+)""");
                        romName = matchedRom(lines, myRegex);
                    }
                    else if (romName == string.Empty)
                    {
                        myRegex = new Regex(@"GameName = ""(\w+)""");
                        romName = matchedRom(lines, myRegex);
                    }
                    else if (romName == string.Empty)
                    {
                        myRegex = new Regex(@"Const cGameName.*= ""(\w+)""");
                        romName = matchedRom(lines, myRegex);
                    }

                    file.Close();

                }                
            }

            return romName;
        }

        private string ParseForAuthor()
        {
            var returnString = "";

            try
            {
                if (File.Exists("AuthorName"))
                {
                    var file = new StreamReader("AuthorName", Encoding.Unicode);
                    var line = file.ReadLine();
                    
                    if (line != null || line != string.Empty)
                    {
                        file.Close();
                        file = new StreamReader("AuthorName", Encoding.Unicode);
                        while ((line = file.ReadLine()) != null)
                        {
                            returnString = line;
                        }
                        file.Close();
                    }
                }
            }
            catch (Exception) { }

            return returnString;
        }

        private string ParseForAuthorFuturePinball()
        {
            var returnString = "";

            if (!File.Exists("Table Data")) return returnString;

            var file = new StreamReader("Table Data", Encoding.Default);
            var line = file.ReadToEnd();
            line.ToString();
            if (line != null || line != string.Empty)
            {
                file.Close();
                file = new StreamReader("Table Data");
                var myRegex = new Regex(@"0����-\\0\\0\\0(\w.*)");
                var AllMatches = myRegex.Matches(line);
                foreach (Match SomeMatch in AllMatches)
                {
                    returnString  = SomeMatch.Groups[1].Value;
                }
            }

            file.Close();

            return returnString;
        }

        private string matchedRom(string lines, Regex pattern )
        {
            var romname = "";

            MatchCollection allMatches = pattern.Matches(lines);
            foreach (Match someMatch in allMatches)
            {
                romname = someMatch.Groups[1].Value;
            }

            return romname;
        }

        /// <summary>
        /// Opens a VP table with the SevenZip class and pulls the info
        /// </summary>
        /// <param name="sys"></param>
        /// <param name="tablePath"></param>
        /// <param name="selectedTables"></param>
        //public static void GetInfoFromTableFile(int sysType, string tablePath)
        //{
        //    var p = string.Empty;

        //    if (sysType == 1)
        //        GetInfoVisualPinball(sysType, tablePath);
        //    else if (sysType == 2)
        //        GetInfoFuturePinball(sysType, tablePath);

        //    if (sysType == 1 || sysType == 2)
        //    {

        //        var itemTable = item as Table;
        //        var vpTable = itemTable.Name + ".vpt";
        //        var fpTable = itemTable.Name + ".fpt";
        //        if (sysType == 1)
        //        {
        //            p = Path.Combine(tablePath, vpTable);
        //            if (!File.Exists(p))
        //            {
        //                vpTable = itemTable.Name + ".vpx";
        //                p = Path.Combine(tablePath, vpTable);
        //            }
        //        }
        //        else if (sysType == 2)
        //            p = Path.Combine(tablePath, fpTable);


        //        p = "\"" + p + "\"";
        //        sevenZip.GetTableInfo(p, itemTable.Name, sysType, "Rom");
        //        string line = null;
        //        itemTable.Rom = string.Empty;

        //        if (sysType == 1)
        //        {
        //            //cGameName.*= "(\w+)"
        //            if (File.Exists("GameData"))
        //            {
        //                var file = new StreamReader("GameData", Encoding.UTF8);
        //                line = file.ReadLine();
        //                if (line != null || line != string.Empty)
        //                {
        //                    file.Close();
        //                    file = new StreamReader("GameData", Encoding.UTF8);
        //                    line = file.ReadToEnd();

        //                    //var myRegex = new Regex(@"Const cGameName.*= *""(\w+)""");
        //                    var myRegex = new Regex(@"cGameName *= *""(\w+)""");
        //                    //Const cGameName= *"(\w+)"

        //                    MatchCollection allMatches = myRegex.Matches(line);
        //                    foreach (Match someMatch in allMatches)
        //                    {
        //                        itemTable.Rom = someMatch.Groups[1].Value;
        //                    }

        //                    if (itemTable.Rom == string.Empty)
        //                    {
        //                        //  myRegex = new Regex(@"GameName.=""(\w+)""");
        //                        myRegex = new Regex(@"Const RomSet1 = ""(\w+)""");
        //                        allMatches = myRegex.Matches(line);
        //                        foreach (Match someMatch in allMatches)
        //                        {
        //                            itemTable.Rom = someMatch.Groups[1].Value;
        //                        }
        //                    }
        //                    if (itemTable.Rom == string.Empty)
        //                    {
        //                        //  myRegex = new Regex(@"GameName.=""(\w+)""");
        //                        myRegex = new Regex(@"Const Romset1.*=.*""(\w+)""");
        //                        allMatches = myRegex.Matches(line);
        //                        foreach (Match someMatch in allMatches)
        //                        {
        //                            itemTable.Rom = someMatch.Groups[1].Value;
        //                        }
        //                    }
        //                    if (itemTable.Rom == string.Empty)
        //                    {
        //                        //  myRegex = new Regex(@"GameName.=""(\w+)""");
        //                        myRegex = new Regex(@"Const RS1.*=.*""(\w+)""");
        //                        allMatches = myRegex.Matches(line);
        //                        foreach (Match someMatch in allMatches)
        //                        {
        //                            itemTable.Rom = someMatch.Groups[1].Value;
        //                        }
        //                    }


        //                    if (itemTable.Rom == string.Empty)
        //                    {
        //                        //  myRegex = new Regex(@"GameName.=""(\w+)""");
        //                        myRegex = new Regex(@"GameName=""(\w+)""");
        //                        allMatches = myRegex.Matches(line);
        //                        foreach (Match someMatch in allMatches)
        //                        {
        //                            itemTable.Rom = someMatch.Groups[1].Value;
        //                        }
        //                    }
        //                    if (itemTable.Rom == string.Empty)
        //                    {
        //                        //  myRegex = new Regex(@"GameName.=""(\w+)""");
        //                        myRegex = new Regex(@"GameName = ""(\w+)""");
        //                        allMatches = myRegex.Matches(line);
        //                        foreach (Match someMatch in allMatches)
        //                        {
        //                            itemTable.Rom = someMatch.Groups[1].Value;
        //                        }
        //                    }
        //                    if (itemTable.Rom == string.Empty)
        //                    {
        //                        //  myRegex = new Regex(@"GameName.=""(\w+)""");
        //                        myRegex = new Regex(@"Const cGameName.*= ""(\w+)""");
        //                        allMatches = myRegex.Matches(line);
        //                        foreach (Match someMatch in allMatches)
        //                        {
        //                            itemTable.Rom = someMatch.Groups[1].Value;
        //                        }
        //                    }


        //                    file.Close();
        //                }
        //            }
        //        }

        //    }


        //    //return selectedTables;
        //}


        public void LaunchVp(string tPath, string tName, string vpExe, string workingPath, bool editor = false, bool script = false)
        {
            var vp = Path.Combine(workingPath, vpExe);
            var table = Path.Combine(tPath, tName);

            if (!File.Exists(vp)) return;

            bool isVpx = IsTableVpx(table);
            //if (!File.Exists()) return;            

            Task.Run(() =>
            {

                if (script)
                {
                    _ahk.ExecFunction("LoadVP", workingPath, vpExe);
                    _ahk.ExecFunction("LoadTableToEditor", tPath, tName);
                    _ahk.ExecFunction("ActivateScript", Convert.ToInt32(isVpx).ToString());
                }
                else if (editor)
                {
                    _ahk.ExecFunction("LoadVP", workingPath, vpExe);
                    _ahk.ExecFunction("LoadTableToEditor", tPath, tName);
                }
                else
                {
                    _ahk.ExecFunction("PlayTable", vp, table, Convert.ToInt32(isVpx).ToString());
                }
            });
        }

        private bool IsTableVpx(string table)
        {
            if (File.Exists(table + ".vpx"))
                return true;

            return false;
        }

    }
}
