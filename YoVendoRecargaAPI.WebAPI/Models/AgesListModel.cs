using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class AgesListModel: IResponse
    {
        public int ageID { get; set; }
        public string name { get; set; }
        public int status { get; set; }
        public string iconImage { get; set; }
        public string mainImage { get; set; }
    }
}