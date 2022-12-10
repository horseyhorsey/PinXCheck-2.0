using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.DescriptionMatch.Helper;
using Hs.PinXCheck.Domain.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Hs.PinXCheck.DescriptionMatch.ViewModels
{
    public class DescriptionMatchViewModel : ViewModelBase
    {
        private ITablesRepo _tableRepo;
        private IEventAggregator _eventAggregator;

        private ICollectionView unMatchedTables;
        public ICollectionView UnMatchedTables
        {
            get { return unMatchedTables; }
            set { SetProperty(ref unMatchedTables, value); }
        }

        #region Commands
        public DelegateCommand MatchDescriptionsCommand { get; private set; }
        public DelegateCommand StopMatchingCommand { get; private set; }
        public DelegateCommand RenameCommand { get; private set; }

        #endregion

        #region Constants        
        const string patterns = @"\(.*\)";
        #endregion

        #region Properties
        private bool tableMatchDescription = true;
        public bool TableMatchDescription
        {
            get { return tableMatchDescription; }
            set { SetProperty(ref tableMatchDescription, value); }
        }

        private bool masterMatchDescription;
        public bool MasterMatchDescription
        {
            get { return masterMatchDescription; }
            set { SetProperty(ref masterMatchDescription, value); }
        }

        private bool removeParenthysis = true;
        public bool RemoveParenthysis
        {
            get { return removeParenthysis; }
            set { SetProperty(ref removeParenthysis, value); }
        }

        private bool removeParenthysisMaster = true;
        public bool RemoveParenthysisMaster
        {
            get { return removeParenthysisMaster; }
            set { SetProperty(ref removeParenthysisMaster, value); }
        }

        private bool matchYearGreaterThan = true;
        public bool MatchYearGreaterThan
        {
            get { return matchYearGreaterThan; }
            set { SetProperty(ref matchYearGreaterThan, value); }
        }

        private int matchDistance = 6;
        public int MatchDistance
        {
            get { return matchDistance; }
            set { SetProperty(ref matchDistance, value); }
        }

        private int yearValue;
        public int YearValue
        {
            get { return yearValue; }
            set { SetProperty(ref yearValue, value); }
        }

        private bool flagMatchEnabled;

        private bool renameEnabled;
        public bool RenameEnabled
        {
            get { return renameEnabled; }
            set { SetProperty(ref renameEnabled, value); }
        }

        #endregion

        public DescriptionMatchViewModel(ITablesRepo tableRepo, IEventAggregator ea)
        {
            _tableRepo = tableRepo;
            _eventAggregator = ea;

            try
            {
                UnMatchedTables = new ListCollectionView(_tableRepo.UnMatchedTableList);
            }
            catch (Exception) { }

            ea.GetEvent<DatabaseUpdated>().Subscribe(x =>
            {
                UnMatchedTables = new ListCollectionView(_tableRepo.UnMatchedTableList);
            });

            MatchDescriptionsCommand = new DelegateCommand(MatchDescriptions);

            RenameCommand = new DelegateCommand(RenameTables);
        }

        private void MatchDescriptions()
        {
            var g = MatchYearGreaterThan;
            var tableNameEdit = "";
            var tableDescriptionEdit = "";
            var masterNameEdit = "";
            var masterDescriptionEdit = "";
            var rgx = new Regex(patterns);
            var count = 0;
            var bestTableMatch = new PinballXTable();

            //clearMatchedDescriptions();

            foreach (UnMatchedTable table in UnMatchedTables)
            {
                var inputTableName = table.FileName;
                var inputTableDesc = table.Description;

                if (RemoveParenthysis)
                {
                    tableNameEdit = rgx.Replace(inputTableName, string.Empty);
                    tableDescriptionEdit = rgx.Replace(inputTableDesc, string.Empty);
                }

                if (MatchYearGreaterThan)
                {
                    if (table.Year > YearValue || table.Year == 0)
                        flagMatchEnabled = true;
                    else flagMatchEnabled = false;
                }
                else
                {
                    if (table.Year < YearValue || table.Year == 0)
                        flagMatchEnabled = true;
                    else flagMatchEnabled = false;
                }

                if (flagMatchEnabled)
                {
                    if (!table.MatchedName)
                    {
                        foreach (var masterTable in _tableRepo.MasterTableList)
                        {
                            if (RemoveParenthysisMaster)
                            {
                                masterNameEdit = rgx.Replace(masterTable.Name, string.Empty);
                                masterDescriptionEdit = rgx.Replace(masterTable.Description, string.Empty);
                            }

                            if (MatchYearGreaterThan)
                            {
                                flagMatchEnabled = masterTable.Year > YearValue;
                            }
                            else {
                                flagMatchEnabled = masterTable.Year < YearValue;
                            }

                            if (flagMatchEnabled)
                            {
                                string pattern;
                                //if (bw.CancellationPending)
                                //{
                                //    e.Cancel = true;
                                //    return;
                                //}

                                if (!TableMatchDescription)
                                {
                                    pattern = RemoveParenthysis ? tableNameEdit.ToUpper() : table.FileName.ToUpper();
                                }
                                else
                                {
                                    pattern = RemoveParenthysis
                                        ? tableDescriptionEdit.ToUpper()
                                        : table.Description.ToUpper();
                                }

                                var input = !MasterMatchDescription ? masterTable.Name.ToUpper() : masterTable.Description.ToUpper();

                                var d = new Distance();
                                var i = d.LD(input, pattern);

                                if (i <= MatchDistance)
                                {
                                    MatchDistance = i;
                                    bestTableMatch = masterTable;
                                    table.FlagRename = true;
                                    table.MatchedDescription = masterTable.Description;

                                    RenameEnabled = true;
                                }
                            }
                        }
                    }
                }
                count++;
                //li.Add(bestTableMatch);
                bestTableMatch = new PinballXTable();
                //var percentage = (Int32)Math.Round((double)(count * 100) / ScanCount);
                //bw.ReportProgress(percentage);

            }

            UnMatchedTables.Refresh();

            // Put the list into the background workers Result
            //e.Result = li;
        }

        private void clearMatchedDescriptions()
        {
            foreach (UnMatchedTable item in UnMatchedTables)
            {
                item.MatchedDescription = "";
            }
        }

        private void RenameTables()
        {
            try
            {
                var tablesToRemove = new List<UnMatchedTable>();

                foreach (UnMatchedTable table in UnMatchedTables)
                {
                    if (!string.IsNullOrEmpty(table.MatchedDescription)
                        && table.FlagRename)
                    {
                        var master = _tableRepo.MasterTableList.Where(x => x.Description == table.MatchedDescription).FirstOrDefault();

                        var tableToEdit = _tableRepo.PinballXTableList.Where(x => x.Name == table.FileName).FirstOrDefault();

                        tableToEdit.Description = master.Description;
                        tableToEdit.Genre = master.Genre;
                        tableToEdit.Manufacturer = master.Manufacturer;
                        tableToEdit.Year = master.Year;
                        tableToEdit.IsDescriptionMatched = true;
                        tableToEdit.Type = master.Type;

                        tablesToRemove.Add(table);
                    }

                }

                try
                {
                    if (tablesToRemove.Count > 0)
                    {
                        foreach (UnMatchedTable table in tablesToRemove)
                        {
                            _tableRepo.UnMatchedTableList.Remove(table);
                        }
                    }
                }
                catch (Exception e) { }

                UnMatchedTables.Refresh();
            }
            catch (Exception) { }

        }
    }
}
