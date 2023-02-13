using Automation.Pages.Modules.LocalizadorRefacciones.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.LocalizadorRefacciones
{
    public class LocalizadorPage:BasePage
    {
        public LocalizadorElements LocalizadorElements { get; set; }
        public LocalizadorPage(IWebDriver driver) : base(driver) {

            LocalizadorElements = new LocalizadorElements();
            PageFactory.InitElements(driver, LocalizadorElements);
        }

    }
}
