using Automation.Pages.Modules.CatalogoClientes.PageElements;
using Automation.Pages.Modules.LocalizadorRefacciones.PageElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.CatalogoClientes
{
    public class CatClientesPage:BasePage
    {
        public CatClientesElements catClientesElements { get; set; }
        public CatClientesPage(IWebDriver driver) : base(driver) {
            catClientesElements = new CatClientesElements();
            PageFactory.InitElements(driver,catClientesElements);
        }
    }
}
