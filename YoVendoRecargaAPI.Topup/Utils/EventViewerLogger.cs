using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Topup
{
    public class EventViewerLogger
    {
        public static void LogError(string pError)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("Topup " + Values.EventLogAppName + ": " + pError, EventLogEntryType.Error, Values.EventLogID, Values.EventLogCategoryError);
            }
        }

        public static void LogInformation(string pMessage)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("Topup " + Values.EventLogAppName + ": " + pMessage, EventLogEntryType.Information, Values.EventLogID, Values.EventLogCategory);
            }
        }
    }
}
