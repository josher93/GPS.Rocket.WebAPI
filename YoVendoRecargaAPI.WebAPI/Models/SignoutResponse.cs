using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SignoutResponse : IResponse
    {
        public Boolean Status { get; set; }
    }
}