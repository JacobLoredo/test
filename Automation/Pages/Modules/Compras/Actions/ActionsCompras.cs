using Automation.Pages.CommonElements;
using Automation.Pages.Modules.CatalogoProvedores.PageElements;
using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Automation.Pages.Modules.Compras.Actions
{
    public class ActionsCompras:comprasPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public ActionsCompras(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        public void GoToPage()
        {
            Reporter.LogTestStepForBugLogger(Status.Info, "Ir a la pagina de login");
            Driver.Navigate().GoToUrl(config.UrlPage + "/Shopping");
            Reporter.LogPassingTestStepForBugLogger($"Abriendo URL=>{config.UrlPage + "/Shopping"}");

        }
        public bool IsComprasPage() {
            return ComprasElements.h1Titulo.Displayed;
        }
        public void ClickNewPedido() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{ComprasElements.btnNuevoPedido.Text}'");
            ComprasElements.btnNuevoPedido.Click();
        }
        public bool CheckEstatusOrden(string status, string FechaIni, string FechaFin,string idOrden) {
            OpenNewTab();
            GoToPage();
            SearchOrden(FechaIni,FechaFin);
            string statusOrder = ReturnDataRowOrden(OrdenRow(idOrden),"estatus");
            return statusOrder.Equals(status);
        }
        private string ReturnDataRowOrden(IWebElement DataRow, string ColumnName) {
            List<IWebElement> data = new List<IWebElement>(DataRow.FindElements(By.TagName("td")));

            switch (ColumnName.ToLower())
            {
                case ("folio"):
                    return data[0].Text;
                case ("fecha"):
                    return data[1].Text;
                case ("proveedor"):
                    return data[2].Text;
                case ("clave"):
                    return data[3].Text;
                case ("estatus"):
                    return data[4].Text;
                case ("total"):
                    return data[5].Text;
                case ("importe"):
                    return data[6].Text;
                default:
                    return "NoFound";  
            }

        }
        public void SearchOrden(string FechaIni, string FechaFin, [Optional] string NombreProv)
        {
            BuscardorOrdenes buscador = new BuscardorOrdenes(Driver);
            if (NombreProv != null)
                buscador.SearchOrden(FechaIni, FechaFin, NombreProv);
            else
                buscador.SearchOrden(FechaIni, FechaFin);

        }
        public IWebElement OrdenRow(string idOrden) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='AuthorizationTable']")));
            List<IWebElement> Pagination = new List<IWebElement>(Driver1.FindElements(By.XPath("//ul[@class='pagination']//li")));
            int numPag= Pagination.Count()-2;
            IWebElement Row=null;

            for (int i = 0; i < numPag; i++)
            {
                try
                {
                    Row = Driver1.FindElement(By.XPath("//table[@id='AuthorizationTable']//tr[@id='" + idOrden + "']"));
                    break;
                }
                catch (NoSuchElementException)
                {
                    IWebElement nextPagination = Driver1.FindElement(By.XPath("//ul[@class='pagination']//li[@id='AuthorizationTable_next']"));
                    ScrollToElement(nextPagination.Location.X, nextPagination.Location.Y,0,20);
                    nextPagination.Click();
                    ScrollToElement(0,0,0,0);
                    continue;
                }
            }
            if (Row==null)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, $"No se encontrao la orden con id '{idOrden}' en el listado de ordenes de compra");
            }
            return Row;


        }
        public void ClickVerOrden(string idOrden)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='AuthorizationTable_wrapper']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='AuthorizationTable_wrapper']"));
            try
            {
                Reporter.LogTestStepForBugLogger(Status.Info, $"Buscando orden en Autorizacion con el id '{idOrden}'");
                IWebElement RowOrden = table.FindElement(By.XPath("//tr[@id='" + idOrden + "']"));
                IWebElement btnVerOrden = RowOrden.FindElement(By.TagName("a"));
                ScrollToElement(btnVerOrden.Location.X, btnVerOrden.Location.Y, 0, 0);
                btnVerOrden.Click();
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            }
            catch (NoSuchElementException)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, $"No se encontro la orden con el id {idOrden}");
            }

        }

    }
}
