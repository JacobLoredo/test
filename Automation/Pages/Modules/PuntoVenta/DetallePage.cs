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
    public class DetallePage:BasePage
    {
        public DetalleElements DetalleEle { get; set; }
        public DetallePage(IWebDriver driver) : base(driver)
        {
           DetalleEle = new DetalleElements();
            PageFactory.InitElements(driver, DetalleEle);
        }
    }
}
