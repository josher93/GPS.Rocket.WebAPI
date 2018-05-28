using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YoVendoRecargaAPI.DAL
{
    public class Constants
    {
        //Event Viewer values
        public static int EventLogID = 102;
        public static string EventLogAppName = "YoVendoRecargaAPI";
        public static short EventLogCategoryError = 1;
        public static short EventLogCategory = 2;

        public Constants()
        {
            
        }

        public string getConnectionString()
        {
            return ConfigurationManager.AppSettings["YoVendoSaldoBD"].ToString();
        }

    }
}
