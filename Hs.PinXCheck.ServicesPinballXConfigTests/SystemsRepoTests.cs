using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hs.PinXCheck.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hs.PinXCheck.Services.Tests
{
    [TestClass()]
    public class SystemsRepoTests
    {
        [TestMethod()]
        public void BuildSystemFromIniTest()
        {
            var pinballXPath = @"I:\PinballX";

            Assert.IsTrue(Directory.Exists(pinballXPath));

            var pinballXConfig = pinballXPath + "\\Config\\PinballX.ini";

            Assert.IsTrue(File.Exists(pinballXConfig));

            var systemsRepo = new SystemsRepo();

            systemsRepo.BuildSystemFromIni(pinballXConfig);
        }
    }
}