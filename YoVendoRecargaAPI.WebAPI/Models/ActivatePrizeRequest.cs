using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ActivatePrizeRequest
    {
        public string Phone { get; set; }
        public string PIN { get; set; }
    }
}