using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.Inventario.PageElements
{
    public class InventoryElements
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='addSpareParts']")]
        public IWebElement BtnAddSpareParts { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='uploadSparepartFormat']")]
        public IWebElement BtnUpdloadSparePart { get; set; }

        public void ClicBtnAddSpareParts()
        {
            BtnAddSpareParts.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Nueva Refaccion'");
        }
        public void ClicBtnUpdloadSparePart()
        {
            BtnUpdloadSparePart.Click();
            Reporter.LogPassingTestStepForBugLogger($"Clic al boton  : 'Cargar Inventario inicial'");
        }

    }
}
