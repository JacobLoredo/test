using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.LocalizadorRefacciones.PageElements
{
    public class LocalizadorElements
    {
        [FindsBy(How = How.XPath, Using = "//h1[@class='page-title']")]
        public IWebElement h1Titulo { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@data-location-key='TOWN']")]
        public IWebElement FiltroMunicipio { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@data-location-key='STATE']")]
        public IWebElement FiltroEstado { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@data-location-key='ZONE']")]
        public IWebElement FiltroZona { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-location-key='REGION']")]
        public IWebElement FiltroRegion { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@data-location-key='COUNTRY']")]
        public IWebElement FiltroPais { get; set; }


    }
}
