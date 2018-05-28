using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.SVC.Utils;

namespace YoVendoRecargaAPI.SVC
{
    public class IPAddressClient
    {
        public String GetLocationByIpAddress(string pIpAddress)
        {
            string address = "";

            try
            {
                string requestURL = ConfigurationManager.AppSettings["IpLocationUrl"].ToString() + pIpAddress;

                WebRequest request = WebRequest.Create(requestURL);

                //Response
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                string responseStr = reader.ReadToEnd();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var jsonObject = serializer.Deserialize<Dictionary<string, string>>(responseStr);

                string city = jsonObject["city"];
                string region_name = jsonObject["region_name"];
                string country_name = jsonObject["country_name"];

                
                if (city != "" && region_name != "" && country_name != "")
                {
                    address = city + ", " + region_name + " - " + country_name;
                }

                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("GetLocationByIpAddress: " + ex.Message);
            }

            return address;
        }

        private StreamReader RequestLocation(string pUrl)
        {
            StreamReader reader = null;

            try
            {
                WebRequest request = WebRequest.Create(pUrl);

                //Response
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("RequestLocation: " + ex.Message);
            }

            return reader;
        }
    }
}
