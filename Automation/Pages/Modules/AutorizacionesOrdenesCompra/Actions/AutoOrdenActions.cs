using Automation.Pages.CommonElements;
using Automation.Pages.Modules.AutorizacionesOrdenesCompra.PageElements;
using Automation.Pages.Modules.Compras.Actions;
using Automation.Pages.Modules.Compras.PageElements;
using Automation.Reports;
using AventStack.ExtentReports;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.AutorizacionesOrdenesCompra.Actions
{
    public class AutoOrdenActions : autorizacionOrdenPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public AutoOrdenActions(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        public void GoToPage()
        {
            Reporter.LogTestStepForBugLogger(Status.Info, "Ir a la pagina de login");
            Driver.Navigate().GoToUrl(config.UrlPage + "/Authorizations");
            Reporter.LogPassingTestStepForBugLogger($"Abriendo URL=>{config.UrlPage + "/Shopping/NewOrder"}");

        }
        public bool IsAutoOrdenPage()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[@class='page-title']")));
            return ordenElements.h1Titulo.Text.Equals("Autorización de órdenes de compra");
        }


        public void SearchOrden(string FechaIni, string FechaFin, [Optional] string NombreProv) {
            BuscardorOrdenes buscador = new BuscardorOrdenes(Driver);
            if (NombreProv != null)
                buscador.SearchOrden(FechaIni, FechaFin, NombreProv);
            else
                buscador.SearchOrden(FechaIni, FechaFin);
        }
        public void ClickVerOrden(string idOrden) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='AuthorizationTable_wrapper']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='AuthorizationTable_wrapper']"));
            List<IWebElement> Pagination = new List<IWebElement>(Driver1.FindElements(By.XPath("//ul[@class='pagination']//li")));
            int numPag = Pagination.Count() - 2;
            Pagination.Remove(Pagination.First());
            Pagination.Remove(Pagination.Last());
            Pagination[0].Click();
            for (int i = 0; i < numPag; i++)
            {

                try
                {
                    Reporter.LogTestStepForBugLogger(Status.Info, $"Buscando orden en Autorizacion con el id '{idOrden}'");
                    Thread.Sleep(500);
                    IWebElement RowOrden = table.FindElement(By.XPath("//tr[@id='" + idOrden + "']"));
                    IWebElement btnVerOrden = RowOrden.FindElement(By.TagName("a"));
                    ScrollToElement(btnVerOrden.Location.X, btnVerOrden.Location.Y, 0, 0);
                    btnVerOrden.Click();
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                    Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                }
                catch (NoSuchElementException)
                {
                    IWebElement nextPagination = Driver1.FindElement(By.XPath("//ul[@class='pagination']//li[@id='AuthorizationTable_next']"));
                    ScrollToElement(nextPagination.Location.X, nextPagination.Location.Y, 0, 20);
                    nextPagination.Click();
                    ScrollToElement(0, 0, 0, 0);
                    continue;
                    //Reporter.LogTestStepForBugLogger(Status.Fail, $"No se encontro la orden con el id {idOrden}");
                }
            }

        }
        public void CheckVerOrden(string idOrden, string fechaInicio, string fechaFin) {
            OpenNewTab();
            GoToPage();
            SearchOrden(fechaInicio, fechaFin);
            ClickVerOrden(idOrden);

            //CloseTab();
        }
        public bool ValidaClaveYFecha(string clave, string fecha) {
            bool valid = false;

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            try
            {
                Thread.Sleep(450);
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[contains(text(),'" + clave + "')]")));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[contains(text(),'" + fecha + "')]")));
                IWebElement Clave = Driver1.FindElement(By.XPath("//p[contains(text(),'" + clave + "')]"));
                IWebElement fechaa = Driver1.FindElement(By.XPath("//p[contains(text(),'" + fecha + "')]"));

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, $"No coinciden la clave '{clave}' y la fecha '{fecha}'");
                return valid;
            }
        }
        public int NumArticulosDetalle() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//table[@id='ArticleOrderTable']")));
            IWebElement TableArticles = Driver1.FindElement(By.XPath("//table[@id='ArticleOrderTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TableArticles.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            return ListRowArti.Count;

        }

        private void SelectRandRefasInPedido()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//table[@id='ArticleOrderTable']")));
            IWebElement TableArticles = Driver1.FindElement(By.XPath("//table[@id='ArticleOrderTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TableArticles.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            List<string> refasAgregadas = new List<string>();
            Random rng = new Random();
            int numRefa = rng.Next(1, ListRowArti.Count - 1);
            int i = 0;
            while (numRefa > 0 && i < ListRowArti.Count)
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                if (!ListRowArti[i].GetAttribute("class").Contains("selected"))
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                    List<IWebElement> ListElementsArticle = new List<IWebElement>(ListRowArti[i].FindElements(By.TagName("td")));
                    refasAgregadas.Add(ListElementsArticle[1].Text);
                    ListElementsArticle[0].Click();
                    i++;
                    numRefa--;
                }
            }
            Reporter.LogTestStepForBugLoggerJSON(Status.Info, refasAgregadas.ToJson().ToString());
        }
        private void SelectAllRefas() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//table[@id='ArticleOrderTable']")));
            IWebElement labelSelectAll = Driver1.FindElement(By.XPath("//*[@id='ArticleOrderTable']/thead/tr/th/label"));
            labelSelectAll.Click();
        }
        public void ClickAccionOrden(string accion, [Optional] string comentario) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> ContenedorBtn = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[@id='content']//button")));

            if (accion.Equals("Autorizar completa"))
            {
                ContenedorBtn[1].Click();
            }
            else if (accion.Equals("Rechazar")) {
                ContenedorBtn[0].Click();
                clickSwal2Button("Aceptar");
                PutComentarioSwal2(comentario);

            }
            else if (accion.Equals("Parcial"))
            {
                ContenedorBtn[1].Click();
                if (comentario != null)
                    PutComentarioSwal2(comentario);
                clickSwal2Button("Aceptar");
            }

        }
        public void AutorizarOrden(string TipoAutorizacion, [Optional] string comentario) {
            if (TipoAutorizacion.Equals("Completa"))
            {
                SelectAllRefas();
                ClickAccionOrden("Autorizar completa");
                clickSwal2Button("Aceptar");
            }
            else if (TipoAutorizacion.Equals("Parcial"))
            {
                SelectRandRefasInPedido();
                ClickAccionOrden("Parcial", comentario);
            }

        }
        public void RechazarOrden(string comentario)
        {
            ClickAccionOrden("Rechazar", comentario);
            clickSwal2Button("Aceptar");

        }
        public List<string> ObtenerSkuRefasDetalle() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//table[@id='ArticleOrderTable']")));
            IWebElement TableArticles = Driver1.FindElement(By.XPath("//table[@id='ArticleOrderTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TableArticles.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);

            List<string> refasAgregadas = new List<string>();

            foreach (IWebElement Refa in ListRowArti)
            {
                string id = Refa.GetAttribute("data-sku");
                refasAgregadas.Add(id);
            }

            return refasAgregadas;

        }
        public void CrearODC() {
            int numArticulos = 5;
            ActionsNuevoPedido actionsNuevoPedido = new ActionsNuevoPedido(Driver1);
            ActionsCompras actionsCompras = new ActionsCompras(Driver1);
            List<string> infoOrdenPedido = actionsNuevoPedido.CreaPedido("XAXX010101001", "Central Fijo Italika", numArticulos);
            string fecha = ObtenerFechaActual();
            actionsCompras.GoToPage();
            //Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            SearchOrden(fecha, fecha);
            ClickVerOrden(infoOrdenPedido[1]);

        }
        public void CrearYAutorizarODC() {
            int numArticulos = 5;
            ActionsNuevoPedido actionsNuevoPedido = new ActionsNuevoPedido(Driver1);
            ActionsCompras actionsCompras = new ActionsCompras(Driver1);
            List<string> infoOrdenPedido = actionsNuevoPedido.CreaPedido("XAXX010101001", "Central Fijo Italika", numArticulos);
            string fecha = ObtenerFechaActual();
            SearchOrden(fecha, fecha);
            ClickVerOrden(infoOrdenPedido[1]);
            AutorizarOrden("Completa");
            actionsCompras.CheckEstatusOrden("Autorizado", fecha, fecha, infoOrdenPedido[1]);
            ClickVerOrden(infoOrdenPedido[1]);
        }
        public void CrearYRechazarODC() {
            int numArticulos = 5;
            ActionsNuevoPedido actionsNuevoPedido = new ActionsNuevoPedido(Driver1);
            ActionsCompras actionsCompras = new ActionsCompras(Driver1);
            List<string> infoOrdenPedido = actionsNuevoPedido.CreaPedido("XAXX010101001", "Central Fijo Italika", numArticulos);
            string fecha = ObtenerFechaActual();
            SearchOrden(fecha, fecha);
            ClickVerOrden(infoOrdenPedido[1]);
            RechazarOrden("Esto es un rechazon de QA");
            actionsCompras.CheckEstatusOrden("Rechazado", fecha, fecha, infoOrdenPedido[1]);
            ClickVerOrden(infoOrdenPedido[1]);
        }
        public bool CheckEstatusActual(string status) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> spansStatus = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='content']/div[1]/div[1]/span")));
            Reporter.LogPassingTestStepForBugLogger($"Verificando estatus: '{status}'");
            switch (status.ToLower())
            {
                case ("autorizado"):
                    return (spansStatus.Count == 2) && (spansStatus[0].Text.Trim().Equals(status));
                case ("rechazado"):
                    return (spansStatus.Count == 2) && (spansStatus[0].Text.Trim().Equals(status));
                case ("cancelada por usuario"):
                    return (spansStatus.Count == 1) && (spansStatus[0].Text.Trim().Equals(status));
                case ("pendiente de autorización"):
                    return (spansStatus.Count == 1) && (spansStatus[0].Text.Trim().Equals(status));
                case ("en tránsito"):
                    return (spansStatus.Count == 2) && (spansStatus[0].Text.Trim().Equals(status));
                default:
                    return false;
            }
        }
        public bool CheckStatusDisponibles(string statusActual) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> NuevosStatus = new List<IWebElement>();
            List<string> nuevosEstatusList=new List<string>();
            if (!statusActual.Equals("Rechazado")&& !statusActual.Equals("Pendiente de autorización"))
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='change-status-modal']")));
                NuevosStatus = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='change-status-modal']//input[@type='radio']")));
                nuevosEstatusList = ReturnStatusAvailable(NuevosStatus);
            }
            switch (statusActual.ToLower())
            {
                case ("autorizado"):
                    return nuevosEstatusList.Contains("Cancelada por usuario") && nuevosEstatusList.Contains("En tránsito");
                case ("rechazado"):
                    return IsOrderRefuse();
                case ("en tránsito"):
                    return nuevosEstatusList.Contains("Ingreso a inventario") && nuevosEstatusList.Contains("En aclaración") && nuevosEstatusList.Contains("Parcial en aclaración");
                default:
                    return false;
                   
            }
        }
        private List<string> ReturnStatusAvailable(List<IWebElement> NuevosStatus) 
        {
            List<string> nuevosEstatusList = new List<string>();
            foreach (IWebElement item in NuevosStatus)
            {
                string status = item.GetAttribute("data-text"); 
                nuevosEstatusList.Add(status);
            }
           return nuevosEstatusList;
        }
        public bool ChangeStatus(string newStatus) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> NuevosStatus = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='change-status-modal']//input[@type='radio']")));
            List<string> nuevosEstatusList = ReturnStatusAvailable(NuevosStatus);
            bool click =false;
            if (nuevosEstatusList.Contains(newStatus))
            {
                try
                {
                    IWebElement labelOption = Driver1.FindElement(By.XPath("//label[contains(text(),'" + newStatus + "')]"));
                    labelOption.Click();
                    click = true;   
                }
                catch (NoSuchElementException)
                {

                    return click;
                }

            }
            return click;

        }
        public void ClickChanceStatus() {

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='content']/div[1]/div[1]/span")));
            IWebElement spanStatusEditar = Driver1.FindElements(By.XPath("//*[@id='content']/div[1]/div[1]/span"))[1];
            try
            {
                spanStatusEditar.Click();
            }
            catch (StaleElementReferenceException)
            {

                spanStatusEditar.Click();
            }
           
            Reporter.LogTestStepForBugLogger(Status.Info,"Abriendo modal para cambiar estatus");
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        public void ClickAceptChangeStatusBtn() {
            ordenElements.btnModalRechazoAcept.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        public bool IsOrderRefuse() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='rejection-cause-modal']")));
            return ordenElements.bModalRechazo.Displayed;

        }

    }
}
