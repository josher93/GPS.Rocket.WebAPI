using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.SVC;

namespace YoVendoRecargaAPI.BL
{
    public class CampaignBL
    {
        CampaignDAL campaignDAL = new CampaignDAL();
        CampaignHandler handler = new CampaignHandler();

        #region User (YCR & YVR)
        public List<CampaignEN> GetUserNotifications(PersonEN pPerson)
        {
            List<CampaignEN> notifications = new List<CampaignEN>();

            try
            {
                notifications = campaignDAL.GetNotificationsByUserID(pPerson.PersonID);
            }
            catch (Exception ex)
            {
                notifications = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("CampaignBL " + ex.Message);
            }

            return notifications;
        }

        public List<CampaignEN> GetUserNewNotifications(PersonEN pPerson)
        {
            List<CampaignEN> notifications = new List<CampaignEN>();

            try
            {
                var data = campaignDAL.GetNotificationsByUserID(pPerson.PersonID);
                notifications = data.Where(rd => !rd.Read).ToList();
            }
            catch (Exception ex)
            {
                notifications = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("CampaignBL " + ex.Message);
            }

            return notifications;
        }

        public bool MarkNotificationAsRead(int pNotificationID)
        {
            bool read = false;
            try
            {
                read = campaignDAL.MarkNotificationAsRead(pNotificationID);
            }
            catch (Exception ex)
            {
                read = false;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("CampaignBL " + ex.Message);
            }

            return read;
        }


        public async Task<List<NotificationResultEN>> SendTopupRequest(string pPersonEmail, string pNickname, string pPhoneRequester, string pAmount, string pFirstname)
        {
            List<NotificationResultEN> camapignResult = new List<NotificationResultEN>();

            try
            {
                camapignResult = await handler.HandleTopupRequestContent(pPersonEmail, pNickname, pPhoneRequester, pAmount, pFirstname);
            }
            catch (Exception ex)
            {
                camapignResult = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("SendTopupRequest: " + ex.Message);
            }

            return camapignResult;
        }


        public List<NotificationResultEN> SendTopupDeniedNotification(string pTitle, string pMessage, string pImageURL, string pConsumerPhone)
        {
            List<NotificationResultEN> camapignResult = new List<NotificationResultEN>();

            try
            {
                if(!String.IsNullOrEmpty(pTitle) && !String.IsNullOrEmpty(pMessage) &&  !String.IsNullOrEmpty(pConsumerPhone))
                {
                    camapignResult = handler.HandleTopupDeniedContent(pTitle, pMessage, pImageURL, pConsumerPhone);
                }
                else
                {
                    camapignResult = null;
                }
            }
            catch (Exception ex)
            {
                camapignResult = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("SendTopupRequest: " + ex.Message);
            }

            return camapignResult;
        }

        public List<NotificationResultEN> SendEarnedCoinNotification(string pTitle, string pMessage, string pImageURL, string pConsumerPhone)
        {
            List<NotificationResultEN> campaignResult = new List<NotificationResultEN>();

            try
            {
                if (!String.IsNullOrEmpty(pTitle) && !String.IsNullOrEmpty(pMessage) && !String.IsNullOrEmpty(pConsumerPhone))
                {
                    campaignResult = handler.HandleEarnedCoinContent(pTitle, pMessage, pImageURL, pConsumerPhone);
                }
                else
                {
                    campaignResult = null;
                }
                    
            }
            catch (Exception ex)
            {
                campaignResult = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("SendTopupRequest: " + ex.Message);
            }

            return campaignResult;
        }


        #endregion

        #region Admin YVR

        public async Task<List<NotificationResultEN>> SendNotification(CampaignEN pCampaign)
        {
            List<NotificationResultEN> camapignResult = new List<NotificationResultEN>();

            try
            {
                int resultInsert = campaignDAL.InsertCampaign(pCampaign);
                if (resultInsert > 0)
                {
                    bool inserted = campaignDAL.InsertUsersNotifications(pCampaign.CountryISO2Code, resultInsert);
                    if (inserted)
                    {
                        //Send notification
                        camapignResult = await handler.HandleContent(pCampaign);
                    }
                }
            }
            catch (Exception ex)
            {
                camapignResult = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("CampaignBL " + ex.Message);
            }

            return camapignResult;
        }

        public async Task<List<NotificationResultEN>> SendDepositConfirmation(int pUserID, string pUserEmail, string pFirstname, string pTitle, string pContent)
        {
            List<NotificationResultEN> camapignResult = new List<NotificationResultEN>();

            try
            {
                CampaignEN userCampaign = new CampaignEN();
                userCampaign.Active = true;
                userCampaign.Title = "Depósito Aplicado";
                userCampaign.NotificationTitle = pTitle;
                userCampaign.NotificationMessage = String.Format("Hola {0}. {1}", pFirstname, pContent);
                userCampaign.Read = false;
                userCampaign.RegDate = DateTime.Now;

                int campaignID = campaignDAL.InsertCampaign(userCampaign);

                if (campaignID > 0)
                {
                    int insertResult = campaignDAL.InsertNotificationByUser(campaignID, pUserID);

                    if (insertResult > 0)
                    {
                        //Pushes notification
                        camapignResult = await handler.HandleDepositContent(userCampaign, pUserEmail, pFirstname, pContent);
                    }
                }
            }
            catch (Exception ex)
            {
                camapignResult = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("SendDepositConfirmation " + ex.Message);
            }

            return camapignResult;
        }



        #endregion

        #region Admin RGO

        public async Task<List<NotificationResultEN>> SendRGOnNotification(CampaignEN pCampaign)
        {
            List<NotificationResultEN> camapignResult = new List<NotificationResultEN>();

            try
            {

                //Send notification
                camapignResult = await handler.HandleRGOPushContent(pCampaign);
            }
            catch (Exception ex)
            {
                camapignResult = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("CampaignBL " + ex.Message);
            }

            return camapignResult;
        }

        #endregion
    }
}

