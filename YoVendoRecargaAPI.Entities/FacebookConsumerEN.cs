using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class FacebookConsumerEN
    {
        public int ConsumerID { get; set; }
        public String Phone { get; set; }
        public int CountryID { get; set; }
        public bool Active { get; set; }
        public String DeviceID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public String URL { get; set; }
        public String Email { get; set; }
        public String ProfileID { get; set; }
        public String UserID { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Nickname { get; set; }
    }
}
