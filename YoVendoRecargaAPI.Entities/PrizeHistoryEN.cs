using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class PrizeHistoryEN
    {
        public DateTime RegDate { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DialNumberOrPlace { get; set; }
        public int Level { get; set; }
    }
}
