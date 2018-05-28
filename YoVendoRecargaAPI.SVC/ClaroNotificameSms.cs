using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteractuaMovil.ContactoSms.Api;
using YoVendoRecargaAPI.SVC.Utils;
using System.Configuration;

namespace YoVendoRecargaAPI.SVC
{
    public class ClaroNotificameSms
    {
        public InteractuaMovil.ContactoSms.Api.SmsApi sdk;
        public bool SendSMS(string pBody, string pTargetPhone)
        {
            bool result = false;

            try
            {
                // API Key
                string key = ConfigurationManager.AppSettings["keynotificame"].ToString();

                // API Secret
                string secret = ConfigurationManager.AppSettings["secretnotificame"].ToString();

                // API Url
                string url = "https://notificame.claro.com.sv/api/rest/"; /* ej: http://<url>/api/ */

                sdk = new SmsApi(key, secret, url);

                Random id = new Random();

                var smsResult = sdk.Messages.SendToContact(pTargetPhone, pBody, id.Next().ToString());

                if( smsResult.ErrorCode==200 ||smsResult.ErrorCode==50000 ||smsResult.ErrorCode==50001 || smsResult.ErrorCode==503)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
               
                               

                result = (!String.IsNullOrEmpty(smsResult.ToString())) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("SendSMS: " + ex.Message);
            }

            return result;
        }
    }
}
