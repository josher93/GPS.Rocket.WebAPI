using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class LeaderBoardsController : ApiController
    {
        GameBL gameBL = new GameBL();
        LeaderBoardsInteractor interactor = new LeaderBoardsInteractor();
        [HttpPost]
        [Route("LeaderBoards")]    
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] LeaderBoardsRequest search)
        {
            GenericApiResponse response = new GenericApiResponse();

            if (!string.IsNullOrEmpty(search.Search))
            {
                var responseSuccess = interactor.createSuccessResponse(search.Search);

                if (responseSuccess != null)
                {
                    return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                }
                else
                {
                    response.HttpCode = 500;
                    response.Message = "something went wrong";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "search parameter cannot be empty";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpPost]
        [Route("GetLeaderBoards")]
        public HttpResponseMessage GetLeaderBoards(HttpRequestMessage request)
        {
            GenericApiResponse response = new GenericApiResponse();

            try
            {
                var responseSuccess = interactor.createResponseSaveJson();

                if (responseSuccess != null)
                {
                    return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                }
                else
                {
                    response.HttpCode = 500;
                    response.Message = "something went wrong";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                
            }
            catch (Exception)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }
    }
}
