using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class ExternalClientController : ApiController
    {

        ExternalClientBL clientBL = new ExternalClientBL();
        GenericApiResponse response = new GenericApiResponse();

        [HttpPost]
        [Route("addExternalClient")]
        public HttpResponseMessage AddExternalApiClient(HttpRequestMessage pRequest, ExternalClientData externalClient)
        {
            string apikey = null;
            var registeredClient = clientBL.RegisterExternalClient(externalClient.countryID, externalClient.name, externalClient.alias, externalClient.description, externalClient.assignedPassword, ref apikey);

            if (registeredClient != null)
            {
                ExternalClientResponse registerResponse = new ExternalClientResponse();
                registerResponse.alias = registeredClient.Alias;
                registerResponse.apikey = apikey;
                registerResponse.name = registeredClient.Name;
                registerResponse.registrationDate = registeredClient.RegDate;

                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, registerResponse);
            }
            else
            {
                response.HttpCode = 500;
                response.Message = "Something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }

        }
    }
}
