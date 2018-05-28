using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TopupRequest
    {
        public String Operador { get; set; }
        public long? IdCountry { get; set; }
    }
}