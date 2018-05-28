using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class NotificationsInteractor
    {
        public NotificationsResponse CreateNotificationsResponse(List<CampaignEN> pNotifications)
        {
            NotificationsResponse response = new NotificationsResponse();
            NotificationMessages messages = new NotificationMessages();
            messages.userNotifications = new List<NotificationItem>();

            try
            {
                foreach (var item in pNotifications)
                {
                    NotificationItem notif = new NotificationItem();
                    notif.trackingID = item.NotificationID;
                    notif.notificationID = item.CampaignID;
                    notif.title = item.NotificationTitle;
                    notif.message = item.NotificationMessage;
                    notif.seen = item.Read;

                    messages.userNotifications.Add(notif);
                }

                response.notifications = messages;
                response.count = messages.userNotifications.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }

        public MassivePushResult CreateMassiveNotificationResponse(List<NotificationResultEN> pResult, DateTime pStartedAt)
        {
            MassivePushResult response = new MassivePushResult();
            response.pushResults = new List<MassivePushItemResult>();

            try
            {
                foreach (var item in pResult)
                {
                    MassivePushItemResult push = new MassivePushItemResult();
                    push.code = item.CodeResult;
                    push.isSuccessful = item.Result;
                    push.platform = item.Platform;
                    push.resultText = item.Message;
                    push.pushServer = item.ServiceName;
                    response.pushResults.Add(push);
                }

                response.operationStart = pStartedAt;
                response.operationFinished = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }

        public SimpleTextResponse CreateDepositConfirmationResponse(List<NotificationResultEN> pResult)
        {
            SimpleTextResponse response = new SimpleTextResponse();
            int counter = 0;

            try
            {
                foreach (var item in pResult)
                {
                    counter = (item.Result) ? counter + 1 : counter;
                }

                if (counter <= 0)
                {
                    response.result = false;
                    response.Message = "Something went wrong";
                }
                else
                {
                    response.result = true;
                    response.Message = "Operation completed succesfully.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }
    }
}