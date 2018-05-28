using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class DepositInteractor
    {
        BankDepositBL depositBL = new BankDepositBL();

        public BankDepositResponse CreatorDepositResponse(bool pResult, string pNewToken)
        {
            BankDepositResponse response = new BankDepositResponse();

            response.status = pResult;
            response.token = pNewToken;

            return response;
        }
    }
}