using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class PromotionEN
    {
        public int PromotionID { get; set; }
        public int OperatorID { get; set; }
        public String OperatorBrand { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String URL { get; set; }
        public String HttpMethod { get; set; }
        public bool Active { get; set; }
    }
}
