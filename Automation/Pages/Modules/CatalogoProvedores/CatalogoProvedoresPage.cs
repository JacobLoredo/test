using Automation.Pages.Modules.CatalogoProvedores.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.CatalogoProvedores
{
    public class CatalogoProvedoresPage : BasePage
    {
        public CatProvElements CatProvElements { get; set; }
        public CatalogoProvedoresPage(IWebDriver driver) : base(driver)
        {
            CatProvElements = new CatProvElements();
            PageFactory.InitElements(driver, CatProvElements);

        }

    }
}
