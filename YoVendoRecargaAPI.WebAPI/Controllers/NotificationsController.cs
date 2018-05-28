using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class NotificationsController : ApiController
    {
        PersonBL personBL = new PersonBL();
        CampaignBL campaignBL = new CampaignBL();
        ExternalClientBL clientBL = new ExternalClientBL();
        GenericApiResponse response = new GenericApiResponse();

        #region Users

        [HttpGet]
        [Route("getUserNotifications")]
        public HttpResponseMessage GetUserNotifications(HttpRequestMessage pRequest)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            if (token != null)
            {
                PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

                if (personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        var notifications = campaignBL.GetUserNotifications(personVerified);
                        if (notifications != null)
                        {
                            NotificationsInteractor interactor = new NotificationsInteractor();
                            var notifResponse = interactor.CreateNotificationsResponse(notifications);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, notifResponse);
                        }
                        else
                        {
                            response.HttpCode = 500;
                            response.Message = "Something went wrong";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Expired token or not valid credentials.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Credentials are not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpGet]
        [Route("getNewUserNotifications")]
        public HttpResponseMessage GetNewUserNotifications(HttpRequestMessage pRequest)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            if (token != null)
            {
                PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

                if (personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        //Obtiene las notificaciones no leidas a partir de las ultimas 10
                        var notifications = campaignBL.GetUserNewNotifications(personVerified);
                        if (notifications != null)
                        {
                            NotificationsInteractor interactor = new NotificationsInteractor();
                            var notifResponse = interactor.CreateNotificationsResponse(notifications);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, notifResponse);
                        }
                        else
                        {
                            response.HttpCode = 500;
                            response.Message = "Something went wrong";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Expired token or not valid credentials.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Credentials are not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpPost]
        [Route("markNotificationSeen/{notificationID}")]
        public HttpResponseMessage MarkNotificationAsSeen(HttpRequestMessage pRequest, int notificationID)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            if (token != null)
            {
                if (notificationID <= 0)
                {
                    response.HttpCode = 400;
                    response.Message = "Notification ID must be greater than zero";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

                    if (personVerified != null)
                    {
                        if (personVerified.IsValidToken)
                        {
                            if (campaignBL.MarkNotificationAsRead(notificationID))
                            {
                                SimpleTextResponse resultResponse = new SimpleTextResponse();
                                resultResponse.result = true;
                                resultResponse.Message = "Success";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, resultResponse);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "Something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }
                        }
                        else
                        {
                            response.HttpCode = 401;
                            response.Message = "Expired token or not valid credentials.";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Credentials are not valid.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpPost]
        [Route("setNotificationRead")]
        public HttpResponseMessage SetNotificationRead(HttpRequestMessage pRequest, NotificationTrackingReq pTracking)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            if (token != null)
            {
                int notificationID = int.Parse(pTracking.trackingID);

                if (notificationID <= 0)
                {
                    response.HttpCode = 400;
                    response.Message = "Notification ID must be greater than zero";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

                    if (personVerified != null)
                    {
                        if (personVerified.IsValidToken)
                        {
                            if (campaignBL.MarkNotificationAsRead(notificationID))
                            {
                                SimpleTextResponse resultResponse = new SimpleTextResponse();
                                resultResponse.result = true;
                                resultResponse.Message = "Success";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, resultResponse);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "Something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }
                        }
                        else
                        {
                            response.HttpCode = 401;
                            response.Message = "Expired token or not valid credentials.";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Credentials are not valid.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        #endregion

        #region Admin

        [HttpPost]
        [Route("sendNotification")]
        public async Task<HttpResponseMessage> SendNotification(HttpRequestMessage pRequest, NotificationContentRequest pContent)
        {
            IEnumerable<string> apiKey = null;
            pRequest.Headers.TryGetValues("apikey", out apiKey);

            if (apiKey != null)
            {
                var externalClient = clientBL.VerifyExternalClient(apiKey);
                if (externalClient != null)
                {
                    if (String.IsNullOrEmpty(pContent.campaignTitle))
                    {
                        response.HttpCode = 400;
                        response.Message = "Campaign title must not be empty";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pContent.countryIso2Code))
                    {
                        response.HttpCode = 400;
                        response.Message = "Must specify an ISO2 code for Country";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pContent.notificationTitle))
                    {
                        response.HttpCode = 400;
                        response.Message = "Notification title must not be empty";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pContent.notificationMessage))
                    {
                        response.HttpCode = 400;
                        response.Message = "Notification message must not be empty";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else
                    {
                        DateTime start = DateTime.Now;
                        NotificationsInteractor interactor = new NotificationsInteractor();

                        CampaignEN campaign = new CampaignEN();
                        campaign.Title = pContent.campaignTitle;
                        campaign.Description = pContent.campaignDescription;
                        campaign.NotificationTitle = pContent.notificationTitle;
                        campaign.NotificationMessage = pContent.notificationMessage;
                        campaign.ImageURL = pContent.imageUrl;
                        campaign.CountryISO2Code = pContent.countryIso2Code;
                        campaign.RegDate = DateTime.Now;

                        var resultOperation = await campaignBL.SendNotification(campaign);

                        if (resultOperation != null)
                        {
                            var responseResult = interactor.CreateMassiveNotificationResponse(resultOperation, start);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseResult);
                        }
                        else
                        {
                            response.HttpCode = 500;
                            response.Message = "Something went wrong";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                        }
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "API Key provided is not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "API Key must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }

        }

        [HttpPost]
        [Route("depositConfirmation")]
        public async Task<HttpResponseMessage> DepositConfirmation(HttpRequestMessage pRequest, DepositValidationRequest pBody)
        {
            IEnumerable<string> apiKey = null;
            pRequest.Headers.TryGetValues("apikey", out apiKey);

            if (apiKey != null)
            {
                var externalClient = clientBL.VerifyExternalClient(apiKey);
                if (externalClient != null)
                {
                    if (pBody.userId <= 0)
                    {
                        response.HttpCode = 400;
                        response.Message = "PersonID must be greater than 0";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pBody.userEmail))
                    {
                        response.HttpCode = 400;
                        response.Message = "Email is required";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pBody.userFirstname))
                    {
                        response.HttpCode = 400;
                        response.Message = "User first name must be provided";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pBody.title))
                    {
                        response.HttpCode = 400;
                        response.Message = "Notification title is required";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pBody.message))
                    {
                        response.HttpCode = 400;
                        response.Message = "Notification message is required";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else
                    {
                        var resultOperation = await campaignBL.SendDepositConfirmation(pBody.userId, pBody.userEmail, pBody.userFirstname, pBody.title, pBody.message);

                        if (resultOperation != null)
                        {
                            NotificationsInteractor interactor = new NotificationsInteractor();
                            var responseResult = interactor.CreateDepositConfirmationResponse(resultOperation);
                            
                            if(responseResult.result)
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseResult);
                            }
                            else
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, responseResult);
                            }
                        }
                        else
                        {
                            response.HttpCode = 500;
                            response.Message = "Something went wrong";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                        }
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "API Key provided is not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "API Key must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            } 
        }
        #endregion

        #region RecarGO

        [HttpPost]
        [Route("sendRecarGoNotif")]
        public async Task<HttpResponseMessage> sendRecarGoNotif(HttpRequestMessage pRequest, NotificationContentRequest pContent)
        {
            IEnumerable<string> apiKey = null;
            pRequest.Headers.TryGetValues("apikey", out apiKey);

            if (apiKey != null)
            {
                var externalClient = clientBL.VerifyExternalClient(apiKey);
                if (externalClient != null)
                {
                    if (String.IsNullOrEmpty(pContent.campaignTitle))
                    {
                        response.HttpCode = 400;
                        response.Message = "Campaign title must not be empty";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pContent.countryIso2Code))
                    {
                        response.HttpCode = 400;
                        response.Message = "Must specify an ISO2 code for Country";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pContent.notificationTitle))
                    {
                        response.HttpCode = 400;
                        response.Message = "Notification title must not be empty";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else if (String.IsNullOrEmpty(pContent.notificationMessage))
                    {
                        response.HttpCode = 400;
                        response.Message = "Notification message must not be empty";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                    }
                    else
                    {
                        DateTime start = DateTime.Now;
                        NotificationsInteractor interactor = new NotificationsInteractor();

                        CampaignEN campaign = new CampaignEN();
                        campaign.Title = pContent.campaignTitle;
                        campaign.Description = pContent.campaignDescription;
                        campaign.NotificationTitle = pContent.notificationTitle;
                        campaign.NotificationMessage = pContent.notificationMessage;
                        campaign.ImageURL = pContent.imageUrl;
                        campaign.CountryISO2Code = pContent.countryIso2Code;
                        campaign.SpecificUser = pContent.SpecificUser;
                        campaign.RegDate = DateTime.Now;

                        var resultOperation = await campaignBL.SendRGOnNotification(campaign); //Envia Push

                        if (resultOperation != null)
                        {
                            var responseResult = interactor.CreateMassiveNotificationResponse(resultOperation, start);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseResult);
                        }
                        else
                        {
                            response.HttpCode = 500;
                            response.Message = "Something went wrong";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                        }
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "API Key provided is not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "API Key must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }

        }

        #endregion

    }
}
