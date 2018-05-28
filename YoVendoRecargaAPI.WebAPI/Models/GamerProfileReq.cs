using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class GamerProfileReq
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public int PersonID { get; set; }
        public DateTime regDate { get; set; }
    }
}