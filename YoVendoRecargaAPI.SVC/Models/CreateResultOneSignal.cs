using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.SVC.Models
{
    public class CreateResultOneSignal
    {
        public string id { get; set; }
        public int recipients { get; set; }
        public string[] errors { get; set; }
    }
}
