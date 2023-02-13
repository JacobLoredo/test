using Automation.Pages.Modules.PuntoVenta.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.PuntoVenta
{
    public class SalesPointPage : BasePage
    {
        public SalesPointElements SalesPointEle { get; set; }
        public SalesPointPage(IWebDriver driver) : base(driver)
        {
            SalesPointEle = new SalesPointElements();
            PageFactory.InitElements(driver, SalesPointEle);
        }
    }
}
