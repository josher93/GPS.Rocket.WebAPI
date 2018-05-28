using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class BankDepositoReqBody
    {
        public String fecha { get; set; }
        public string nombre { get; set; }
        public long? banco { get; set; }
        public Decimal monto { get; set; }
        public String comprobante { get; set; }
        public String token { get; set; }
    }
}