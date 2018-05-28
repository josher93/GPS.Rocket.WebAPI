using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class CountryResponse : IResponse
    {
        public List<CountryEN> countries { get; set; }
    }
}