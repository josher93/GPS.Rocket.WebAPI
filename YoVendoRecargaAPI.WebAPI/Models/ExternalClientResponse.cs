using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ExternalClientResponse : IResponse
    {
        public string name { get; set; }
        public string alias { get; set; }
        public DateTime registrationDate { get; set; }
        public string apikey { get; set; }
    }
}