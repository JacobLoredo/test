

using Automation.Pages.Modules.PuntoVenta.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Automation.Pages.Modules.PuntoVenta
{
    public class EditQuotationPage:BasePage
    {
        public EditQuotationElements EditQuotationEle { get; set; }
        public EditQuotationPage(IWebDriver driver) : base(driver)
        {
            EditQuotationEle = new EditQuotationElements();
            PageFactory.InitElements(driver, EditQuotationEle);
        }
    }
}
