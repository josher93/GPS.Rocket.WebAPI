using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.SVC.Models;
using YoVendoRecargaAPI.SVC.Utils;

namespace YoVendoRecargaAPI.SVC
{
    public class OneSignalClient
    {
        #region YoVendoRecargaIOs
        public async Task<NotificationResultEN> CreateMassiveCampaignIOs(CampaignEN pCampaignContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "IOS";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_YVR_IOS);
                string url = string.Format("/api/v1/notifications");

                #region Target Filter
                OneSignalFilter filter = new OneSignalFilter();

                if (MassiveTestModeActive())
                {
                    filter.field = "tag";
                    filter.key = "userid";
                    filter.relation = "=";
                    filter.value = ConfigurationManager.AppSettings["OneSignal_SingleUserTest"].ToString();
                }
                else
                {
                    filter.field = "country";
                    filter.value = pCampaignContent.CountryISO2Code;
                }
                #endregion

                var attachment = new
                {
                    id = (pCampaignContent.ImageURL != null) ? pCampaignContent.ImageURL : null
                };

                var notificationContent = new
                {
                    app_id = Constants.APPID_ONESIGNAL_YVR_IOS,
                    headings = new { en = pCampaignContent.NotificationTitle },
                    contents = new { en = pCampaignContent.NotificationMessage },
                    content_available = true,
                    ios_badgeType = "Increase",
                    ios_badgeCount = 1,
                    category = "",
                    collapse_id = "",
                    ios_attachments = (pCampaignContent.ImageURL != null) ? attachment : null,
                    filters = new object[] { filter }
                };

                

                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("CreateMassiveCampaign: " + ex.Message);
            }

