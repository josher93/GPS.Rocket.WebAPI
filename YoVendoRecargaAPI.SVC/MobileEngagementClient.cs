using Microsoft.Azure.Management.Engagement;
using Microsoft.Azure.Management.Engagement.Models;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.SVC.Models;
using YoVendoRecargaAPI.SVC.Utils;

namespace YoVendoRecargaAPI.SVC
{
    public class MobileEngagementClient
    {
        static EngagementManagementClient engagementClient;

        public async Task<NotificationResultEN> CreateMassiveCampaign(CampaignEN pCampaignContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "Engagement";

            int engagementCampaignID = 0;

            try
            {
                var credentials = await ApplicationTokenProvider.LoginSilentAsync(Constants.TENANT_ID, Constants.CLIENT_ID, Constants.CLIENT_SECRET);
                engagementClient = new EngagementManagementClient(credentials)
                {
                    SubscriptionId = Constants.SUBSCRIPTION_ID
                };

                engagementClient.ResourceGroupName = Constants.RESOURCE_GROUP;
                engagementClient.AppCollection = Constants.APP_COLLECTION_NAME;
                engagementClient.AppName = Constants.APP_RESOURCE_NAME_ANDROID;

                #region Campaign Audience
                //Campaign audience
                CampaignAudience audience = new CampaignAudience();

                if (YVRMassiveTestModeActive())
                {
                    string userTest = ConfigurationManager.AppSettings["Engagement_SingleUserTest"].ToString();

                    Dictionary<string, Criterion> stringTagCriteria = new Dictionary<string, Criterion>();
                    stringTagCriteria.Add("StringTag", new StringTagCriterion("userid", userTest));
                    audience.Expression = "StringTag";
                    audience.Criteria = stringTagCriteria;
                }
                else
                {
                    Dictionary<string, Criterion> locationCriteria = new Dictionary<string, Criterion>();
                    locationCriteria.Add("CarrierCountry", new CarrierCountryCriterion(pCampaignContent.CountryISO2Code));
                    audience.Expression = "CarrierCountry";
                    audience.Criteria = locationCriteria;
                }
                #endregion

                NotificationOptions notificationOptions = new NotificationOptions();
                notificationOptions.BigPicture = (pCampaignContent.ImageURL != null) ? pCampaignContent.ImageURL : null;


                Campaign campaigne = new Campaign(
                    name: pCampaignContent.Title,
                    notificationTitle: pCampaignContent.NotificationTitle,
                    notificationMessage: pCampaignContent.NotificationMessage,
                    type: "text/plain",
                    deliveryTime: "any",
                    notificationType: "system",
                    pushMode: "one-shot",
                    title: pCampaignContent.NotificationTitle,
                    body: pCampaignContent.NotificationMessage,
                    actionButtonText: "ACEPTAR",
                    notificationOptions: (pCampaignContent.ImageURL != null) ? notificationOptions : null, //If imageURL is null or empty, sets an object's null value
                    audience: audience);

                CampaignStateResult campaignResultState = await engagementClient.Campaigns.CreateAsync(CampaignKinds.Announcements, campaigne);

                if (String.Equals(campaignResultState.State, "draft"))
                {
                    engagementCampaignID = campaignResultState.Id;

                    var campaignStatus = await engagementClient.Campaigns.ActivateAsync(CampaignKinds.Announcements, engagementCampaignID);
                    operationResult.CodeResult = campaignStatus.State;

                    if (!String.Equals(campaignStatus.State, "scheduled") || !String.Equals(campaignStatus.State, "in-progress"))
                        operationResult.Result = true;
                }

            }
            catch (ApiErrorException apiEx)
            { 
                if(String.Equals(apiEx.Body.Error.Code, "Conflict"))
                {
                    operationResult.CodeResult = "conflict";
                }

                EventViewerLoggerSVC.LogError("CreateMassiveCampaign: " + apiEx.Message);
            }
            catch (Exception ex)
            {
                operationResult.CodeResult = "error";
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerSVC.LogError("CreateMassiveCampaign: " + ex.Message);
            }

            return operationResult;
        }

        public async Task<NotificationResultEN> PushTopupRequest(string pPersonEmail, string pRequester, string pAmount)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "Engagement";

