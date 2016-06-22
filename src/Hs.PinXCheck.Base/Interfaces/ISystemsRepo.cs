using Hs.PinballX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.PinXCheck.Base.Interfaces
{
    public interface ISystemsRepo
    {
        Systems SystemsList { get; set; }

        void BuildSystemFromIni(string pinballXConfigFile);

        void UpdateFromIni(string pinballXConfigFile, string systemId);

        void SaveSystemToIni(string pinballXConfigFile, PinballXSystem systemId);

    }
}
