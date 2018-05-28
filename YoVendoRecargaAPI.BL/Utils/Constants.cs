using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.BL
{
    public class Constants
    {
        //Event Viewer values
        public static int EventLogID = 102;
        public static string EventLogAppName = "YoVendoRecargaAPI";
        public static short EventLogCategoryError = 1;
        public static short EventLogCategory = 2;

        //TimeZones
        public static string TimeZoneCentralAmerica = "Central America Standard Time";
        public static string TimeZoneSAPacificStandardTime = "SA Pacific Standard Time";

        //Countries ID
        public const int ElSalvador = 69;
        public const int Guatemala = 93;
        public const int Nicaragua = 161;
        public const int Panama = 172;

        //Days Intervals
        public static int Yesterday = 1;
        public static int Week = 7;

        //Authentication
        public static int TokenLifetime = (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["TokenLifetime"].ToString())) ? int.Parse(ConfigurationManager.AppSettings["TokenLifetime"].ToString()) : 15;


        //Encryptation Keys
        public const string KeyBites1 = "8080808080808080";
        public const string KeyBites2 = "8080808080808089";
        public const string InitializationVector1 = "PasswordGP201707";
        public const string InitializationVector2 = "8080808080808090";
    }
}
