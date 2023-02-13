using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.Compras.PageElements
{
    public class NuevoPedidoElements
    {
        [FindsBy(How = How.XPath, Using = "//h1[@class='page-title']")]
        public IWebElement h1Titulo { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='searchBySku']")]
        public IWebElement inputBusqueda { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='searchArticlesContainer']/div/div/div[@class='tabs-wrap']/div/ul/li/a")]
        public IList<IWebElement> navBusquedaArticulo { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchEvent']")]
        public IWebElement btnBusquedaLupa { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchAdvanced']")]
        public IWebElement btnBusquedaAvanzada { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='providerSearch']")]
        public IWebElement btnAsignarProveedor { get; set; }

        [FindsBy(How = How.Id, Using = "By.Id('ejbeatycelledit')")]
        public IWebElement EditableInput2 { get; set; }
        #region Busqueda avanzada

        [FindsBy(How = How.XPath, Using = "//*[@id='searchAdvancedForm']/div/div[1]/div/label")]
            public IList<IWebElement> selectMarcaAvanzada { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='search']")]
            public IWebElement btnModalBuscar { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='cleanInputs']")]
            public IWebElement btnModalLimpiar { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='containerTableRender']/div/div/div[@class='title-table d-none d-md-block']")]
            public IWebElement titleTableResult { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='containerTableRender']/div/div[@class='icon-empty col-12 radar-malo-face']")]
            public IWebElement noFoundTableResult { get; set; }
            [FindsBy(How = How.XPath, Using = "//div[@id='searchAdvancedModalId']//button[@class='close']")]
            public IWebElement btnModalClose { get; set; }
        #endregion
    }
}
