using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
namespace Automation.Pages.Modules.PuntoVenta.PageElements
{
    public class SalesPointElements:BaseElements
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='keyCustomerSearch']")]
        public IWebElement InputCustomerKey { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='NameCustomerSearch']")]
        public IWebElement InputCustomerName { get; set; }
        #region
        [FindsBy(How = How.XPath, Using = "//*[@id='searchCustomerBtn']")]
        public IWebElement BtnCustomerSearch { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[text()='Corte de caja']")]
        public IWebElement BtnCorteCaja { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[text()='Consulta de saldos']")]
        public IWebElement BtnConsultaSaldos { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@onclick,'SalePoint/History')]")]
        public IWebElement BtnConsultaHistorial{ get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='show-terms']")]
        public IWebElement BtnShowTerms { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@id='cancelSaleCart']")]
        public IWebElement BtnCancelSaleCart { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[text()='Cotizar']")]
        public IWebElement BtnCotizar { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='create-sale']")]
        public IWebElement BtnCobrar { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchBySKUButton']")]
        public IWebElement BtnSearchBySKU { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchBySkuNoAutocomplete']")]
        public IWebElement InputSearchBySKU { get; set; }

        #endregion

        public void PutClientKey(string ClientKey) {
            InputCustomerKey.Clear();
            InputCustomerKey.SendKeys(ClientKey);
            Reporter.LogPassingTestStepForBugLogger($"Buscando cliente con clave : '{ClientKey}'");
        }

        public void PutCustomerName(string CustomerName)
        {
            InputCustomerName.SendKeys(CustomerName);
            Reporter.LogPassingTestStepForBugLogger($"Buscando cliente con Nombre : '{CustomerName}'");
        }
        public void ClicBtnCustomerSearch()
        {
            BtnCustomerSearch.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Buscar' de cliente ");
        }

        public void ClicBtnCorteCaja()
        {
            BtnCorteCaja.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Corte de caja ");
        }

        public void ClicBtnConsultarSaldos()
        {
            BtnConsultaSaldos.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Consultar saldos ");
        }

        public void ClicBtnConsultarHistorial()
        {
            BtnConsultaHistorial.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Consultar Historial ");
        }

        public void ClicShowTerm()
        {
            BtnShowTerms.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Terminos y condiciones ");
        }



        public void PutSKUSearch(string sku)
        {
            InputSearchBySKU.SendKeys(sku);
            Reporter.LogPassingTestStepForBugLogger($"Buscando el SKU '{sku}' ");
        }

        public void ClicCancelSaleCart()
        {
            BtnCancelSaleCart.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Cancelar ");
        }

        public void ClicBtnCotizar()
        {
            BtnCotizar.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Cotizar ");
        }

        public void ClicBtnCobrar()
        {
            BtnCobrar.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Cobrar ");
        }

        public void ClicBtnSearchBySku()
        {
            BtnSearchBySKU.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Cobrar ");
        }
    }
}
