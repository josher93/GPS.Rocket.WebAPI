using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Topup.Models
{
    public class ApiResponse
    {
        public string resultCode { get; set; }
        public String errorMessage { get; set; }
        public string serviceTransactionId { get; set; }
        public List<ExtraParameters> ExtraParameters { get; set; }
    }
}
