using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class PersonEN
    {
        public int PersonID { get; set; }
        public String Firstname { get; set; }
        public String Middlename { get; set; }
        public String Lastname { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Nickname { get; set; }
        public String Phone { get; set; }
        public DateTime Birthdate { get; set; }
        public String CurrentToken { get; set; }
        public DateTime TokenExpiration { get; set; }
        public bool IsValidToken { get; set; }
        public Decimal SingleBagValue { get; set; }
        public int MasterID { get; set; }
        public bool VendorM { get; set; }
        public String PhoneCode { get; set; }
        public String ISO2Code { get; set; }
        public String ISO3Code { get; set; }
        public int CountryID { get; set; }
        public int VendorCode { get; set; }
        public bool ProfileCompleted { get; set; }
        public bool EmailVerified { get; set; }
        public String CurrencySymbol { get; set; }
        public int? ReferredStatus { get; set; }
        public int RollID { get; set; }
        public String DeviceInfo { get; set; }
        public String DeviceIp { get; set; }
        public String DeviceID { get; set; }
        public String BuildVersion { get; set; }
        public int SessionID { get; set; }
        public bool Active { get; set; }
        public List<PersonBagOperatorEN> OperatorsBalance { get; set; }
        public int CategoryID { get; set; }
        // For master
        public string PersonMaster { get; set; }
        public string EmailMaster { get; set; }
        public string MyName { get; set; }

    }
}
