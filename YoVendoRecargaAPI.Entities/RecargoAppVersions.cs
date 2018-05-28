using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class RecargoAppVersions
    {
        public int AppVersionID { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public bool Required { get; set; }
        public bool Status { get; set; }
        public string PlatformName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime RegDate { get; set; }
    }
}
