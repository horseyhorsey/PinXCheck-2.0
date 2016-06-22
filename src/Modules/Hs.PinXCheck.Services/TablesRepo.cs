using Hs.PinXCheck.Base.Interfaces;
using Hs.VirtualPin.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Hs.PinXCheck.Services
{
    public class TablesRepo : ITablesRepo
    {
        public PinballXTables PinballXTableList { get; set; }
        public MasterTables MasterTableList { get; set; }
        public UnMatchedTables UnMatchedTableList { get; set; }

        public void GetMasterTables()
        {

            System.Reflection.Assembly assembly =
                this.GetType().Assembly;
            
            Stream stream = assembly.GetManifestResourceStream(
                "Hs.PinXCheck.Services.Resource.IPDBBigList.txt");

            const string pattern = @"\|\|\|";

            using (var sr = new StreamReader(stream))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var lineArray = Regex.Split(line, pattern);
                    var s = lineArray[1].Replace("\"", string.Empty);
                    var s1 = s.Replace(":", " ");

                    MasterTableList.Add(new IpdbDatabase()
                    {
                        Id = Convert.ToInt32(lineArray[0]),
                        Name = lineArray[1],
                        Manufacturer = lineArray[2],
                        Type = lineArray[3],
                        Year = Convert.ToInt32(lineArray[4]),
                        Rating = ConvertRatings(float.Parse(lineArray[5])),
                        Genre = lineArray[6],
                        Players = Convert.ToByte(lineArray[7]),
                        Abbreviation = lineArray[8],
                        Units = Convert.ToInt32(lineArray[9]),
                        Description = lineArray[1] + " (" + lineArray[2] + " " + Convert.ToInt32(lineArray[4]) + ")"
                    });
                }

            }


            stream.Close();

        }

        public async Task GetTablesFromXmlAsync(string pinballXDatabase, string tableFolder)
        {
            PinballXTableList.Clear();
            UnMatchedTableList.Clear();
            try
            {                
                var xdoc = new XmlDocument();

                xdoc.Load(pinballXDatabase);

                var i = 0;
                foreach (XmlNode node in xdoc.SelectNodes("menu/game"))
                {
                    var table = new PinballXTable();

                    table = await GetTable(node, tableFolder);

                    table.IsDescriptionMatched = MatchDescriptionsAsync(table);

                    table.TableFileExists = GetTableFileName(tableFolder + "//" + table.Name);

                    PinballXTableList.Add(table);

                    i++;
                }

            }
            catch (Exception e) { }

        }

        private async Task<PinballXTable> GetTable(XmlNode node, string tableFolder)
        {
            var table = await Task.Run(async () =>
           {
               string rom = string.Empty, manu = string.Empty;
               var year = 2015;

               var name = node.SelectSingleNode("@name").InnerText;
               var desc = node.SelectSingleNode("description").InnerText;

               if (node.SelectSingleNode("rom") != null)
                   if (!string.IsNullOrEmpty(node.SelectSingleNode("rom").InnerText))
                       rom = node.SelectSingleNode("rom").InnerText;
                   else
                       rom = string.Empty;

               if (node.SelectSingleNode("manufacturer") != null)
                   manu = node.SelectSingleNode("manufacturer").InnerText;

               if (node.SelectSingleNode("year") != null)
                   if (!string.IsNullOrEmpty(node.SelectSingleNode("year").InnerText))
                       year = Int32.Parse(node.SelectSingleNode("year").InnerText);
                   else
                       year = 2015;

               string type;
               if (node.SelectSingleNode("type") != null)
                   type = node.SelectSingleNode("type").InnerText;
               else
                   type = "SS";

               string author;
               if (node.SelectSingleNode("author") != null)
                   author = node.SelectSingleNode("author").InnerText;
               else
                   author = "";

               string genre;
               if (node.SelectSingleNode("genre") != null)
                   genre = node.SelectSingleNode("genre").InnerText;
               else
                   genre = "";

               bool hidedmd;
               if (node.SelectSingleNode("hidedmd") != null)
                   hidedmd = Convert.ToBoolean(node.SelectSingleNode("hidedmd").InnerText);
               else
                   hidedmd = true;
               bool hidebackglass;
               if (node.SelectSingleNode("hidebackglass") != null)
                   hidebackglass = Convert.ToBoolean(node.SelectSingleNode("hidebackglass").InnerText);
               else
                   hidebackglass = true;
               bool enabled;
               if (node.SelectSingleNode("enabled") != null)
                   enabled = Convert.ToBoolean(node.SelectSingleNode("enabled").InnerText);
               else
                   enabled = true;

               int rating;
               if (node.SelectSingleNode("//rating") != null)
                   rating = Int32.Parse(node.SelectSingleNode("rating").InnerText);
               else
                   rating = 0;
                // Read from exe tag if the alternateExe isn't used.
                string altExe;
               if (node.SelectSingleNode("alternateExe") != null)
                   altExe = node.SelectSingleNode("alternateExe").InnerText;
               else if (node.SelectSingleNode("exe") != null)
                   altExe = node.SelectSingleNode("exe").InnerText;
               else
                   altExe = " ";

               bool desktop;
               if (node.SelectSingleNode("Desktop") != null)
                   desktop = Convert.ToBoolean(node.SelectSingleNode("Desktop").InnerText);
               else
                   desktop = false;


               return await CreateTable(name, desc, rom, manu, year, type, hidedmd, hidebackglass, altExe, enabled, rating, desktop, genre, author);

           });



            return table;
        }

        public async Task<PinballXTable> CreateTable(string name, string desc, string rom, string manu, int year,
            string type, bool hideDmd, bool hideBg, string altExe, bool enabled, int rating, bool desktop, string genre, string author)
        {
            return  await Task.Run(() => new PinballXTable()
            {
                Name = name,
                Description = desc,
                RomName = rom,
                Manufacturer = manu,
                Year = year,
                Type = type,
                HideDmd = hideDmd,
                HideBackGlass = hideBg,
                AlternateExe = altExe,
                Enabled = enabled,
                Rating = rating,
                Desktop = desktop,
                Genre = genre,
                Author = author
            });

        }

        public bool MatchDescriptionsAsync(PinballXTable table)
        {
            var matched = false;

            var value = MasterTableList.Where(item => item.Description.ToUpper() == table.Description.ToUpper()).FirstOrDefault();

            if (value != null)
            {
                matched = true;
            }
            else
            {
                UnMatchedTableList.Add(new UnMatchedTable()
                {
                    FileName = table.Name,
                    Description = table.Description,
                    Year = table.Year,
                    FlagRename = false,
                    MatchedDescription = "",
                    MatchedName = false
                });
            }

            return matched;
        }

        public bool GetTableFileName(string tablesPathName)
        {
            var exist = false;
            try
            {
                if (File.Exists(tablesPathName + ".vpt") || File.Exists(tablesPathName + ".vpx") || File.Exists(tablesPathName + ".fpt"))
                {
                    exist = true;
                }
                //exist = false;
            }
            catch (Exception) { exist = false; }

            return exist;

        }

        /// <summary>
        /// Convert the Pinball database ratings to 1,2,3,4,5
        /// </summary>
        /// <param name="ratings"></param>
        /// <returns></returns>
        public int ConvertRatings(float ratings)
        {
            var rating = 0;
            if (ratings >= 8.1)
                rating = 5;
            else if (ratings >= 7.4 && ratings < 8.1)
                rating = 4;
            else if (ratings >= 6.9 && ratings < 7.4)
                rating = 3;
            else if (ratings >= 6.5 && ratings < 6.9)
                rating = 2;
            else if (ratings >= 0 && ratings < 6.5)
                rating = 1;

            return rating;
        }

        public bool MatchDescription(string desc)
        {

            try
            {
                var value = MasterTableList.Where(item => item.Description.ToUpper() == desc.ToUpper()).ElementAt(0);

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

    }
}
