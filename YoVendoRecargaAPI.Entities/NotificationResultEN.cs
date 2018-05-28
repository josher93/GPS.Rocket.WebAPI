using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class NotificationResultEN
    {
        public String ServiceName { get; set; }
        public Boolean Result { get; set; }
        public String CodeResult { get; set; }
        public String Message { get; set; }
        public String Platform { get; set; }
    }
}
