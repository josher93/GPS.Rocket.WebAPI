using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class UserBag
    {
        public Int32 PersonID { get; set; }
        public String AvailableAmount { get; set; }
        public List<OperatorBalance> OperatorsBalance { get; set; }
    }
}