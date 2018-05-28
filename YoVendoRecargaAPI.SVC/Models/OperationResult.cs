using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.SVC.Models
{
    public class OperationResult
    {
        public string ServiceName { get; set; }
        public bool Result { get; set; }
        public string CodeResult { get; set; }
        public string Message { get; set; }
        public string Platform { get; set; }
    }
}