            try
            {
                var credentials = await ApplicationTokenProvider.LoginSilentAsync(Constants.TENANT_ID, Constants.CLIENT_ID, Constants.CLIENT_SECRET);
                engagementClient = new EngagementManagementClient(credentials)
                {
                    SubscriptionId = Constants.SUBSCRIPTION_ID
                };

                engagementClient.ResourceGroupName = Constants.RESOURCE_GROUP;
                engagementClient.AppCollection = Constants.APP_COLLECTION_NAME;
                engagementClient.AppName = Constants.APP_RESOURCE_NAME_ANDROID;

                Device userDevice = await engagementClient.Devices.GetByUserIdAsync(pPersonEmail);

                Campaign campaign = new Campaign();
                campaign.Type = "only_notif";
                campaign.DeliveryTime = "any";
                campaign.PushMode = "manual";
                campaign.NotificationType = "system";
                campaign.NotificationCloseable = true;
                campaign.NotificationTitle = "¡Nueva solicitud de recarga!";
                campaign.NotificationMessage = "Hola ${userFirstName}, el número: " + pRequester + " te ha pedido una recarga de " + pAmount + ". Revisa el listado de solicitudes de recarga.";

                List<string> devices = new List<string>();
                devices.Add(userDevice.DeviceId);

                CampaignPushParameters parameters = new CampaignPushParameters(devices, campaign);
                CampaignPushResult pushResult = await engagementClient.Campaigns.PushAsync(CampaignKinds.Announcements, Constants.TopupRequestCampaign, parameters);

                if (pushResult.InvalidDeviceIds.Count <= 0)
                {
                    //Success
                    operationResult.CodeResult = "Success";
                    operationResult.Result = true;
                }

            }
            catch (ApiErrorException apiEx)
            {
                if (String.Equals(apiEx.Body.Error.Code, "Conflict"))
                {
                    operationResult.CodeResult = "conflict";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return operationResult;
        }

        public async Task<NotificationResultEN> PushDepositNotification(CampaignEN pCampaign, string pUserEmail, string pFirstName, string pContent)
        {
            NotificationResultEN operationResult = new NotificationResultEN();
            operationResult.Platform = "ANDROID";
            operationResult.Result = false;
            operationResult.ServiceName = "Engagement";
            int campaignID = Constants.DepositConfCampaign;

            try
            {
                var credentials = await ApplicationTokenProvider.LoginSilentAsync(Constants.TENANT_ID, Constants.CLIENT_ID, Constants.CLIENT_SECRET);
                engagementClient = new EngagementManagementClient(credentials)
                {
                    SubscriptionId = Constants.SUBSCRIPTION_ID
                };

                engagementClient.ResourceGroupName = Constants.RESOURCE_GROUP;
                engagementClient.AppCollection = Constants.APP_COLLECTION_NAME;
                engagementClient.AppName = Constants.APP_RESOURCE_NAME_ANDROID;


                string userIdentifier = (!YVRSingleTestModeActive()) ? pUserEmail : ConfigurationManager.AppSettings["Engagement_SingleUserTest"].ToString();

                Device userDevice = await engagementClient.Devices.GetByUserIdAsync(userIdentifier);

                Campaign campaign = new Campaign();
                campaign.Type = "text/plain";
                campaign.DeliveryTime = "any";
                campaign.PushMode = "manual";
                campaign.NotificationType = "system";
                campaign.NotificationCloseable = true;
                campaign.NotificationTitle = pCampaign.NotificationTitle;
                campaign.NotificationMessage = String.Format("Hola {0}, tu deposito fue validado de forma exitosa, haz click aquí para más detalles.", pFirstName);
                campaign.Title = pCampaign.Title;
                campaign.Body = pContent;
                campaign.ActionButtonText = "ACEPTAR";


                List<string> devices = new List<string>();
                devices.Add(userDevice.DeviceId);

                CampaignPushParameters parameters = new CampaignPushParameters(devices, campaign);
                CampaignPushResult pushResult = await engagementClient.Campaigns.PushAsync(CampaignKinds.Announcements, campaignID, parameters);

                if (pushResult.InvalidDeviceIds.Count <= 0)
                {
                    //Success
                    operationResult.Result = true;
                }
            }
            catch (ApiErrorException apiEx)
            {
                if (String.Equals(apiEx.Body.Error.Code, "Conflict"))
                {
                    operationResult.CodeResult = "conflict";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return operationResult;
        }

        //Validates Test Mode
        private bool YVRMassiveTestModeActive()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["YVR_MassivePushTestMode"]);
        }

        private bool YVRSingleTestModeActive()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["YVR_SingleUserPushTestMode"]);
        }

    }
}
