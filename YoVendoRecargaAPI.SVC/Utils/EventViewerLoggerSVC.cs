using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.SVC.Utils
{
    public class EventViewerLoggerSVC
    {
        public static void LogError(string pError)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("SVC " + Constants.EventLogAppName + ": " + pError, EventLogEntryType.Error, Constants.EventLogID, Constants.EventLogCategoryError);
            }
        }

        public static void LogInformation(string pMessage)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("SVC " + Constants.EventLogAppName + ": " + pMessage, EventLogEntryType.Information, Constants.EventLogID, Constants.EventLogCategory);
            }
        }
    }
}
