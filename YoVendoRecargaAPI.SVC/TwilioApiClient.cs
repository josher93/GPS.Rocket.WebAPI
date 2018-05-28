using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using YoVendoRecargaAPI.SVC.Utils;

namespace YoVendoRecargaAPI.SVC
{
    public class TwilioApiClient
    {
        public bool SendSMS(string pBody, string pTargetPhone)
        {
            bool result = false;

            try
            {
                TwilioClient.Init(Constants.TwilioAccountID, Constants.TwilioAuthToken);

                var to = new PhoneNumber(pTargetPhone);
                var message = MessageResource.Create(
                    to,
                    from: new PhoneNumber("+1(305) 697-1696"),
                    body: pBody);

                var smsResult = message.Sid;

                result = (!String.IsNullOrEmpty(smsResult)) ? true : false;
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
