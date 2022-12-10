using Hs.PinXCheck.Domain.Model;

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
