using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class TopupEN
    {
        public String Phone { get; set; }
        public Decimal Amount { get; set; }
        public String Operator { get; set; }
        public ProductEN Product { get; set; }
        public Int32 PackageCode { get; set; }
        public Boolean IsValid { get; set; }
        public Int32 PersonID { get; set; }
        public String Result { get; set; }
        public int ResultCode { get; set; }
        public Int32 OperatorID { get; set; }
        public Int32 CategoryID { get; set; }
    }
}
