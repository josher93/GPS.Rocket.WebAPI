using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("SessionsLog", Schema = "dbo")]
    public class SessionEN
    {
        [Key]
        public int SessionID { get; set; }

        [Column("PersonID")]
        public int PersonID { get; set; }

        [Column("RegDate")]
        public DateTime RegDate { get; set; }

        [Column("ActiveSession")]
        public bool ActiveSession { get; set; }

        [Column("DeviceInfo")]
        public string DeviceInfo { get; set; }

        [Column("DeviceID")]
        public string DeviceID { get; set; }

        [Column("MobileDevice")]
        public bool MobileDevice { get; set; }

        [Column("DeviceIP")]
        public string DeviceIP { get; set; }

    }
}
