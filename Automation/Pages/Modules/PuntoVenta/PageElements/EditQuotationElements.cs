using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.PuntoVenta.PageElements
{
    public class EditQuotationElements:BaseElements
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='BtnEditDetails']")]
        public IWebElement BtnEditQuotation { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='CancelOrder']")]
        public IWebElement BtnCancelQuotation { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='BtnSaveEdition']")]
        public IWebElement BtnSaveEditionQuotation { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='keyCustomerSearch']")]
        public IWebElement InputCustomerKey { get; set; }

        public void ClicBtnSaveEditionQuotation() {
            BtnSaveEditionQuotation.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Guardar edicion de cotizacion");
        }

        public void ClicBtnCancelQuotation()
        {
            BtnCancelQuotation.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Eliminar cotizacion'");
        }
        public void ClicBtnEditQuotation()
        {
            BtnEditQuotation.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Editar cotizacion'");
        }
        public void PutClientKey(string ClientKey)
        {
            InputCustomerKey.Clear();
            InputCustomerKey.SendKeys(ClientKey);
            Reporter.LogPassingTestStepForBugLogger($"Buscando cliente con clave : '{ClientKey}'");
        }
    }
}
