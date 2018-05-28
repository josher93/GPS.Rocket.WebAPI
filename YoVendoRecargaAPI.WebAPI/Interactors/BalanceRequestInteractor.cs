using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class BalanceRequestInteractor
    {
        public IResponse createSuccessResponse(PersonEN person)
        {
            BalanceRequestResponse response = new BalanceRequestResponse();
            try
            {
                response.MasterEmail = person.EmailMaster;
                response.MasterName = person.PersonMaster;
                response.Status = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }
    }
}