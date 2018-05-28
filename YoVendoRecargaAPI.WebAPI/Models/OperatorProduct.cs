using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class OperatorProduct
    {
        public int operatorID { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public string operatorLogo { get; set; }
        public string logoUrl { get; set; }
        public string HexColor { get; set; }
        public string RGBColor { get; set; }
        public int Relevance { get; set; }
        public RGBColor rgbColor { get; set; }
        public List<OperatorAmounts> Amounts { get; set; }
    }

    public class RGBColor
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }

    public class OperatorAmounts
    {
        public string Code { get; set; }
        public string OperatorName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int Relevance { get; set; }
    }
}