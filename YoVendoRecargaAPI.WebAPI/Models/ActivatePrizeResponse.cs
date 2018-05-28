using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ActivatePrizeResponse : IResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}