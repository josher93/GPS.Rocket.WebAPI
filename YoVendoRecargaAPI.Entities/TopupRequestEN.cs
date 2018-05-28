using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("TopupRequests", Schema = "Consumer")]
    public class TopupRequestEN
    {
        [Key]
        public int TopupRequestID { get; set; }

        [Column("ConsumerID")]
        public int ConsumerID { get; set; }

        [Column("TargetPhoneNumber")]
        public String TargetPhone { get; set; }

        [NotMapped]
        public String ConsumerNickname { get; set; }

        [NotMapped]
        public String ConsumerPhone { get; set; }

        [NotMapped]
        public String OperatorName { get; set; }

        [Column("VendorCode")]
        public int VendorCode { get; set; }

        [NotMapped]
        public int PersonID { get; set; }

        [Column("OperatorID")]
        public int OperatorID { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("StatusCode")]
        public int StatusCode { get; set; }

        [NotMapped]
        public int PackageCode { get; set; }

        [Column("RegDate")]
        public DateTime RequestDate { get; set; }

        [NotMapped]
        public string RequestDateISO { get; set; }

        [Column("ModDate")]
        public DateTime ModDate { get; set; }

        [Column("CategoryID")]
        public int CategoryID { get; set; }
    }
}
