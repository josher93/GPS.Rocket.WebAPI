using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("Facebook", Schema = "Person")]
    public class FacebookEN
    {
        [Key]
        public int Id { get; set; }

        [Column("ProfileID")]
        public String FacebookProfileID { get; set; }

        [Column("UserID")]
        public String FacebookUserID { get; set; }

        [Column("Firstname")]
        public String Firstname { get; set; }

        [Column("MiddleName")]
        public String MiddleName { get; set; }

        [Column("Lastname")]
        public String Lastname { get; set; }

        [Column("Email")]
        public String Email { get; set; }

        [Column("URL")]
        public String URL { get; set; }

        [Column("PhoneNumber")]
        public String Phone { get; set; }

        [Column("PersonId")]
        public int PersonID { get; set; }

        [Column("RegDate")]
        public DateTime RegDate { get; set; }
    }
}
