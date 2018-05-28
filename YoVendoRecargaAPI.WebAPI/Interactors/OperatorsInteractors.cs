using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class OperatorsInteractors
    {
        OperatorBL operatorBL = new OperatorBL();

        public OperatorsResponse CreateOperatorResponse(List<OperatorEN> pOperators)
        {
            OperatorsResponse response = new OperatorsResponse();
            Operators operators = new Operators();
            operators.countryOperators = new List<CountryOperators>();
            
            foreach(var countryOperatr in pOperators)
            {
                CountryOperators _countryOperator = new CountryOperators();

                _countryOperator.operatorID = countryOperatr.OperatorID;
                _countryOperator.name = countryOperatr.Name;
                _countryOperator.brand = countryOperatr.Brand;
                _countryOperator.mnc = countryOperatr.Mnc;
                _countryOperator.operatorLogo = countryOperatr.OperatorLogo;
                _countryOperator.logoUrl = countryOperatr.LogoUrl;
                _countryOperator.logoVersion = countryOperatr.LogoVersion;
                _countryOperator.HexColor = countryOperatr.HEXColor;
                _countryOperator.RGBColor = countryOperatr.RGBColor;
                _countryOperator.Relevance = countryOperatr.Relevance;

                RGBColors rgbColor = new RGBColors();
                if (!string.IsNullOrEmpty(countryOperatr.RGBColor))
                {
                    var codeColor = countryOperatr.RGBColor.Split(',');
                    rgbColor.R = int.Parse(codeColor[0].ToString());
                    rgbColor.G = int.Parse(codeColor[1].ToString());
                    rgbColor.B = int.Parse(codeColor[2].ToString());
                }

                _countryOperator.rgbColor = rgbColor;


                operators.countryOperators.Add(_countryOperator);
            }

            response.operators = operators;
            response.count = operators.countryOperators.Count;

                return response;
        }

        public OperatorProductsResponse CreateOperatorProductsResponse(List<OperatorEN> pOperatorsResult)
        {
            OperatorProductsResponse response = new OperatorProductsResponse();
            OperatorsProducts operators = new OperatorsProducts();

            List<OperatorProduct> operatorsList = new List<OperatorProduct>();
            List<OperatorAmounts> operatorsProductsList = new List<OperatorAmounts>();

            try
            {
                foreach (var oprtor in pOperatorsResult)
                {
                    OperatorProduct operatorItem = new OperatorProduct();
                    operatorItem.brand = oprtor.Brand;
                    operatorItem.logoUrl = oprtor.LogoUrl;
                    operatorItem.name = oprtor.Name;
                    operatorItem.operatorID = oprtor.OperatorID;
                    operatorItem.Relevance = oprtor.Relevance;
                    operatorItem.RGBColor = oprtor.RGBColor;
                    operatorItem.operatorLogo = oprtor.OperatorLogo;
                    operatorItem.HexColor = oprtor.HEXColor;
                    operatorItem.rgbColor = new RGBColor();
                    operatorItem.Amounts = new List<OperatorAmounts>();

                    if (!String.IsNullOrEmpty(oprtor.RGBColor))
                    {
                        var codeColor = oprtor.RGBColor.Split(',');
                        operatorItem.rgbColor.R = int.Parse(codeColor[0].ToString());
                        operatorItem.rgbColor.G = int.Parse(codeColor[1].ToString());
                        operatorItem.rgbColor.B = int.Parse(codeColor[2].ToString());
                    }

                    foreach (var product in oprtor.Products)
                    {
                        OperatorAmounts amount = new OperatorAmounts();
                        amount.Amount = product.Amount;
                        amount.Code = product.Code;
                        amount.Description = product.Description;
                        amount.OperatorName = oprtor.Brand; //In order to create a response identical in version 1.0, property must be 'Brand'
                        amount.Relevance = product.Relevance;
                        operatorItem.Amounts.Add(amount);
                    }

                    operatorsList.Add(operatorItem);
                }

                operators.countryOperators = operatorsList;

                //Actual response
                response.operators = operators;
                response.count = operatorsList.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }
    }
}