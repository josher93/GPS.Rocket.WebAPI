using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TopupResultResponse : IResponse
    {
        public string id { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Info info { get; set; }
        public List<OperatorBalance> OperatorsBalance { get; set; }
    }
}