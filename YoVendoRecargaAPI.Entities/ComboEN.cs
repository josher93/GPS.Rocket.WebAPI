using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ComboEN
    {
        public int ComboID { get; set; }
        public int PacketID { get; set; }
        public int AgeID { get; set; }
        public string Description { get; set; }
        public int Available { get; set; }
        public DateTime RegDate { get; set; }

        public SouvenirEN pSouvenir { get; set; }
        public int Secuence { get; set; }
        public string PrizeTitle { get; set; }
        public string PrizeDescription { get; set; }
    }
}
