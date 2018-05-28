using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class AgeResponse: IResponse
    {
        public Ages ages { get; set; }
        public int count { get; set; }
    }
}