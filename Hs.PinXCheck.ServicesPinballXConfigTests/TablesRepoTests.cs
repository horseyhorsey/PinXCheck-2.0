using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hs.PinXCheck.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinXCheck.Services.Tests
{
    [TestClass()]
    public class TablesRepoTests
    {
        [TestMethod()]
        public void GetTablesFromXmlAsync()
        {
            TablesRepo tableRepo = new TablesRepo();

            tableRepo.GetTablesFromXmlAsync(@"I:\PinballX\Databases\Visual Pinball\Visual Pinball.xml",
                @"I:\Emulators\Visual Pinball\tables");
        }
    }
}