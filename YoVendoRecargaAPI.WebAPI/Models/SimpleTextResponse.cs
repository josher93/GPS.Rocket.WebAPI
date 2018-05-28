using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SimpleTextResponse : IResponse
    {
        public bool result { get; set; }
        public string Message { get; set; }
    }
}