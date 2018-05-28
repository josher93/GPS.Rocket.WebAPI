using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.WebAPI.Interactors;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class StoresController : ApiController
    {
        [HttpPost]
        [Route("SendLackCreditReport")]
        public HttpResponseMessage SendLackCreditReport(HttpRequestMessage request, [FromBody] StoreAttributes data)
        {
            PersonBL ConsumerAuth = new PersonBL();
            GenericApiResponse response = new GenericApiResponse();
            StoreBL storeBL = new StoreBL();
            string error = "";
            SimpleTextResponse ReportResponse = new SimpleTextResponse();

            try
            {
                IEnumerable<string> key = null;
                request.Headers.TryGetValues("authenticationKey", out key);
                var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                if (consumerFb != null)
                {
                    if (!string.IsNullOrEmpty(data.FirebaseID))
                    {
                        int? ResultOfReport = storeBL.AirTimeReporting(data.StoreName, data.AddressStore, data.Longitude, data.Latitude, data.FirebaseID, consumerFb.ConsumerID, consumerFb.RegistrationDate, consumerFb.ModificationDate, ref error);

                        if (ResultOfReport > 0)
                        {
                            ReportResponse.Message = "Operation completed succesfully";
                            ReportResponse.result = true;

                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, ReportResponse);
                        }
                        else
                        {
                            ReportResponse.Message = "Something went wrong";
                            ReportResponse.result = false;

                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, ReportResponse);
                        }
                    }
                    else
                    {
                        ReportResponse.Message = "Bad Request";
                        ReportResponse.result = false;

                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, ReportResponse);
                    }
                }
                else
                {
                    response.HttpCode = 404;
                    response.Message = "Facebook information not found";
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
