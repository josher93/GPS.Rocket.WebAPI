using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class BuildVersionEN
    {
        public int AppVersionID { get; set; }
        public int VersionCode { get; set; }
        public String VersionName { get; set; }
        public String Description { get; set; }
        public String Features { get; set; }
        public bool Required { get; set; }
        public bool Status { get; set; }
        public bool Valid { get; set; }
        public String Result { get; set; }
        public String Platform { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