            return operationResult;
        }

        public async Task<NotificationResultEN> PushDepositNotificationIOs(CampaignEN pCampaign, string pUserEmail, string pFirstName, string pContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "IOS";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_YVR_IOS);
                string url = string.Format("/api/v1/notifications");


                OneSignalFilter filter = new OneSignalFilter();
                filter.field = "tag";
                filter.key = "userid";
                filter.relation = "=";
                filter.value = (!SingleTestModeActive()) ? pUserEmail : ConfigurationManager.AppSettings["OneSignal_SingleUserTest"].ToString();

              
                var notificationContent = new
                {
                    app_id = Constants.APPID_ONESIGNAL_YVR_IOS,
                    headings = new { en = pCampaign.NotificationTitle },
                    contents = new { en = String.Format("Hola {0}. {1}", pFirstName, pContent) },
                    content_available = true,
                    ios_badgeType = "Increase",
                    ios_badgeCount = 1,
                    category = "",
                    collapse_id = "",
                    filters = new object[] { filter }
                };

                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null && notificationResult.recipients > 0)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("PushDepositNotification: " + ex.Message);
            }

            return operationResult;
        }

        public async Task<NotificationResultEN> PushTopupRequestIOs(string pPersonEmail, string pNickname, string pRequester, string pAmount, string pFirstName)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "IOS";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_YVR_IOS);
                string url = string.Format("/api/v1/notifications");

                var filter = new
                {
                    field = "tag",
                    key = "userid",
                    relation = "=",
                    value = pPersonEmail

                };
                var notificationContent = new
                {
                    app_id = Constants.APPID_ONESIGNAL_YVR_IOS,
                    headings = new { en = "¡Nueva solicitud de recarga!" },
                    contents = new { en = String.Format("Hola {0}, {1} te ha pedido una recarga de {2} al número {3}. Revisa el listado de solicitudes de recarga.", pFirstName, pNickname, pAmount, pRequester, pAmount) },
                    content_available = true,
                    ios_badgeType = "Increase",
                    ios_badgeCount = 1,
                    category = "",
                    collapse_id = "",
                    filters = new object[] { filter }
                };

                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null && notificationResult.recipients > 0)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("PushTopupRequest: " + ex.Message);
            }

            return operationResult;
        }

        #endregion

        #region YoVendoRecargaAndroid

        #region CreateMassiveCampgaignAndroid
                public async Task<NotificationResultEN> CreateMassiveCampaignAndroid(CampaignEN pCampaignContent)
                {
                    NotificationResultEN operationResult = new NotificationResultEN();
                    operationResult.Platform = "ANDROID";
                    operationResult.Result = false;
                    operationResult.ServiceName = "OneSignal";

                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(Constants.OneSignalURL);
                        client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_YVR_ANDROID);
                        string url = string.Format("/api/v1/notifications");

                        OneSignalFilter filter = new OneSignalFilter();

                        if (MassiveTestModeActive())
                        {
                            filter.field = "tag";
                            filter.key = "userid";
                            filter.relation = "=";
                            filter.value = ConfigurationManager.AppSettings["OneSignal_SingleUserTest_Android"].ToString();
                        }
                        else
                        {
                            filter.field = "country";
                            filter.value = pCampaignContent.CountryISO2Code;
                        }

                        var notificationContent = new
                        {
                            app_id = Constants.APPID_ONESIGNAL_YVR_ANDROID,
                            headings = new { en = pCampaignContent.NotificationTitle },
                            contents = new { en = pCampaignContent.NotificationMessage },
                            big_picture = (pCampaignContent.ImageURL != null) ? pCampaignContent.ImageURL : null,
                            category = "",
                            collapse_id = "",
                            filters = new object[] { filter }
                        };
                        var jsonPost = JsonConvert.SerializeObject(notificationContent);
                        StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(url, _content);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                            if (notificationResult.errors == null)
                            {
                                operationResult.CodeResult = "scheduled";
                                operationResult.Result = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                        EventViewerLoggerSVC.LogError("CreateMassiveCampaign: " + ex.Message);
                    }
                    return operationResult;
                }
        #endregion

        #region PushNotificacionesAndroid
        public async Task<NotificationResultEN> PushDepositNotificationAndroid(CampaignEN pCampaign, string pUserEmail, string pFirstName, string pContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_YVR_ANDROID);
                string url = string.Format("/api/v1/notifications");


                OneSignalFilter filter = new OneSignalFilter();
                filter.field = "tag";
                filter.key = "userid";
                filter.relation = "=";
                filter.value = (!SingleTestModeActive()) ? pUserEmail : ConfigurationManager.AppSettings["OneSignal_SingleUserTest_Android"].ToString();


                var notificationContent = new
                {
                    app_id = Constants.APPID_ONESIGNAL_YVR_ANDROID,
                    headings = new { en = pCampaign.NotificationTitle },
                    contents = new { en = String.Format("Hola {0}. {1}", pFirstName, pContent) },
                    category = "",
                    collapse_id = "",
                    filters = new object[] { filter }
                };

                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null && notificationResult.recipients > 0)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("PushDepositNotification: " + ex.Message);
            }

            return operationResult;
        }
        #endregion

        #region PushTopupRequestAndroid
        public async Task<NotificationResultEN> PushTopupRequestAndroid(string pPersonEmail, string pNickname, string pRequester, string pAmount, string pFirstName)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_YVR_ANDROID);
                string url = string.Format("/api/v1/notifications");

                var filter = new
                {
                    field = "tag",
                    key = "userid",
                    relation = "=",
                    value = pPersonEmail

                };
                var notificationContent = new
                {
                    app_id = Constants.APPID_ONESIGNAL_YVR_ANDROID,
                    headings = new { en = "¡Nueva solicitud de recarga!" },
                    contents = new { en = String.Format("Hola {0}, {1} te ha pedido una recarga de {2} al número {3}. Revisa el listado de solicitudes de recarga.", pFirstName, pNickname, pAmount, pRequester, pAmount) },
                    category = "",
                    collapse_id = "",
                    filters = new object[] { filter }
                };

                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null && notificationResult.recipients > 0)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("PushTopupRequest: " + ex.Message);
            }

            return operationResult;
        }
        #endregion


        #endregion

        #region RecarGO!  

        #region iOS

        public async Task<NotificationResultEN> CreateMassiveCampaignIOsRGO(CampaignEN pCampaignContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "IOS";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_RGO_IOS);
                string url = string.Format("/api/v1/notifications");

                #region Target Filter
                OneSignalFilter filter = new OneSignalFilter();

                if (RecarGOMassiveTestModeActive())
                {
                    filter.field = "tag";
                    filter.key = "userid";
                    filter.relation = "=";
                    filter.value = ConfigurationManager.AppSettings["OneSignal_SingleUserTest_RGO_IOS"].ToString();
                }
                else
                {
                    filter.field = "country";
                    filter.value = pCampaignContent.CountryISO2Code;
                }
                #endregion

                var attachment = new
                {
                    id = (pCampaignContent.ImageURL != null) ? pCampaignContent.ImageURL : null
                };

                var notificationContent = new
                {
                    app_id = Constants.APPID_ONESIGNAL_RGO_IOS,
                    headings = new { en = pCampaignContent.NotificationTitle },
                    contents = new { en = pCampaignContent.NotificationMessage },
                    content_available = true,
                    ios_badgeType = "Increase",
                    ios_badgeCount = 1,
                    category = "",
                    collapse_id = "",
                    ios_attachments = (pCampaignContent.ImageURL != null) ? attachment : null,
                    filters = new object[] { filter }
                };



                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("CreateMassiveCampaign: " + ex.Message);
            }

            return operationResult;
        }


        public NotificationResultEN PushSingleUserIos(string pTitle, string pMessage, string ImageURL, string pUserID)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "IOS";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            var request = WebRequest.Create(Constants.OneSignalNotifications) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_RGO_IOS);

            var serializer = new JavaScriptSerializer();

            var filter = new
            {
                field = "tag",
                key = "userid",
                relation = "=",
                value = pUserID

            };

            var attachment = new
            {
                id = (ImageURL != null) ? ImageURL : null
            };

            var notificationContent = new
            {
                app_id = Constants.APPID_ONESIGNAL_RGO_IOS,
                headings = new { en = pTitle },
                contents = new { en = pMessage },
                content_available = true,
                ios_badgeType = "Increase",
                ios_badgeCount = 1,
                category = "",
                collapse_id = "",
                ios_attachments = (ImageURL != null) ? attachment : null,
                filters = new object[] { filter }
            };

            var param = serializer.Serialize(notificationContent);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        var serializedResult = serializer.Deserialize<CreateResultOneSignal>(responseContent);

                        if (serializedResult.errors == null)
                        {
                            operationResult.Result = true;
                            operationResult.Message = "scheduled";
                        }

                    }
                }
            }
            catch (WebException ex)
            {
                operationResult.Result = false;
                operationResult.Message = "error";
                Console.WriteLine(ex.InnerException);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                EventViewerLoggerSVC.LogError("PushSingleUserIos: " + ex.Message);
            }

            return operationResult;
        }
        #endregion

        //------------------------ANDROID
        #region Android
        public NotificationResultEN PushSingleUserAndroid(string pTitle, string pMessage, string pImageURL, string pUserID)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            var request = WebRequest.Create(Constants.OneSignalNotifications) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_RGO_ANDROID);

            var serializer = new JavaScriptSerializer();
            var filter = new
            {
                field = "tag",
                key = "userid",
                relation = "=",
                value = pUserID

            };
            var notificationContent = new
            {
                app_id = Constants.APPID_ONESIGNAL_RGO_ANDROID,
                headings = new { en = pTitle },
                contents = new { en = pMessage },
                content_available = true,
                category = "",
                big_picture = (pImageURL != null) ? pImageURL : null,
                collapse_id = "",
                filters = new object[] { filter }
            };



            var param = serializer.Serialize(notificationContent);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        var serializedResult = serializer.Deserialize<CreateResultOneSignal>(responseContent);

                        if (serializedResult.errors == null)
                        {
                            operationResult.Result = true;
                            operationResult.CodeResult = "scheduled";
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                operationResult.Result = false;
                operationResult.CodeResult = "error";
                Console.WriteLine(ex.InnerException);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                EventViewerLoggerSVC.LogError("PushSingleUserAndroid: " + ex.Message);
            }

            return operationResult;
        }
        #endregion

        public async Task<NotificationResultEN> CreateMassiveCampaignAndroidRGO(CampaignEN pCampaignContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "OneSignal";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.OneSignalURL);
                client.DefaultRequestHeaders.Add("authorization", "Basic " + Constants.KEY_ONESIGNAL_RGO_ANDROID); //antes era OneSignalKey_Android_Yvr //TODO: REcomendacion: Cambiar nombre a RG!
                string url = string.Format("/api/v1/notifications");

                OneSignalFilter filter = new OneSignalFilter();

                if (RecarGOMassiveTestModeActive())
                {
                    filter.field = "tag";
                    filter.key = "userid";
                    filter.relation = "=";
                    filter.value = ConfigurationManager.AppSettings["OneSignal_SingleUserTest_RGO_Android"].ToString();

                }
                else
                {
                    filter.field = "country";
                    filter.value = pCampaignContent.CountryISO2Code;
                }

                var notificationContent = new
                {

                    app_id = Constants.APPID_ONESIGNAL_RGO_ANDROID,
                    headings = new { en = pCampaignContent.NotificationTitle },
                    contents = new { en = pCampaignContent.NotificationMessage },
                    big_picture = (pCampaignContent.ImageURL != null) ? pCampaignContent.ImageURL : null,
                    category = "",
                    collapse_id = "",
                   filters = new object[] { filter }

                };


                var jsonPost = JsonConvert.SerializeObject(notificationContent);
                StringContent _content = new StringContent(jsonPost, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, _content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var notificationResult = JsonConvert.DeserializeObject<CreateResultOneSignal>(result);

                    if (notificationResult.errors == null)
                    {
                        operationResult.CodeResult = "scheduled";
                        operationResult.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("CreateMassiveCampaign: " + ex.Message);
            }
            return operationResult;
        }

        private bool RecarGOMassiveTestModeActive()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["RGO_MassivePushTestMode"]);
        }

        

        //Validates Test Mode
        private bool MassiveTestModeActive()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["YVR_MassivePushTestMode"]); //TODO: implementar para RG! tambien
        }

        private bool SingleTestModeActive()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["YVR_SingleUserPushTestMode"]);
        }

        #endregion

    }
}
