using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class CountriesController : ApiController
    {
        CountryBL countryBL = new CountryBL();
        PersonBL personBL = new PersonBL();
        GenericApiResponse response = new GenericApiResponse();
        CountryResponse countryResponse = new CountryResponse();

        [HttpGet]
        [Route("Countries")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                var inf = countryBL.GetCountries();
                countryResponse.countries = inf;
                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, countryResponse);
            }
            catch (Exception)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
