using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Topup
{
    public class Values
    {
        //Event Viewer values
        public static int EventLogID = 102;
        public static string EventLogAppName = "YoVendoRecargaAPI";
        public static short EventLogCategoryError = 1;
        public static short EventLogCategory = 2;

        //Topup Codes

        public const string PackageTopupKey = "package_topup_key";
        public const string PackageTopupResult = "Package topup";
        public const int PackageTopupCode = 0;

        public const string GeneralError = "General error";
        public const int GeneralErrorCode = 1;

        public const string InvalidProduct = "Invalid product.";
        public const int InvalidProductCode = 2;

        public const string NoCreditLeft = "Out of credit";
        public const int NoCreditLeftCode = 3;

        public const string Ok = "Ok";
        public const int OkCode = 4;

        public const string Success = "Success";
        public const int SuccessCode = 5;

        //Azure Search
        public const string AzureSearchApiKey = "52A6B9802CA54E7FBDD78826F47496FF";

        //URLs
        //public const string GatsTopupUrl = "http://csncusgats.cloudapp.net:8070/api/topup/";

        public string TopupUrl()
        {
            return ConfigurationManager.AppSettings["TopupUrl"].ToString();
        }

        //Connection String (SQL Server Client)
        public string getConnectionString()
        {
            return ConfigurationManager.AppSettings["YoVendoSaldoBD"].ToString();
        }

    }
}
