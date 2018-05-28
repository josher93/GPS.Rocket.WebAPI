using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class RocketSaleDetailEN
    {
        public int BalanceID { get; set; }
        public int PersonID { get; set; }
        public int PersonMasterID { get; set; }
        public decimal Receivable { get; set; } //cuenta por cobrar
        public decimal ReconcileCount { get; set; } //cuenta por conciliar
        public string Name { get; set; } //Nombre de sucursal
        public int BStatus { get; set; } //Estatus del balance
        public string Zone { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public string Distributor { get; set; }
        public decimal Sale { get; set; } //venta

    }
}
