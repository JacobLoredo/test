using Automation.Pages.Modules.Compras.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.Compras
{
    public class comprasPage:BasePage
    {
        public ComprasElements ComprasElements { get; set; }
        public comprasPage(IWebDriver driver) : base(driver)
        {
            ComprasElements= new ComprasElements();
            PageFactory.InitElements(driver,ComprasElements);
        }
    }
}
