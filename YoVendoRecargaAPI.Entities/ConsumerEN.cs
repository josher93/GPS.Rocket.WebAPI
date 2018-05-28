using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("Consumer", Schema = "Consumer")]
    public class ConsumerEN
    {
        [Key]
        public int ConsumerID { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }

        [Column("CountryID")]
        public int CountryID { get; set; }

        [Column("Active")]
        public bool Active { get; set; }

        [Column("DeviceID")]
        public string DeviceID { get; set; }

        [Column("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [Column("ModificationDate")]
        public DateTime ModificationDate { get; set; }

        [Column("URL")]
        public string URL { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("ProfileID")]
        public string ProfileID { get; set; }

        [Column("UserID")]
        public string UserID { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("MiddleName")]
        public string MiddleName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("Nickname")]
        public string Nickname { get; set; }

        [NotMapped]
        public string ConsumerAuthKey { get; set; }

        [NotMapped]
        public bool IsValidKey { get; set; }
    }
}
