using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class OperatorsResponse: IResponse
    {
        public Operators operators { get; set; }
        public int count { get; set; }
    }
}