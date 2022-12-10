using System;
using System.Linq;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Domain.Model;

namespace Hs.PinXCheck.Services
{
    public class SystemsRepo : ISystemsRepo
    {
        public Systems SystemsList { get; set; }

        /// <summary>
        /// Get all systems from a pinball X config.ini
        /// </summary>
        /// <param name="pinballXConfigFile"></param>
        public void BuildSystemFromIni(string pinballXConfigFile)
        {
            var iniRead = new IniFile.IniFileReader(pinballXConfigFile);

            SystemsList = new Systems();

            SystemsList.Add(new PinballXSystem()
            { Id = "VisualPinball", PinXType = 1,Name="Visual Pinball" });
            SystemsList.Add(new PinballXSystem()
            { Id = "FuturePinball", PinXType = 2, Name = "Future Pinball" });

            int i = 1;
            var customSystemId = "System_" + i;
            var flag = true;
            
            while (flag)
            {
                var systemName = iniRead.IniReadValue(customSystemId, "Name");

                if (string.IsNullOrEmpty(systemName))
                    flag = false;
                else
                {
                    SystemsList.Add(new PinballXSystem()
                    {
                        Id = customSystemId, Name = systemName
                    });

                    i++;

                    customSystemId = "System_" + i;
                }
            }
        }

        public void UpdateFromIni(string pinballXConfigFile, string systemId)
        {
            var iniFile = new IniFile.IniFileReader(pinballXConfigFile);

            var systemToEdit = SystemsList.Where(x => x.Id == systemId);
            
            var system = systemToEdit.FirstOrDefault();
            system.Enabled = Convert.ToBoolean(iniFile.IniReadValue(systemId, "Enabled"));
            system.PinXType = Convert.ToInt32(iniFile.IniReadValue(systemId, "SystemType"));

            system.WorkingPath = iniFile.IniReadValue(systemId, "WorkingPath");
            system.Executable = iniFile.IniReadValue(systemId, "Executable");
            system.Parameters = iniFile.IniReadValue(systemId, "Parameters");
            
            system.TablePath = iniFile.IniReadValue(systemId, "TablePath");                      
            system.Nvrampath = iniFile.IniReadValue(systemId, "NVRAMPath");                                    

            system.LaunchBefore = Convert.ToBoolean(iniFile.IniReadValue(systemId, "LaunchBeforeEnabled"));
            system.LaunchBeforeHideWindow = Convert.ToBoolean(iniFile.IniReadValue(systemId, "LaunchBeforeHideWindow"));
            system.LaunchBeforeWaitForExit = Convert.ToBoolean(iniFile.IniReadValue(systemId, "LaunchBeforeWaitForExit"));
            system.LaunchBeforePath = iniFile.IniReadValue(systemId, "LaunchBeforeWorkingPath");
            system.LaunchBeforeexe = iniFile.IniReadValue(systemId, "LaunchBeforeExecutable");
            system.LaunchBeforeParams = iniFile.IniReadValue(systemId, "LaunchBeforeParameters");

            system.LaunchAfter = Convert.ToBoolean(iniFile.IniReadValue(systemId, "LaunchAfterEnabled"));
            system.LaunchAfterHideWindow = Convert.ToBoolean(iniFile.IniReadValue(systemId, "LaunchAfterHideWindow"));
            system.LaunchAfterWaitForExit = Convert.ToBoolean(iniFile.IniReadValue(systemId, "LaunchAfterWaitForExit"));
            system.LaunchAfterWorkingPath = iniFile.IniReadValue(systemId, "LaunchAfterWorkingPath");
            system.LaunchAfterexe = iniFile.IniReadValue(systemId, "LaunchAfterExecutable");
            system.LaunchAfterParams = iniFile.IniReadValue(systemId, "LaunchAfterParameters");

        }

