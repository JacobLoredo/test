using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;
namespace Automation.Pages.CommonElements.Modales
{
    public class ModalCompraFinalizada:BasePage
    {
        public IWebDriver driver1;
        public ModalCompraFinalizada(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//*[@id='SaleChange']")]
        private IWebElement ModalCF { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='PrintReceipt']")]
        private IWebElement BtnPrintReceipt { get; set; }


        [FindsBy(How = How.XPath, Using = "//button[text()='Salir']")]
        private IWebElement BtnSalir { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[contains(text(),'FOLIO')]")]
        private IWebElement PFolioVenta { get; set; }

        public void WaitModalCobro()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='PrintReceipt']")));
        }
        public void ClicBtnPrintReceipt()
        {
            ActionsSelenium actions = new ActionsSelenium(driver1);
            wait.Until(ExpectedConditions.ElementToBeClickable(BtnPrintReceipt));
            actions.MoveToElement(BtnPrintReceipt).Pause(TimeSpan.FromMilliseconds(300)).Click().Build().Perform();
        }

        public void ClicBtnSalir()
        {
            ActionsSelenium actions = new ActionsSelenium(driver1);
            actions.MoveToElement(BtnSalir).Click().Build().Perform();
        }
        public string GetFolioVentaBusqueda() {
          return  PFolioVenta.Text.Replace("FOLIO:", "").Split('-')[1];
        }
        public string GetFolioVenta()
        {
            return PFolioVenta.Text.Replace("FOLIO:", "").Trim();
        }

    }
}
