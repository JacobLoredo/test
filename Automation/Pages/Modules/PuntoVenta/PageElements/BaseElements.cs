using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using Automation.Reports;
namespace Automation.Pages.Modules.PuntoVenta.PageElements
{
    public class BaseElements
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='GenerateOrder']")]
        public IWebElement BtnGenerateOrder { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[contains(@class,'status-span')]")]
        private IWebElement StatusOrder { get; set; }

        [FindsBy(How = How.XPath, Using = "//h4[contains(text(),'Folio')]")]
        private IWebElement Folio { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='update-terms']")]
        public IWebElement IconUpdateTerms { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='CancelOrder']")]
        public IWebElement BtnCancelOrder { get; set; }

        public void ClicBtnCancelOrder() {
            BtnCancelOrder.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Eliminar cotizacion'");
        }

        public void ClicBtnGenerateOrder()
        {
            BtnGenerateOrder.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Generar orden de venta'");
        }
        public void ClicUpdateTerms()
        {
            IconUpdateTerms.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Para actualizar terminos y condiciones ");
        }
        public string GetStatus()
        {
            return StatusOrder.Text.Trim();
        }
        public string GetFolio()
        {
            return Folio.Text.Replace("Folio:","").Trim();
        }
    }
}
