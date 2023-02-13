using Automation.Pages.Modules.Compras.PageElements;
using Automation.Pages.Modules.Inventario.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.Inventario
{
    public class InventoryPage : BasePage
    {
        public InventoryElements InventoryEle { get; set; }
        public InventoryPage(IWebDriver driver) : base(driver)
        {
            InventoryEle = new InventoryElements();
            PageFactory.InitElements(driver, InventoryEle);
        }
    }
}
