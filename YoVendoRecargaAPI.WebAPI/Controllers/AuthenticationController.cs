using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.WebAPI.Interactors;
using Newtonsoft.Json.Linq;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class AuthenticationController : ApiController
    {
        PersonBL personBL = new PersonBL();
        SessionBL sessionBL = new SessionBL();
        GenericApiResponse response = new GenericApiResponse();

        #region YoVendoRecarga



        [HttpPost]
        [Route("Signin")]
        public HttpResponseMessage Signin([FromBody] SigninReqBody signinRequest)
        {
            GenericApiResponse response = new GenericApiResponse();

            if (String.IsNullOrEmpty(signinRequest.email) || String.IsNullOrEmpty(signinRequest.password))
            {
                response.HttpCode = 400;
                response.Message = "Email or password cannot be empty";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
            else
            {
                var person = personBL.AuthenticatePerson(signinRequest.email, signinRequest.password, signinRequest.deviceInfo, signinRequest.deviceIP, signinRequest.deviceID, false);
                if (person != null)
                {

                    if (!person.ProfileCompleted)
                    {
                        response.HttpCode = 403;
                        response.Message = "Make sure your profile is complete.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Forbidden, response);
                    }
                    else
                    {
                        //Crear respuesta exitosa
                        SigninInteractor interactor = new SigninInteractor();
                        var responseSuccess = interactor.createSuccessResponse(person);
                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Invalid credentials";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }

        }

        [HttpPost]
        [Route("Auth/Signin")]
        public HttpResponseMessage EncryptedSignin([FromBody] SigninReqBody signinRequest)
        {
            GenericApiResponse response = new GenericApiResponse();

            if (String.IsNullOrEmpty(signinRequest.email) || String.IsNullOrEmpty(signinRequest.password))
            {
                response.HttpCode = 400;
                response.Message = "Email or password cannot be empty";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
            //else if (String.IsNullOrEmpty(signinRequest.buildVersion))
            //{
            //    response.HttpCode = 400;
            //    response.Message = "Build version must be provided";
            //    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            //}
            else
            {
                var person = personBL.AuthenticatePerson(signinRequest.email, signinRequest.password, signinRequest.deviceInfo, signinRequest.deviceIP, signinRequest.deviceID, true);
                if (person != null)
                {

                    if (!person.ProfileCompleted)
                    {
                        response.HttpCode = 403;
                        response.Message = "Make sure your profile is complete.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Forbidden, response);
                    }
                    else
                    {
                        //Crear respuesta exitosa
                        SigninInteractor interactor = new SigninInteractor();
                        var responseSuccess = interactor.createSuccessResponse(person);
                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Invalid credentials";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }

        }

        [HttpPost]
        [Route("Signout")]
        public HttpResponseMessage Signout(HttpRequestMessage pRequest, [FromBody] JToken pFormBody)
        {
            HttpResponseMessage responseMessage;

            try
            {
                int SessionID = pFormBody.Value<int?>("SessionID") ?? 0;

                if (SessionID > 0)
                {
                    if (sessionBL.SignoutUser(SessionID) > 0)
                    {
                        SignoutResponse result = new SignoutResponse();
                        result.Status = true;
                        responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        response.HttpCode = 500;
                        response.Message = "Something went wrong";
                        responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                    }

                }
                else
                {
                    response.HttpCode = 400;
                    response.Message = "SessionID is required";
                    responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.HttpCode = 500;
                response.Message = "Something went wrong";
                responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }

            return responseMessage;
        }


        #endregion


    }
}

