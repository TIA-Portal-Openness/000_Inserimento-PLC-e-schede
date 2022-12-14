using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering;
using Siemens.Engineering.Hmi;

using Siemens.Engineering.SW.Tags;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;

namespace ConsoleApp_CreazioneHW
{
    class Program
    {
        static void Main(string[] args)
        {
            //Aggancio a progetto aperto
	    IList<TiaPortalProcess> ProcessiTIAAperti = TiaPortal.GetProcesses();
            TiaPortalProcess mioProcessoTIAPortal = ProcessiTIAAperti[0];
            TiaPortal mioTIAPortal = mioProcessoTIAPortal.Attach();
	    Project mioProgettoTIA = mioTIAPortal.Projects[0];
            Console.WriteLine("Progetto pronto!");

            // Inserimento PLC di Linea
            Device miaStazionePLCLinea = mioProgettoTIA.Devices.CreateWithItem("OrderNumber:6ES7 513-1FL02-0AB0/V2.9", "PLC_Linea", "PLC_Linea");            
            
	    //Modifica Indirizzo IP PLC
            DeviceItem mioPlcLinea = miaStazionePLCLinea.DeviceItems[1];
            DeviceItem miaInterfacciaPlcLinea = mioPlcLinea.DeviceItems[3];
            NetworkInterface mioDatiInterfaccePlcLinea = miaInterfacciaPlcLinea.GetService<NetworkInterface>();
            Node mioNodoX1_PlcLinea = mioDatiInterfaccePlcLinea.Nodes[0];
            mioNodoX1_PlcLinea.SetAttribute("Address", "192.168.0.10");
            Console.WriteLine("IP a PLC assegnato");

            //Aggiunta schede su rack centrale e assegnazione indirizzi su immagine di processo
            DeviceItem mioRackPlc = miaStazionePLCLinea.DeviceItems[0];
            DeviceItem miaSchedaIngressiStandard =mioRackPlc.PlugNew("OrderNumber:6ES7 521-1BH00-0AB0/V2.2", "IngressiStandard", 2);
            DeviceItem mioSottomoduloStandard= miaSchedaIngressiStandard.DeviceItems[0];
	    Address mioDatiIndirizzoSottomodulo = mioSottomoduloStandard.Addresses[0];
	    mioDatiIndirizzoSottomodulo.SetAttribute("StartAddress",0);

	    //Salva progetto
           mioProgettoTIA.Save();

        }
    }
}
