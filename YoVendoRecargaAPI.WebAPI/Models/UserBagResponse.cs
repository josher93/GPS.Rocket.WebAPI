using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class UserBagResponse : IResponse
    {
        public UserBag userBag { get; set; }
        public String token { get; set; }
    }
}