using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("Campaign", Schema = "dbo")]
    public class CampaignEN
    {
        [Key]
        public int CampaignID { get; set; }

        [Column("Title")]
        public String Title { get; set; }

        [Column("PlatformID")]
        public int PlatformID { get; set; }

        [NotMapped]
        public String Platform { get; set; }

        [NotMapped]
        public int PersonID { get; set; }

        [NotMapped]
        public int NotificationID { get; set; }

        [Column("Description")]
        public String Description { get; set; }

        [Column("NotificationTitle")]
        public String NotificationTitle { get; set; }

        [Column("NotificationMessage")]
        public String NotificationMessage { get; set; }

        [Column("ImageURL")]
        public String ImageURL { get; set; }

        [NotMapped]
        public String Result { get; set; }

        [NotMapped]
        public Boolean IsSuccessful { get; set; }

        [NotMapped]
        public String CountryISO2Code { get; set; }

        [Column("Active")]
        public Boolean Active { get; set; }

        [NotMapped]
        public Boolean Read { get; set; }

        [Column("RegDate")]
        public DateTime RegDate { get; set; }

        [NotMapped]
        public String SpecificUser { get; set; }
    }
}
