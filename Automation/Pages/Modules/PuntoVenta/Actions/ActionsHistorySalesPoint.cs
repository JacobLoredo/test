using Automation.Pages.CommonElements;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;

namespace Automation.Pages.Modules.PuntoVenta.Actions
{
    public class ActionsHistorySalesPoint:HistorySalesPointPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public ActionsHistorySalesPoint(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }

        public bool LookQuotationByFolio(string Folio)
        {
            string FolioSub = Folio.Substring(7, 7);
            BuscadorHistorialPuntoVenta buscadorHistorial = new BuscadorHistorialPuntoVenta(Driver1);
            buscadorHistorial.SearchQuotationByFolio(FolioSub);
            try
            {
                IWebElement RowQuotation = Driver1.FindElement(By.XPath("//*[@data-folio='" + Folio + "']"));
                IWebElement BtnSeeQuoatation = RowQuotation.FindElement(By.TagName("a"));
                ActionsSelenium actions = new ActionsSelenium(Driver);
                wait.Until(ExpectedConditions.ElementToBeClickable(BtnSeeQuoatation));
                actions.MoveToElement(BtnSeeQuoatation).Click().Build().Perform();
                WaitSpinner();
                return true;
            }
            catch (System.Exception)
            {
                return false;

            }
           

        }

        public void LookSaleByFolio(string Folio)
        {
            string FolioSub = Folio.Substring(7, 8);
            BuscadorHistorialPuntoVenta buscadorHistorial = new BuscadorHistorialPuntoVenta(Driver1);
            buscadorHistorial.SearchSaleByFolio(FolioSub);
            IWebElement RowQuotation = Driver1.FindElement(By.XPath("//*[@data-folio='" + Folio + "']"));
            RowQuotation.FindElement(By.TagName("a")).Click();
            WaitSpinner();

        }
    }
}
