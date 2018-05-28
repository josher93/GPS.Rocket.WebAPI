using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YoVendoRecargaAPI.SVC
{
    public class ClaroClient
    {
        public string ActivatePin(int ConsumerID, string Msisdn, string Pin, ref string Error)
        {
            string result = "";
            
            try
            {
                WebServiceClaro.ActivarPinClient client = new WebServiceClaro.ActivarPinClient();

                client.Open();

                result = client.activarPinPromocional(Msisdn, Pin, "YoVendoRecarga");

                

                //result = "0";
                client.Close();
            }
            catch (Exception ex)
            {
                result = "-101";
                Error = ex.StackTrace.ToString();
            }

            return result; 
        }
    }
}
