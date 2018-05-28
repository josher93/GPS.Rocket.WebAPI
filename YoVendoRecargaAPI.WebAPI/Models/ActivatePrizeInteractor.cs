using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.SVC;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ActivatePrizeInteractor
    {
        public IResponse SuccessActivatePrizeResponse(int ConsumerID, string Phone, string PIN, ref string error)
        {
            ActivatePrizeResponse response = new ActivatePrizeResponse();
            GameBL gameBl = new GameBL();
            string result = "";

            try
            {
                ClaroClient client = new ClaroClient();

                result = client.ActivatePin(ConsumerID, Phone, PIN, ref error);
                YoVendoRecargaAPI.BL.EventViewerLoggerBL.LogInformation("Response Web service Claro: " + result);

                var insertProcess = gameBl.InsertActivatePIN(ConsumerID, Phone, PIN, "Claro El Salvador", result, ref error);

                response.Code = (result == "0") ? "00" : result;
                response.Message = "Success";

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "";
            }

            return response;
        }
    }
}