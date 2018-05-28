using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ExternalClientData
    {
        public int countryID { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string description { get; set; }
        public string guid { get; set; }
        public string assignedPassword { get; set; }
        public bool active { get; set; }
        public DateTime date { get; set; }
    }
}