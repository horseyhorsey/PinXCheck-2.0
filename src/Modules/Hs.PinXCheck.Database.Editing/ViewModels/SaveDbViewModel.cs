using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Hs.PinXCheck.Domain.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Hs.PinXCheck.Database.Editing.ViewModels
{
    public class SaveDbViewModel : ViewModelBase
    {
        #region EventsCommands
        private IEventAggregator _eventAgg;
        public DelegateCommand SaveDatabaseCommand { get; private set; }
        #endregion

        #region Contructors
        public SaveDbViewModel(IEventAggregator ea, ITablesRepo tableRepo,
    ISelectedService selectedService, ISettingsRepo settings)
        {
            _eventAgg = ea;

            _tableRepo = tableRepo;

            _SelectedService = selectedService;

            _settingsRepo = settings;

            _eventAgg.GetEvent<DatabaseChanged>().Subscribe(x =>
            {
                try
                {
                    SelectedDatabase = Path.GetFileNameWithoutExtension(x);
                }
                catch (Exception) { }

            });

            SaveDatabaseCommand = new DelegateCommand(SaveDatabaseToXml);
        }

        #endregion

        #region Properties
        private bool writeExe;
        public bool WriteExe
        {
            get { return writeExe; }
            set { SetProperty(ref writeExe, value); }
        }

        private string selectedDatabase = "Visual Pinball";
        public string SelectedDatabase
        {
            get { return selectedDatabase; }
            set { SetProperty(ref selectedDatabase, value); }
        }
        #endregion

        #region Services
        private ITablesRepo _tableRepo;
        private ISelectedService _SelectedService;
        private ISettingsRepo _settingsRepo;
        #endregion

        #region Methods
        private void SaveDatabaseToXml()
        {
            if (string.IsNullOrEmpty(SelectedDatabase)) return;

            _eventAgg.GetEvent<ShowDialogEvent>().Publish("Saving Database,Ok");            

            try
            {
                var tableList = _tableRepo.PinballXTableList.ToList();

                tableList.Sort((x, y) => string.Compare(x.Description, y.Description));

                SerializeToXml(tableList, _SelectedService.CurrentSystem, SelectedDatabase, WriteExe);
            }
            catch (Exception e) { var msg = e.Message; }

        }

        /// <summary>
        /// Serialize table objects to XML
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="systemname"></param>
        /// <param name="dbName"></param>
        /// <param name="writeExeTag"></param>
        public void SerializeToXml(List<PinballXTable> tables, string systemname, string dbName, bool writeExeTag = false)
        {
            var ns = new XmlSerializerNamespaces();
            //  Add lib namespace with empty prefix
            ns.Add("", "");
            var root = new XmlRootAttribute("menu");
            //XmlSerializer serializer = new XmlSerializer(typeof(BindingList<Table>), root);
            var serializer = new XmlSerializer(typeof(List<PinballXTable>), root);
            var textWriter = new StreamWriter(_settingsRepo.PinXCheckSettings.PinballXPath + @"\Databases\" + systemname + "\\" + dbName + ".xml");
            serializer.Serialize(textWriter, tables, ns);
            textWriter.Close();

            if (writeExeTag)
                ReplaceToExeTags(_settingsRepo.PinXCheckSettings.PinballXPath + @"\Databases\" + systemname + "\\" + dbName + ".xml");
        }

        /// <summary>
        /// Search and replace nodes <alternateExe></alternateExe> and replace with <exe></exe>
        /// </summary>
        /// <param name="filePath"></param>
        private void ReplaceToExeTags(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("alternateExe"))
                {
                    lines[i] = lines[i].Replace("alternateExe", "exe");
                }
            }

            File.WriteAllLines(filePath, lines);
        }
        #endregion

    }
}
