using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.SVC.Models;
using YoVendoRecargaAPI.SVC.Utils;

namespace YoVendoRecargaAPI.SVC
{
    public class CampaignHandler
    {
        //MobileEngagementClient engagementClient = new MobileEngagementClient();
        OneSignalClient onesignalClient = new OneSignalClient();

        public async Task<List<NotificationResultEN>> HandleContent(CampaignEN pCampaign)
        {
            List<NotificationResultEN> operationResults = new List<NotificationResultEN>();

            try
            {
                var androidOperation = await onesignalClient.CreateMassiveCampaignAndroid(pCampaign);
                var iosOperation = await onesignalClient.CreateMassiveCampaignIOs(pCampaign);
                //var engagementOperation = await engagementClient.CreateMassiveCampaign(pCampaign);

                operationResults.Add(androidOperation);
                operationResults.Add(iosOperation);
                //operationResults.Add(engagementOperation);
            }
            catch (Exception ex)
            {
                operationResults = null;
                Console.WriteLine(ex.InnerException);
            }

            return operationResults;
        }

        public async Task<List<NotificationResultEN>> HandleDepositContent(CampaignEN pCampaign, string pUserEmail, string pFirstName, string pContent)
        {
            List<NotificationResultEN> operationResults = new List<NotificationResultEN>();
            try
            {
                var androidOperation = await onesignalClient.PushDepositNotificationAndroid(pCampaign, pUserEmail, pFirstName, pContent);
                var iosOperation = await onesignalClient.PushDepositNotificationIOs(pCampaign, pUserEmail, pFirstName, pContent);
                //var engagementOperation = await engagementClient.PushDepositNotification(pCampaign, pUserEmail, pFirstName, pContent);

                operationResults.Add(androidOperation);
                operationResults.Add(iosOperation);
                //operationResults.Add(engagementOperation);
            }
            catch (Exception ex)
            {
                operationResults = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("HandleDepositContent: " + ex.Message);
            }

            return operationResults;
        }

        public async Task<List<NotificationResultEN>> HandleTopupRequestContent(string pPersonEmail, string pNickname, string pRequester, string pAmount, string pUserFirstname)
        {
            List<NotificationResultEN> operationResults = new List<NotificationResultEN>();
            try
            {
                var androidOperation = await onesignalClient.PushTopupRequestAndroid(pPersonEmail, pNickname, pRequester, pAmount, pUserFirstname);
                var iosOperation = await onesignalClient.PushTopupRequestIOs(pPersonEmail, pNickname, pRequester, pAmount, pUserFirstname);
                //var engagementOperation = await engagementClient.PushTopupRequest(pPersonEmail, pRequester, pAmount);

                operationResults.Add(androidOperation);
                operationResults.Add(iosOperation);
                //operationResults.Add(engagementOperation);
            }
            catch (Exception ex)
            {
                operationResults = null;
                Console.WriteLine(ex.InnerException);
            }

            return operationResults;
        }

        public List<NotificationResultEN> HandleTopupDeniedContent(string pTitle, string pMessage, string pImageURL, string pConsumerPhone)
        {
            List<NotificationResultEN> operationResults = new List<NotificationResultEN>();
            try
            {
                var androidOperation = onesignalClient.PushSingleUserAndroid(pTitle, pMessage, pImageURL, pConsumerPhone);
                var iosOperation = onesignalClient.PushSingleUserIos(pTitle, pMessage, pImageURL, pConsumerPhone);

                operationResults.Add(androidOperation);
                operationResults.Add(iosOperation);
            }
            catch (Exception ex)
            {
                operationResults = null;
                Console.WriteLine(ex.InnerException);
            }

            return operationResults;
        }

        public List<NotificationResultEN> HandleEarnedCoinContent(string pTitle, string pMessage, string pImageURL, string pConsumerPhone)
        {
            List<NotificationResultEN> operaionResults = new List<NotificationResultEN>();

            try
            {
                var anroidOperation = onesignalClient.PushSingleUserAndroid(pTitle, pMessage, pImageURL, pConsumerPhone);
                var iosOperation = onesignalClient.PushSingleUserIos(pTitle, pMessage, pImageURL, pConsumerPhone);

                operaionResults.Add(anroidOperation);
                operaionResults.Add(iosOperation);

            }
            catch (Exception ex)
            {
                operaionResults = null;
                Console.WriteLine(ex.InnerException);
            }

            return operaionResults;
        }

        #region RGO
        public async Task<List<NotificationResultEN>> HandleRGOPushContent(CampaignEN pCampaign)
        {
            List<NotificationResultEN> operationResults = new List<NotificationResultEN>();

            try
            {
                if(String.IsNullOrEmpty(pCampaign.SpecificUser))
                {
                    var androidOperation = await onesignalClient.CreateMassiveCampaignAndroidRGO(pCampaign);
                    var iosOperation = await onesignalClient.CreateMassiveCampaignIOsRGO(pCampaign);

                    operationResults.Add(androidOperation);
                    operationResults.Add(iosOperation);
                }
                else
                {
                    string Title = pCampaign.Title;
                    string Message = pCampaign.NotificationMessage;
                    string UserID = pCampaign.SpecificUser;
                    string pImageURL = pCampaign.ImageURL;

                    var androidOperation = onesignalClient.PushSingleUserAndroid(Title, Message, pImageURL, UserID);
                    var iosOperation = onesignalClient.PushSingleUserIos(Title, Message, pImageURL, UserID);

                    operationResults.Add(androidOperation);
                    operationResults.Add(iosOperation);
                }

            }
            catch (Exception ex)
            {
                operationResults = null;
                Console.WriteLine(ex.InnerException);
            }

            return operationResults;
        }

        #endregion
    }
}
