using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.AutorizacionesOrdenesCompra.PageElements
{
    public class AutoOrdenElements
    {
        [FindsBy(How = How.XPath, Using = "//h1[contains(text(),'Autorización de órdenes de compra')]")]
        public IWebElement h1Titulo { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//b[contains(text(),'El motivo de rechazo de la compra:')]")]
        public IWebElement bModalRechazo { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='change-status-modal']//button[contains(@class,'change-status-button')]")]
        public IWebElement btnModalRechazoAcept { get; set; }
    }
}
