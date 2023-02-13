using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.CommonElements
{
    public class BuscadorCliente : BasePage
    {
        public IWebDriver driver1;

        public BuscadorCliente(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//button[@data-search-key='CUSTOMERNAME']")]
        public IWebElement btnSearchByName { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[@data-search-key='CUSTOMERID']")]
        public IWebElement btnSearchByID { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[@data-search-key='RFC']")]
        public IWebElement btnSearchByRFC { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchInput']")]
        public IWebElement InputSearch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchBtn']")]
        public IWebElement BtnSearch { get; set; }


        private void PutSearch(string busqueda)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='searchInput']")));
            Reporter.LogPassingTestStepForBugLogger($"Buscando orden con fecha final:  '{busqueda}'");
            InputSearch.SendKeys(busqueda);
        }
        private void ClickBotonBuscar()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de buscar");
            BtnSearch.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        private void ClickBusquedaNombre()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de busqueda por: '{btnSearchByName.Text}'");
            btnSearchByName.Click();
        }
        private void ClickBusquedaID()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de busqueda por: '{btnSearchByID.Text}'");
            btnSearchByID.Click();
        }
        private void ClickBusquedaRFC()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de busqueda por: '{btnSearchByRFC.Text}'");
            btnSearchByRFC.Click();
        }

        public void SearchClientName(string name) {
            ClickBusquedaNombre();
            PutSearch(name);
            ClickBotonBuscar();


        }

    }
}
