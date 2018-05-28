using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ChangeAgeEN
    {
        public int AgeID { get; set; }
        public string Name { get; set; }
        public string IconImage { get; set; }
        public int RequiredSouvenir { get; set; }
        public int SouvenirByConsumer { get; set; }
        
    }
}
