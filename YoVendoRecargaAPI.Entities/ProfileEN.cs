using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ProfileEN
    {
        public int ProfileID { get; set; }
        public DateTime Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Verified { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public int VendorID { get; set; }
        public string Iso2Code { get; set; }
        public string Symbol { get; set; }
        public string PersonPhone { get; set; }
        public string LastSale { get; set; }
    }
}
