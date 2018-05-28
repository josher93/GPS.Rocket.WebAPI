using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class UserBagController : ApiController
    {
        PersonBL personBL = new PersonBL();
        BagBL bagBL = new BagBL();
        UserBagInteractor interactor = new UserBagInteractor();
        GenericApiResponse response = new GenericApiResponse();

        [HttpGet]
        [Route("userbag")]
        public HttpResponseMessage Get(HttpRequestMessage pRequest)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (token != null)
            {
                if (personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        var userBags = bagBL.GetUserBags(personVerified.PersonID);

                        if (userBags != null)
                        {
                            string newToken = personBL.RenewAuthToken(personVerified);
                            var bagsResponse = interactor.CreateBagResponse(userBags, newToken, personVerified.PersonID);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, bagsResponse);
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
                        response.Message = "Authentication token has expired.";
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
    }
}
