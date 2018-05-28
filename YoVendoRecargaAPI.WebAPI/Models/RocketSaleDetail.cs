using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class RocketSaleDetail : IResponse
    {
        public int balanceID { get; set; }
        public int personID { get; set; }
        public int personMasterID { get; set; }
        public decimal receivable { get; set; } //cuenta por cobrar
        public decimal reconcileCount { get; set; } //cuenta por conciliar
        public string name { get; set; } //Nombre de sucursal
        public int bStatus { get; set; } //Estatus del balance
        public string zone { get; set; }
        public DateTime minDate { get; set; }
        public DateTime maxDate { get; set; }
        public string distributor { get; set; }
        public decimal sale { get; set; } //venta
    }
}