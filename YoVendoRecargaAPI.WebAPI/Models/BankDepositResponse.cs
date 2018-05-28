using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class BankDepositResponse: IResponse
    {
        public Boolean status { get; set; }
        public String token { get; set; }
    }
}