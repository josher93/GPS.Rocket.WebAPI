using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ConsumerAuthEN
    {
        public int ConsumerAuthID { get; set; }

        public int ConsumerID { get; set; }

        public string ConsumerAuthKey { get; set; }

        public DateTime RegDate { get; set; }
    }
}
