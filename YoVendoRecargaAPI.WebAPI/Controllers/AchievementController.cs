using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.WebAPI.Interactors;
using System.Web.Http.Description;
using YoVendoRecargaAPI.Game;
using System.Configuration;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class AchievementController : ApiController
    {
        GameBL gameBL = new GameBL();
       
        /// <summary>
        /// Get Achievement by Consumer
        /// </summary>
        /// GET: GetAchievementsByConsumer
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetAchievementsByConsumer")]
        [ResponseType(typeof(List<AchievementsByConsumerResponse>))]
        public HttpResponseMessage GetAchievementsByConsumer(HttpRequestMessage request)
        {
            string error = "";
            PersonBL ConsumerAuth = new PersonBL();
            AchievementInteractor achievementInteractor = new AchievementInteractor();
            GenericApiResponse response = new GenericApiResponse();
            AchievementBL achievementBL = new AchievementBL();

            try
            {
                IEnumerable<string> AppVersion = null;
                request.Headers.TryGetValues("AppVersion", out AppVersion);

                IEnumerable<string> Platform = null;
                request.Headers.TryGetValues("Platform", out Platform);

                bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

                if (ApplyValidation == false || (AppVersion != null && Platform != null))
                {
                    string versionRequired = "";

                    var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                    if (isValidVersion)
                    {
                        IEnumerable<string> key = null;
                        request.Headers.TryGetValues("authenticationKey", out key);
                        var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                        if (consumerFb != null)
                        {

                            var AchievemntsByConsumer = achievementBL.GetAchievementsByConsumer(consumerFb.ConsumerID, ref error);

                            if (AchievemntsByConsumer != null)
                            {
                                List<AchievementsByConsumerResponse> listAchievementsByConsumer = new List<AchievementsByConsumerResponse>();

                                achievementInteractor.listAchievementsByConsumer = achievementInteractor.createAchievementsByConsumerResponse(AchievemntsByConsumer);

                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, achievementInteractor);
                            }
                            else
                            {
                                response.HttpCode = 400;
                                response.Message = "invalid parameters";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }

                        }
                        else
                        {
                            response.HttpCode = 404;
                            response.Message = "Facebook information not found";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 426;
                        response.InternalCode = versionRequired;
                        response.Message = "Upgrade required";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.UpgradeRequired, response);
                    }

                }
                else
                {
                    response.HttpCode = 404;
                    response.Message = "Version or Platform parameter can not be null";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
            }
            catch (Exception ex)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}