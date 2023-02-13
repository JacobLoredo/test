using Automation.Pages.Modules.PuntoVenta.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.PuntoVenta
{
    public class HistorySalesPointPage : BasePage
    {
        public HistorySalesPointElements HistorySalesPointEle { get; set; }
        public HistorySalesPointPage(IWebDriver driver) : base(driver)
        {
            HistorySalesPointEle = new HistorySalesPointElements();
            PageFactory.InitElements(driver, HistorySalesPointEle);
        }
    }
}
