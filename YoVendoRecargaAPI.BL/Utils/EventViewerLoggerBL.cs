using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class EventViewerLoggerBL
    {
        public static void LogError(string pError)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("BL " + Constants.EventLogAppName + ": " + pError, EventLogEntryType.Error, Constants.EventLogID, Constants.EventLogCategoryError);
            }
        }

        public static void LogInformation(string pMessage)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("BL " + Constants.EventLogAppName + ": " + pMessage, EventLogEntryType.Information, Constants.EventLogID, Constants.EventLogCategory);
            }
        }
    }
}
