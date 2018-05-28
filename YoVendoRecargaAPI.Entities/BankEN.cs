using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class BankEN
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public int MinLenght { get; set; }
        public int MaxLength { get; set; }
    }
}
