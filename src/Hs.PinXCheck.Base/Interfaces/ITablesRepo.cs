using Hs.VirtualPin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinXCheck.Base.Interfaces
{
    public interface ITablesRepo
    {
        PinballXTables PinballXTableList { get; set; }

        MasterTables MasterTableList { get; set; }

        UnMatchedTables UnMatchedTableList { get; set; }        

        Task GetTablesFromXmlAsync(string pinballXDatabase, string tableFolder);

        void GetMasterTables();

        bool MatchDescriptionsAsync(PinballXTable table);

        bool MatchDescription(string desc);
    }
}
