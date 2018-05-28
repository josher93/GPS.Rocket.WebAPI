using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class BuildResponse : IResponse
    {
        public bool Valid { get; set; }
        public string RequiredVersion { get; set; }
        public string Platform { get; set; }
        public string Message { get; set; }
    }
}