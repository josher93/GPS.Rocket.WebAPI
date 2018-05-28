using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class BuildValidationReqBody
    {
        public string appVersionName { get; set; }
        public string platformName { get; set; }
    }
}