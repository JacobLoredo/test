using Automation.Pages.Modules.AutorizacionesOrdenesCompra.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.AutorizacionesOrdenesCompra
{
   public class autorizacionOrdenPage:BasePage
    {
        public AutoOrdenElements ordenElements { get; set; }
        public autorizacionOrdenPage(IWebDriver driver):base(driver) {

            ordenElements = new AutoOrdenElements();
            PageFactory.InitElements(driver, ordenElements);
        }
    }
}
