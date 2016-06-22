using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinballX
{
    public class PinballXSystem
    {        
        public bool Enabled { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string WorkingPath { get; set; }
        public string TablePath { get; set; }
        public string Executable { get; set; }
        public string Parameters { get; set; }
        public bool LaunchBefore { get; set; }
        public string LaunchBeforePath { get; set; }
        public string LaunchBeforeexe { get; set; }
        public string LaunchBeforeParams { get; set; }
        public bool LaunchBeforeWaitForExit { get; set; }
        public bool LaunchBeforeHideWindow { get; set; }
        public bool LaunchAfter { get; set; }
        public string LaunchAfterWorkingPath { get; set; }
        public string LaunchAfterexe { get; set; }
        public string LaunchAfterParams { get; set; }
        public bool LaunchAfterHideWindow { get; set; }
        public bool LaunchAfterWaitForExit { get; set; }
        public string Nvrampath { get; set; }
        public int PinXType { get; set; }
    }
}
