using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class BankResponse: IResponse
    {
        public BankList banks { get; set; }
        public int count { get; set; }
    }
}