using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ProfileResponse : IResponse
    {
        public UserProfile profile { get; set; }
    }
}