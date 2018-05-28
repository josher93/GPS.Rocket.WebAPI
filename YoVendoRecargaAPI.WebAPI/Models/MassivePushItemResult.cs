using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class MassivePushItemResult
    {
        public bool isSuccessful { get; set; }
        public string code { get; set; }
        public string resultText { get; set; }
        public string platform { get; set; }
        public string pushServer { get; set; }
    }
}