        public void SaveSystemToIni(string pinballXConfigFile, PinballXSystem pinballXSystem)
        {
            var iniFile = new IniFile.IniFileReader(pinballXConfigFile);

            var sysId = pinballXSystem.Id;

            iniFile.IniWriteValue(sysId, "Enabled", pinballXSystem.Enabled.ToString());
            iniFile.IniWriteValue(sysId, "WorkingPath", pinballXSystem.WorkingPath);
            iniFile.IniWriteValue(sysId, "Executable", pinballXSystem.Executable);
            iniFile.IniWriteValue(sysId, "Parameters", pinballXSystem.Parameters);

            iniFile.IniWriteValue(sysId, "TablePath", pinballXSystem.TablePath);

            iniFile.IniWriteValue(sysId, "LaunchBeforeEnabled", pinballXSystem.LaunchBefore.ToString());
            iniFile.IniWriteValue(sysId, "LaunchBeforeWorkingPath", pinballXSystem.LaunchBeforePath);
            iniFile.IniWriteValue(sysId, "LaunchBeforeExecutable", pinballXSystem.LaunchBeforeexe);
            iniFile.IniWriteValue(sysId, "LaunchBeforeParameters", pinballXSystem.LaunchBeforeParams);
            iniFile.IniWriteValue(sysId, "LaunchBeforeWaitForExit", pinballXSystem.LaunchBeforeWaitForExit.ToString());
            iniFile.IniWriteValue(sysId, "LaunchBeforeHideWindow", pinballXSystem.LaunchBeforeHideWindow.ToString());

            iniFile.IniWriteValue(sysId, "LaunchAfterEnabled", pinballXSystem.LaunchAfter.ToString());
            iniFile.IniWriteValue(sysId, "LaunchAfterWorkingPath", pinballXSystem.LaunchAfterWorkingPath);
            iniFile.IniWriteValue(sysId, "LaunchAfterExecutable", pinballXSystem.LaunchAfterexe);
            iniFile.IniWriteValue(sysId, "LaunchAfterParameters", pinballXSystem.LaunchAfterParams);
            iniFile.IniWriteValue(sysId, "LaunchAfterWaitForExit", pinballXSystem.LaunchAfterWaitForExit.ToString());
            iniFile.IniWriteValue(sysId, "LaunchAfterHideWindow", pinballXSystem.LaunchAfterHideWindow.ToString());

            if ((int)pinballXSystem.PinXType == 1)
            {
                iniFile.IniWriteValue(sysId, "NVRAMPath", pinballXSystem.Nvrampath);
                iniFile.IniWriteValue(sysId, "Bypass", "True");
            }

            if ((int)pinballXSystem.PinXType == 2)
            {
                iniFile.IniWriteValue(sysId, "FPRAMPath", pinballXSystem.Nvrampath);
                iniFile.IniWriteValue(sysId, "MouseClickFocus", "True");
            }
            
            iniFile.IniWriteValue(sysId, "SystemType", ((int)pinballXSystem.PinXType).ToString());
            
        }


        //private static void SetSystemsFromIni()
        //{
        //    try
        //    {
        //        _pinxsystem = new List<PinXSystem>();
        //        var iniRead = new IniFileReader(ConfigPath);
        //        var id = "VisualPinball";
        //        bool enabled;
        //        bool.TryParse(iniRead.IniReadValue("VisualPinball", "Enabled"), out enabled);
        //        var sysName = "Visual Pinball";
        //        var workpath = iniRead.IniReadValue("VisualPinball", "WorkingPath");
        //        var tablepath = iniRead.IniReadValue("VisualPinball", "TablePath");
        //        var parameters = iniRead.IniReadValue("VisualPinball", "Parameters");
        //        var exe = iniRead.IniReadValue("VisualPinball", "Executable");
        //        var nvram = iniRead.IniReadValue("VisualPinball", "NVRAMPath");

        //        var lbEnabled = Convert.ToBoolean(iniRead.IniReadValue("VisualPinball", "LaunchBeforeEnabled"));
        //        var lbPath = iniRead.IniReadValue("VisualPinball", "LaunchBeforeWorkingPath");
        //        var lbExe = iniRead.IniReadValue("VisualPinball", "LaunchBeforeExecutable");
        //        var lbParams = iniRead.IniReadValue("VisualPinball", "LaunchBeforeParameters");
        //        var lbWaitExit = Convert.ToBoolean(iniRead.IniReadValue("VisualPinball", "LaunchBeforeWaitForExit"));
        //        var lbHideWindow = Convert.ToBoolean(iniRead.IniReadValue("VisualPinball", "LaunchBeforeHideWindow"));

        //        var laEnabled = Convert.ToBoolean(iniRead.IniReadValue("VisualPinball", "LaunchAfterEnabled"));
        //        var laPath = iniRead.IniReadValue("VisualPinball", "LaunchAfterWorkingPath");
        //        var laExe = iniRead.IniReadValue("VisualPinball", "LaunchAfterExecutable");
        //        var laHideWindow = Convert.ToBoolean(iniRead.IniReadValue("VisualPinball", "LaunchAfterHideWindow"));
        //        var laParams = iniRead.IniReadValue("VisualPinball", "LaunchAfterParameters");
        //        var laWaitExit = Convert.ToBoolean(iniRead.IniReadValue("VisualPinball", "LaunchAfterWaitForExit"));

        //        _pinxsystem.Add(new PinXSystem(1, enabled, id, sysName, workpath, tablepath, exe, parameters, lbEnabled, lbPath, lbExe, lbParams,
        //            lbWaitExit, lbHideWindow, laEnabled, laWaitExit, laExe, laParams, laHideWindow, laPath, nvram));

