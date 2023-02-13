using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.Compras.PageElements
{
    public class ComprasElements
    {
        [FindsBy(How = How.LinkText, Using = "Nuevo pedido")]
        public IWebElement btnNuevoPedido { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//h1[@class='page-title']")]
        public IWebElement h1Titulo { get; set; }



        [FindsBy(How = How.XPath, Using = "//*[@id='change-status-modal']//label[contains(text(),'Estatus Actual')]//b")]
        public IWebElement estatusActualLabelModal { get; set; }


    }
}
