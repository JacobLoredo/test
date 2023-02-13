using Automation.Pages.Modules.Compras.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.Compras
{
    public class nuevoPedidoPage:BasePage
    {
        public NuevoPedidoElements NuevoPedidoElements { get; set; }
        public nuevoPedidoPage(IWebDriver driver) : base(driver)
        {
            NuevoPedidoElements = new NuevoPedidoElements();
            PageFactory.InitElements(driver, NuevoPedidoElements);
        }
    }
}
