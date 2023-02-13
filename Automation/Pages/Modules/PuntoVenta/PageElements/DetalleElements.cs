using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Collections.Generic;
using System.Globalization;

namespace Automation.Pages.Modules.PuntoVenta.PageElements
{
    public class DetalleElements:BaseElements
    {

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'SparePartsCount')]")]
        private IWebElement SparePartsCount { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'ArticlesCount')]")]
        private IWebElement ArticlesCount { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'Currency')]")]
        private IWebElement Currency { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'PercentageDiscount')]")]
        private IWebElement PercentageDiscount { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'Amount')]")]
        private IWebElement Amount { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'Discounts')]")]
        private IWebElement Discounts { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'Taxes')]")]
        private IWebElement Taxes { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@name,'AmountToPay')]")]
        private IWebElement AmountToPay { get; set; }

        public string ConvertMoneyFormatToFloat(string amount) {
            return float.Parse(amount.Replace('$', ' '), CultureInfo.InvariantCulture).ToString("0.00");
        }
        private string GetSparePartsCount() {
            return SparePartsCount.Text;
        }
        private string GetArticlesCount()
        {
            return ArticlesCount.Text;
        }
        private string GetCurrency()
        {
            return Currency.Text;
        }
        private string GetPercentageDiscount()
        {
            return PercentageDiscount.Text.Replace('%',' ').Trim();
        }
        private string GetAmount()
        {
            return ConvertMoneyFormatToFloat(Amount.Text);
        }
        private string GetDiscounts()
        {
            return ConvertMoneyFormatToFloat(Discounts.Text);
        }
        private string GetTaxes()
        {
            return ConvertMoneyFormatToFloat(Taxes.Text);
        }
        private string GetAmountToPay()
        {
            return ConvertMoneyFormatToFloat(AmountToPay.Text);
        }
        public Dictionary<string, object> GetInfoQuotation() {
            Dictionary<string, object> DetailsPedido = new Dictionary<string, object>();
            DetailsPedido.Add("SKU's", GetSparePartsCount());
            DetailsPedido.Add("Articulo", GetArticlesCount());
            DetailsPedido.Add("Moneda", GetCurrency());
            DetailsPedido.Add("Descuento%", GetPercentageDiscount());
            DetailsPedido.Add("Importe", GetAmount());
            DetailsPedido.Add("DescuentoMon", GetDiscounts());
            DetailsPedido.Add("Subtotal", GetTaxes());
            DetailsPedido.Add("Total", GetAmountToPay());

            return DetailsPedido;

        }
    }
}
