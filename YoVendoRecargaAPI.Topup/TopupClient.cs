using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using YoVendoRecargaAPI.Topup.Models;
using YoVendoRecargaAPI.Entities;
using System.Configuration;

namespace YoVendoRecargaAPI.Topup
{
    public class TopupClient
    {
        Values values = new Values();

        public TopupTransactionEN AttemptSendTopup(string pPhone, decimal pAmount, int pPackCode, int MNOId, int CategoryId)
        {
            TopupTransactionEN topupResult = new TopupTransactionEN();
            string amount;
            string topupUrl;
            string textResult;

            amount = String.Format("{0:0.##}", pAmount);
            //topupUrl = values.TopupUrl() + pPhone + "/" + amount + "/" + pPackCode;
            topupUrl = values.TopupUrl() + pPhone + "/" + amount + "/" + MNOId + "/" + CategoryId;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(topupUrl);
                request.Timeout = Int32.Parse(ConfigurationManager.AppSettings["SocketTimeout"].ToString());

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                EventViewerLogger.LogInformation("SendTopup GATS Response: " + response.ToString());

                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);

                textResult = streamReader.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Deserialize<ApiResponse>(textResult);

                topupResult.Code = serializedResult.resultCode;
                topupResult.ServiceTransactionID = serializedResult.serviceTransactionId;
                topupResult.Message = serializedResult.errorMessage;
                topupResult.RequestURL = topupUrl;
                topupResult.Response = textResult;

                if (serializedResult.ExtraParameters == null)
                {
                    topupResult.Msisdn = pPhone;
                    topupResult.Mno = "";
                }
                else
                {
                    topupResult.Msisdn = pPhone;
                    topupResult.Mno = serializedResult.ExtraParameters[0].value;
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    textResult = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    var serializer = new JavaScriptSerializer();

                    var serializedResult = serializer.Deserialize<ApiResponse>(textResult);
                    topupResult.Code = serializedResult.resultCode;
                    topupResult.ServiceTransactionID = serializedResult.serviceTransactionId;
                    topupResult.Message = serializedResult.errorMessage;
                    topupResult.RequestURL = topupUrl;
                    topupResult.Response = textResult;

                    EventViewerLogger.LogError("SendTopup WebException: " + ex.ToString());
                    EventViewerLogger.LogInformation("SendTopup serializedResult: " + serializedResult.errorMessage);
                }
                else
                {
                    topupResult.Code = "NoServerResponse";
                    topupResult.Message = "";
                    topupResult.RequestURL = topupUrl;
                    topupResult.Response = "";
                }
            }
            catch (Exception ex)
            {
                topupResult.Code = "Error";
                topupResult.Message = ex.Message;
                topupResult.RequestURL = topupUrl;
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AttemptSendTopup: " + ex.Message);
            }

            return topupResult;
        }
    }
}