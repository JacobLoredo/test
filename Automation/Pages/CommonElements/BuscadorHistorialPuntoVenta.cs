using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Collections.Generic;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;
namespace Automation.Pages.CommonElements
{
    public class BuscadorHistorialPuntoVenta : BasePage
    {
        public IWebDriver driver1;
        public BuscadorHistorialPuntoVenta(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='tabs-header']//a[@href='#salesOrders']")]
        public IWebElement TabSalesOrders { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='tabs-header']//a[@href='#quotations']")]
        public IWebElement TabQuotations { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='salesOrders']//button[@data-search-key]")]
        public IList< IWebElement> ListSearchOptions  { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='salesSearchBtn']")]
        public IWebElement BtnSalesSearch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='quotationSearchBtn']")]
        public IWebElement BtnQuotationSearch { get; set; }



        private void ClicTabSales()
        {
            ActionsSelenium actionsSelenium = new ActionsSelenium(driver1);
            actionsSelenium.MoveToElement(TabSalesOrders).Click().Build().Perform();
          
        }
        private void ClicTabQuotations() { 
            TabQuotations.Click();
        }
        private void PutFolioInQuotations(string folio) {
            IWebElement InputFolio = driver1.FindElement(By.XPath("//div[@id='quotations']//div[@id='quotationSearch-mask-container']/div/input[2]"));
            InputFolio.Clear();
            InputFolio.SendKeys(folio);
        }
        private void PutFolioInSales(string folio)
        {
            IWebElement InputFolio = driver1.FindElement(By.XPath("//div[@id='salesOrders']//div[@id='salesSearch-mask-container']/div/input[2]"));
            InputFolio.Clear();
            InputFolio.SendKeys(folio);
        }
        private void ClicBtnSearchQuotations()
        {
            BtnQuotationSearch.Click();
            WaitSpinner();
        }
        private void ClicBtnSalesSearch()
        {
            BtnSalesSearch.Click();
            WaitSpinner();
        }
        public void SearchQuotationByFolio(string Folio) {
            ClicTabQuotations();
            PutFolioInQuotations(Folio);
            ClicBtnSearchQuotations();
        }
        public void SearchSaleByFolio(string Folio)
        {
            ClicTabSales();
            PutFolioInSales(Folio);
            ClicBtnSalesSearch();
        }
    }
}
