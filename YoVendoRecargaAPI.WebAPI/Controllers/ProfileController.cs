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
    public class ProfileController : ApiController
    {
        ProfileBL profileBL = new ProfileBL();
        PersonBL personBL = new PersonBL();
        
        GenericApiResponse response = new GenericApiResponse();
        ProfileInteractor interactor = new ProfileInteractor();

        [HttpGet]
        [Route ("Profile")]
        public HttpResponseMessage GetProfile(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token);

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if(token != null)
            {
                if(personVerified != null)
                {
                    if(personVerified.IsValidToken)
                    {
                        ProfileEN userProfile = profileBL.GetProfile(personVerified.PersonID);

                        if(userProfile != null)
                        {
                            ProfileResponse profileResponse = interactor.CreateProfileResponse(userProfile);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, profileResponse);
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
                response.Message = "Authorization oken must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }


    }
}
