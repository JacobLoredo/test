using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.Pages.CommonElements
{
    public class BuscardorOrdenes : BasePage
    {
        public IWebDriver driver1;

        public BuscardorOrdenes(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }
        public IWebElement SearchOptions { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='initDate']")]
        public IWebElement inputFechaIni { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='endDate']")]
        public IWebElement inputFechaFin { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='providerName']")]
        public IWebElement inputProveedor { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='buttonSearch']")]
        public IWebElement btnBuscar { get; set; }
        private void PutFechaInicio(string FechaIni)
        {
            Thread.Sleep(1000);
            Reporter.LogPassingTestStepForBugLogger($"Buscando orden con fecha inicio:  '{FechaIni}'");
            inputFechaIni.SendKeys(FechaIni);
        }
        private void PutFechaFinal(string FechaFin)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='endDate']")));
            Reporter.LogPassingTestStepForBugLogger($"Buscando orden con fecha final:  '{FechaFin}'");
            inputFechaFin.SendKeys(FechaFin);
        }
        private void PutNombreProveedor(string Nombre)
        {
            Reporter.LogPassingTestStepForBugLogger($"Buscando orden con Nombre de proveedor:  '{Nombre}'");
            inputProveedor.SendKeys(Nombre);
        }
        private void ClickBotonBuscar()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de buscar");
            btnBuscar.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }

        public void SearchOrden(string FechaIni, string FechaFin, [Optional] string NombreProv)
        {
            PutFechaInicio(FechaIni);
            PutFechaFinal(FechaFin);
            if (NombreProv != null)
                PutNombreProveedor(NombreProv);
            ClickBotonBuscar();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));

        }
    }
}