        //        id = "FuturePinball";
        //        bool.TryParse(iniRead.IniReadValue("FuturePinball", "Enabled"), out enabled);
        //        sysName = "Future Pinball";
        //        workpath = iniRead.IniReadValue("FuturePinball", "WorkingPath");
        //        tablepath = iniRead.IniReadValue("FuturePinball", "TablePath");
        //        parameters = iniRead.IniReadValue("FuturePinball", "Parameters");
        //        exe = iniRead.IniReadValue("FuturePinball", "Executable");
        //        nvram = iniRead.IniReadValue("FuturePinball", "NVRAMPath");

        //        lbEnabled = Convert.ToBoolean(iniRead.IniReadValue("FuturePinball", "LaunchBeforeEnabled"));
        //        lbPath = iniRead.IniReadValue("FuturePinball", "LaunchBeforeWorkingPath");
        //        lbExe = iniRead.IniReadValue("FuturePinball", "LaunchBeforeExecutable");
        //        lbParams = iniRead.IniReadValue("FuturePinball", "LaunchBeforeParameters");
        //        lbWaitExit = Convert.ToBoolean(iniRead.IniReadValue("FuturePinball", "LaunchBeforeWaitForExit"));
        //        lbHideWindow = Convert.ToBoolean(iniRead.IniReadValue("FuturePinball", "LaunchBeforeHideWindow"));

        //        laEnabled = Convert.ToBoolean(iniRead.IniReadValue("FuturePinball", "LaunchAfterEnabled"));
        //        laPath = iniRead.IniReadValue("FuturePinball", "LaunchAfterWorkingPath");
        //        laExe = iniRead.IniReadValue("FuturePinball", "LaunchAfterExecutable");
        //        laHideWindow = Convert.ToBoolean(iniRead.IniReadValue("FuturePinball", "LaunchAfterHideWindow"));
        //        laParams = iniRead.IniReadValue("FuturePinball", "LaunchAfterParameters");
        //        laWaitExit = Convert.ToBoolean(iniRead.IniReadValue("FuturePinball", "LaunchAfterWaitForExit"));

        //        _pinxsystem.Add(new PinXSystem(2, enabled, id, sysName, workpath, tablepath, exe, parameters, lbEnabled, lbPath, lbExe, lbParams,
        //            lbWaitExit, lbHideWindow, laEnabled, laWaitExit, laExe, laParams, laHideWindow, laPath, nvram));

        //        var i = 1;

        //        while (sysName != string.Empty)
        //        {
        //            id = "System_" + i;
        //            sysName = iniRead.IniReadValue(id, "Name");
        //            workpath = iniRead.IniReadValue("System_" + i, "WorkingPath");
        //            tablepath = iniRead.IniReadValue("System_" + i, "TablePath");
        //            parameters = iniRead.IniReadValue("System_" + i, "Parameters");
        //            exe = iniRead.IniReadValue("System_" + i, "Executable");
        //            nvram = iniRead.IniReadValue("System_" + i, "NVRAMPath");

        //            int systemType;
        //            int.TryParse(iniRead.IniReadValue("System_" + i, "SystemType"), out systemType);

        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "Enabled"), out enabled);
        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "LaunchBeforeEnabled"), out lbEnabled);
        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "LaunchBeforeWaitForExit"), out lbWaitExit);
        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "LaunchBeforeHideWindow"), out lbHideWindow);

        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "LaunchAfterEnabled"), out laEnabled);
        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "LaunchAfterHideWindow"), out laHideWindow);
        //            bool.TryParse(iniRead.IniReadValue("System_" + i, "LaunchAfterWaitForExit"), out laWaitExit);

        //            lbPath = iniRead.IniReadValue("System_" + i, "LaunchBeforeWorkingPath");
        //            lbExe = iniRead.IniReadValue("System_" + i, "LaunchBeforeExecutable");
        //            lbParams = iniRead.IniReadValue("System_" + i, "LaunchBeforeParameters");
        //            laPath = iniRead.IniReadValue("System_" + i, "LaunchAfterWorkingPath");
        //            laExe = iniRead.IniReadValue("System_" + i, "LaunchAfterExecutable");
        //            laParams = iniRead.IniReadValue("System_" + i, "LaunchAfterParameters");

        //            if (systemType == 1 || systemType == 2)
        //            {
        //                nvram = iniRead.IniReadValue("System_" + i, "NVRAMPath");
        //            }

        //            if (sysName != string.Empty)
        //            {
        //                _pinxsystem.Add(new PinXSystem(systemType, enabled, id, sysName, workpath, tablepath, exe, parameters, lbEnabled, lbPath, lbExe, lbParams,
        //                    lbWaitExit, lbHideWindow, laEnabled, laWaitExit, laExe, laParams, laHideWindow, laPath, nvram));
        //                i++;
        //            }
        //            else
        //                return;
        //        }
        //    }
        //    catch (Exception exception)
        //    {

        //        Trace.WriteLine(DateTime.Now.ToString() + " ERROR:" + exception.Message);
        //    }
        //}
    }
}
