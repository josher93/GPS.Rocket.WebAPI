using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class TokenEN
    {
        public int PersonID { get; set; }
        public String PersonEmail { get; set; }
        public String Password { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int VendorCode { get; set; }
        public bool ProfileCompleted { get; set; }
        public String DeviceIP { get; set; }
        public String DeviceID { get; set; }
        public int MasterID { get; set; }
        public int CountryID { get; set; }
        public bool State { get; set; }
    }
}
