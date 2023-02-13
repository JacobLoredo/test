using Automation.Pages.Modules.Compras.PageElements;
using Automation.Reports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using AventStack.ExtentReports;
using System.Threading;
using MongoDB.Bson;
using System.Runtime.InteropServices;
using System.Linq;
using MongoDB.Driver;
using System.Globalization;
using System.Xml.Linq;

namespace Automation.Pages.Modules.Compras.Actions
{
    public class ActionsNuevoPedido : nuevoPedidoPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }

        public ActionsNuevoPedido(IWebDriver driver) : base(driver) {
            Driver = driver;

        }
        public void GoToPage() {
            Reporter.LogTestStepForBugLogger(Status.Info, "Ir a la pagina de login");
            Driver.Navigate().GoToUrl(config.UrlPage+ "/Shopping/NewOrder");
            Reporter.LogPassingTestStepForBugLogger($"Abriendo URL=>{config.UrlPage + "/Shopping/NewOrder"}");

        }
        public bool IsNuevoPedidoPage()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[@class='page-title']")));
            return NuevoPedidoElements.h1Titulo.Text.Equals("Nuevo pedido");
        }
        public void ClickBtnBA() {

            Reporter.LogPassingTestStepForBugLogger($"Click al boton: '{NuevoPedidoElements.btnBusquedaAvanzada.Text}'");
            ScrollToElement(0,0,0,0);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='searchAdvanced']")));
            NuevoPedidoElements.btnBusquedaAvanzada.Click();
        }
        public void ClickModalBtnBuscarBA()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='search']")));
            Reporter.LogPassingTestStepForBugLogger($"Click al boton: 'Buscar'");
            NuevoPedidoElements.btnModalBuscar.Click();
        }
        public void ClickModalBtnLimpiaBA()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton: '{NuevoPedidoElements.btnModalLimpiar.Text}'");
            NuevoPedidoElements.btnModalLimpiar.Click();
        }
        public void PutMarcaBA(string marca) {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='searchAdvancedModalId']")));
            Thread.Sleep(4000);
            foreach (IWebElement marcaBA in NuevoPedidoElements.selectMarcaAvanzada)
            {
                if (marcaBA.Text == marca.ToUpper())
                {
                    marcaBA.Click();
                    Reporter.LogPassingTestStepForBugLogger($"Seleccionando la marca: '{marcaBA.Text}'");

                    break;
                }
            }
        }
        public void PutTipoBA(string Tipo) {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='TypeFamilyId']")));
            SelectElement TipoSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='TypeFamilyId']")));
            Thread.Sleep(2500);
            TipoSelect.SelectByText(Tipo);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el tipo de equipo  '{Tipo}'");

        }
        public void PutClindrajeBA(string cilindraje)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='CylinderCapacityId']")));
            SelectElement CilindrajeSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='CylinderCapacityId']")));

            CilindrajeSelect.SelectByText(cilindraje);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el tipo de cilindraje  '{cilindraje}'");

        }
        public void PutAnioBA(string Anio)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='YearId']")));
            SelectElement anioSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='YearId']")));

            anioSelect.SelectByText(Anio);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el año del equipo:  '{Anio}'");

        }
        public void PutModeloBA(string Modelo)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ModelVehicleIdId']")));
            SelectElement ModeloSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='ModelVehicleIdId']")));

            ModeloSelect.SelectByText(Modelo);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el modelo del vehiculo:  '{Modelo}'");

        }
        public void PutColorBA(string Modelo)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ColorId']")));
            SelectElement ModeloSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='ColorId']")));

            ModeloSelect.SelectByText(Modelo.ToUpper());
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el modelo del vehiculo:  '{Modelo}'");

        }
        public void PutSistemaBA(string Sistema)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='SystemId']")));
            SelectElement SistemaSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='SystemId']")));
            Thread.Sleep(1000);
            SistemaSelect.SelectByText(Sistema);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el modelo del vehiculo:  '{Sistema}'");

        }
        public void PutSubSistemaBA(string SubSistema)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='SubSystemId']")));
            SelectElement SubSistemaSelect = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='SubSystemId']")));

            SubSistemaSelect.SelectByText(SubSistema);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el modelo del vehiculo:  '{SubSistema}'");

        }
        public bool IsResultVisible() {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='containerTableRender']/div/div/div[@class='title-table d-none d-md-block']")));
            return NuevoPedidoElements.titleTableResult.Displayed;
        }
        public void PutInputSearch(string SKU) {
            Reporter.LogPassingTestStepForBugLogger($"Buscando con :  '{SKU}'");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='searchBySku']")));
            Thread.Sleep(2000);
            NuevoPedidoElements.inputBusqueda.SendKeys(SKU);
        }
        public void ClickBtnBusLupa()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton: '{NuevoPedidoElements.btnBusquedaLupa.Text}'");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='searchEvent']")));
            Thread.Sleep(3000);
            NuevoPedidoElements.btnBusquedaLupa.Click();
        }
        public bool NoFoundIsVisible() {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='containerTableRender']/div/div[@class='icon-empty col-12 radar-malo-face']")));
            return NuevoPedidoElements.noFoundTableResult.Displayed;
        }
        public void searchSKUDirect(string SKU) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='searchEvent']")));
            PutInputSearch(SKU);
            ClickBtnBusLupa();
        }
        public void FillBusquedaAvanzada(string marca, string tipo, string cilindrage, string anio, string Modelo, string Color, string Sistema, string subsistema) {
            Reporter.LogTestStepForBugLogger(Status.Info, "Llenando formulario de busqueda avanzada");
            PutMarcaBA(marca);
            Thread.Sleep(2000);
            PutTipoBA(tipo);
            PutClindrajeBA(cilindrage);
            PutAnioBA(anio);
            PutModeloBA(Modelo);
            PutColorBA(Color);
            PutSistemaBA(Sistema);
            PutSubSistemaBA(subsistema);
            ClickModalBtnBuscarBA();

        }

        public bool VerificarEncabezadosBA() {
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ArticleListTable']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='ArticleListTable']"));
            List<IWebElement> ListEncabezados = new List<IWebElement>(table.FindElements(By.TagName("th")));
            Reporter.LogPassingTestStepForBugLogger($"Verificando las columnas, son un total de: '{ListEncabezados.Count}'");
            if (ListEncabezados.Count == 6)
            {
                for (int i = 0; i < ListEncabezados.Count; i++)
                {
                    string tituloEnc = ListEncabezados[i].Text;
                    switch (i)
                    {
                        case 0:
                            if (tituloEnc != "SKU")
                                return false;
                            break;
                        case 1:
                            if (tituloEnc != "Descripción")
                                return false;
                            break;
                        case 2:
                            if (tituloEnc != "Detalles")
                                return false;
                            break;
                        case 3:
                            if (tituloEnc != "Stock")
                                return false;
                            break;
                        case 4:
                            if (tituloEnc != "Cant. a comprar")
                                return false;
                            break;
                        case 5:
                            if (tituloEnc != "")
                                return false;
                            break;
                        default:
                            return false;
                    }
                }
            }
            return ListEncabezados.Count == 6;
        }
        public IWebElement ObtenRowRefaBusqueda(string SKU) {
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ArticleListTable']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='ArticleListTable']"));
            IWebElement refa = null;
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);
            foreach (IWebElement Row in ListRow)
            {
                string idSKU = Row.GetAttribute("id");
                if (SKU.Equals(idSKU))
                {
                    refa = Row;
                    break;
                }
            }
            return refa;
        }

        public void ObtenerFilaResultadoB(string accion, string SKU) {
            IWebElement refa = ObtenRowRefaBusqueda(SKU);
            if (accion.Equals("Detalles"))
            {
                VerDetallesRefa(refa);
            }
            else if (accion.Equals("Agregar"))
            {
                AgregarAListaProduc(refa, 1);
            }
        }
        private int generateRandomIndex(int numMax) {
            Random rnd = new Random();
            int index = rnd.Next(0, numMax - 1);
            return index;
        }
        public void CrearPedido(string RCFProvedor, int cantidadRefas, int cantidadPedido) {
            searchSKUDirect("123");
            AddRefaRandomToListArticle(cantidadRefas);

            SelectRefasRandom(cantidadPedido);
            AsignarProveedor(RCFProvedor);


        }
        private void AddRefaRandomToListArticle(int cantidadArti)
        {
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ArticleListTable']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='ArticleListTable']"));
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);
            while (cantidadArti > 0)
            {
                int randomIndex = generateRandomIndex(ListRow.Count);
                int randomCant = generateRandomIndex(10);
                AgregarAListaProduc(ListRow[randomIndex], randomCant + 1);
                cantidadArti--;
            }
            NuevoPedidoElements.btnModalClose.Click();
        }
        public void closeModalBA() {
            NuevoPedidoElements.btnModalClose.Click();
        }
        public void AsignarProveedor(string RFC)
        {
            ClickBtnAsignarProveedor();
            CaptureRFCProvedor(RFC);

        }
        public void SelectSearchProvider(string TypeSearch) {
            List<IWebElement> ListbtnSearch = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ProviderModalSelection']//div[contains(@class,'searchOptions')]//button")));

            foreach (IWebElement btn in ListbtnSearch)
            {
                if (btn.Text.Equals(TypeSearch))
                {
                    btn.Click();
                    Thread.Sleep(300);
                    break;
                }
            }
        }
        private void CaptureRFCProvedor(string RFC) {
            waitElementToClick(by: By.XPath("//*[@id='searchInput']"));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='searchBtn']")));
            IWebElement btnBuscarProvedor = Driver1.FindElement(By.XPath("//*[@id='searchBtn']"));
            SelectSearchProvider("RFC");
            searchInput.SendKeys(RFC);
            btnBuscarProvedor.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='contentSearch']")));
            IWebElement containerProv = Driver1.FindElement(By.XPath("//*[@id='contentSearch']"));
            Thread.Sleep(1000);
            containerProv.FindElement(By.TagName("button")).Click();
            Reporter.LogPassingTestStepForBugLogger($"Agregando pedido al provedor con RFC '{RFC}'");
        }
        private void SelectRefasRandom(int numRefas) {
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ArticleTable']/tbody")));
            List<IWebElement> table = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ArticleTable']/tbody//tr")));
            SelectNavPedidoByOption(0);
            Random rng = new Random();
            List<IWebElement> listShuffle = table.OrderBy(a => rng.Next()).ToList();
            List<string> refasAgregadas = new List<string>();

            int i = 0;
            while (numRefas > 0 && i < listShuffle.Count)
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                Thread.Sleep(400);
                try
                {
                    
                    string aa = listShuffle[i].GetAttribute("class");
                    if (!listShuffle[i].GetAttribute("class").Contains("selected"))
                    {
                        IList<IWebElement> ListElementsArticle = new List<IWebElement>(listShuffle[i].FindElements(By.TagName("td")));
                        refasAgregadas.Add(ListElementsArticle[1].Text);
                        OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
                        ScrollToElement(ListElementsArticle[0].Location.X, ListElementsArticle[0].Location.Y, 0, 70);
                        ListElementsArticle[0].Click();
                        numRefas--;
                        i++;
                    }
                }
                catch (StaleElementReferenceException a)
                {
                    table = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ArticleTable']/tbody//tr")));
                    listShuffle = table.OrderBy(a => rng.Next()).ToList();

                }
              

            }

            Reporter.LogTestStepForBugLoggerJSON(Status.Info, refasAgregadas.ToJson().ToString());
        }
        public void ClickBtnAsignarProveedor()
        {
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
            ScrollToElement(0, 0, 0, 0);
           
            actions.MoveToElement(NuevoPedidoElements.btnAsignarProveedor, NuevoPedidoElements.btnAsignarProveedor.Location.X, NuevoPedidoElements.btnAsignarProveedor.Location.Y);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='providerSearch']"))) ;
            waitElementToClick(NuevoPedidoElements.btnAsignarProveedor);

            Reporter.LogPassingTestStepForBugLogger($"Click al boton: '{NuevoPedidoElements.btnAsignarProveedor.Text}'");

        }
        private void VerDetallesRefa(IWebElement rowRefa) {
            IWebElement DetallesVer = rowRefa.FindElement(By.PartialLinkText("Ver"));
            Reporter.LogTestStepForBugLoggerJSON(Status.Info, $"Clic al boton ver del SKU '{rowRefa.GetAttribute("data-sku")}'");
            DetallesVer.Click();
        }
        private bool AgregarAListaProduc(IWebElement rowRefa, int cantidad) {
            string id = rowRefa.GetAttribute("id");
            bool agregado = true;
            try
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                IWebElement elementCantidad = rowRefa.FindElement(By.Id(id + "-id"));
                IWebElement btnAceptar = rowRefa.FindElement(By.Id(id + "-add"));
                elementCantidad.Clear();
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                elementCantidad.SendKeys(cantidad.ToString());
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                //rowRefa.Click();
               IWebElement h4Titulo = Driver1.FindElement(By.XPath("//*[@id='searchAdvancedModalId']//h4"));
                h4Titulo.Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(rowRefa.FindElement(By.Id(id + "-add"))));
                List<string> strings = new List<string>();
                if (btnAceptar.Enabled)
                {
                    Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con SKU '{id}' a Lista de articulos ");
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                    btnAceptar.Click();
                    agregado = true;
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                }
            }
            catch (ElementClickInterceptedException)
            {
                return false;

            }
            catch (NoSuchElementException)
            {
                return false;

            }
            catch (StaleElementReferenceException) {

            }
            return agregado;

        }
        public bool VerificarModalRefa(IWebElement refa, string status, string SKUDescrip, int cantidadPaquete, List<string> Compatibilidades) {
            VerDetallesRefa(refa);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='sparepart-detail-main-container']")));
            IWebElement modalDetalles = Driver1.FindElement(By.XPath("//*[@id='sparepart-detail-main-container']"));
            return (
            VerificarImagenesrRefa(modalDetalles, 4) &&
            VerificarStatus(modalDetalles, status) &&
            VerificarDescripcion(modalDetalles, SKUDescrip) &&
            VerificaCantidadModalRefa(modalDetalles, cantidadPaquete) &&
            VerificarCompatibilidades(modalDetalles, Compatibilidades));

        }
        private bool VerificarCompatibilidades(IWebElement refa, List<string> Compatibilidades) {

            List<IWebElement> ListCompatibilidades = new List<IWebElement>(refa.FindElements(By.ClassName("model-text")));
            foreach (IWebElement Compatibilidad in ListCompatibilidades)
            {
                if (!Compatibilidades.Contains(Compatibilidad.Text)) {
                    return false;
                }
            }
            return true;
        }
        private bool VerificaCantidadModalRefa(IWebElement refa, int cantidad) {
            IWebElement cant = refa.FindElement(By.XPath("./div[2]/div[1]/div[2]/span[2]"));
            int cantidadW = Int32.Parse(cant.Text.Trim());
            return cantidadW == cantidad;


        }
        private bool VerificarImagenesrRefa(IWebElement refa, int numImg) {

            List<IWebElement> ListImg = new List<IWebElement>(refa.FindElements(By.TagName("img")));
            return ListImg.Count == numImg;
        }
        private bool VerificarDescripcion(IWebElement refa, string skuDes) {
            IWebElement descripcion = refa.FindElement(By.ClassName("sparepart-name"));
            string descripW = descripcion.Text.Trim();
            return descripW.Equals(skuDes);

        }
        public bool VerificarSKUSListaPrincipal(List<string> ListSKU)
        {
            bool result = false;
            foreach (string sku in ListSKU)
            {
                result=FindRefacionListaArticulos(sku,"Verificar");
                if (result==false)
                {
                    break;
                }
            }
            return result;
        }
        private bool VerificarStatus(IWebElement refa, string statusRefa) {
            try
            {
                if (statusRefa.Equals("Activo"))
                {
                    IWebElement status = refa.FindElement(By.ClassName("active-status"));
                    return true;
                }
                else if (statusRefa.Equals("Descontinuada"))
                {
                    IWebElement status = refa.FindElement(By.ClassName("inactive-status"));
                    return true;
                }
            }
            catch (ElementNotVisibleException a)
            {
                return false;

            }

            return true;
        }
        public bool VerificarInformacionResult(string SKU, string Descripcion, string cantidadC, string statusBtn) {
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ArticleListTable']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='ArticleListTable']"));
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);
            foreach (IWebElement Row in ListRow)
            {
                string idSKU = Row.GetAttribute("id");
                if (SKU.Equals(idSKU))
                {
                    List<IWebElement> columsContent = new List<IWebElement>(Row.FindElements(By.TagName("td")));
                    //Reporter.LogTestStepForBugLoggerJSON(Status.Info, columsContent.);
                    return (
                    columsContent[0].Text == SKU &&
                    columsContent[1].Text == Descripcion &&
                    columsContent[2].Text == "Ver" &&
                    (columsContent[3].Text == "0" || columsContent[3].Text != "0") &&
                    (columsContent[4].Text != cantidadC || cantidadC == "Descontinuada") &&
                    columsContent[5].Text == statusBtn || (columsContent[5].Text=="Agregado"));


                }
            }
            return false;

        }
        public void SelectNavPedidoByOption(int navOption) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='tabs-header']")));
            IWebElement navPedidos = Driver1.FindElement(By.XPath("//*[@id='tabs-header']"));
            IList<IWebElement> linksNavs = new List<IWebElement>(navPedidos.FindElements(By.TagName("a")));

            if (navOption < linksNavs.Count&& linksNavs.Count<=6) {
                ScrollToElement(0,0, 0, 0);
                Reporter.LogPassingTestStepForBugLogger($"Seleccionando '{linksNavs[navOption].Text}' ");
                linksNavs[navOption].Click();
                Thread.Sleep(2000);
            }
        }
        public string SelectNavPedidoLast() {
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='tabs-header']")));
            IWebElement navPedidos = Driver1.FindElement(By.XPath("//*[@id='tabs-header']"));
            IList<IWebElement> linksNavs = new List<IWebElement>(navPedidos.FindElements(By.TagName("a")));
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando '{linksNavs[linksNavs.Count-1].Text}' ");
            ScrollToElement(0,0, 0, 0);
            linksNavs[linksNavs.Count-1].Click();
            Thread.Sleep(2000);
            var InfoPedido = linksNavs[linksNavs.Count - 1].Text.Replace("\n", "").Split('\r').ToList<string>();
            return InfoPedido[1];
        }
        public string SelectTipoCompraRand() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//select[@id='ShopType']")));
            Thread.Sleep(450);
            SelectElement SelectTipoCompra = new SelectElement(Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//select[@id='ShopType']")));
            Random ran = new Random();
            int r= ran.Next(SelectTipoCompra.Options.Count);
            if(r!=0)
                SelectTipoCompra.SelectByIndex(r);
            else
                SelectTipoCompra.SelectByIndex(1);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el Tipo de compra '{r}' ");
           
            return SelectTipoCompra.SelectedOption.Text;

        }
        public bool FindRefacionListaArticulos(string sku,string accion, [Optional]string cantidad) {
            Thread.Sleep(1500);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleTable']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='ArticleTable']"));
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);
            SelectNavPedidoByOption(0);
            bool result = false;
            foreach (IWebElement article in ListRow)
            {
                IList<IWebElement> ListElementsArticle = new List<IWebElement>(article.FindElements(By.TagName("td")));
                if (sku== ListElementsArticle[1].Text)
                {
                    switch (accion)
                    {
                        case ("ver"):
                            ListElementsArticle[3].FindElement(By.LinkText("Ver")).Click();
                            result = true;
                            break;
                        case ("Eliminar"):
                            ListElementsArticle[9].FindElement(By.TagName("span")).Click();
                            result = true;
                            break;
                        case ("Seleccionar"):
                            ListElementsArticle[0].Click();
                            result = true;
                            break;
                        case ("cantidad"):
                           IWebElement inputCantidad= ListElementsArticle[8].FindElement(By.ClassName("amountToBuyClass"));
                            inputCantidad.Clear();
                            inputCantidad.SendKeys(cantidad);
                            result = true;
                            break;
                        case ("Verificar"):
                            result = true;
                            break;


                    }
                }
            }
         return result;

        }
        public bool FindRefaccionINPedidos(string SKU) {
            Thread.Sleep(3000);
            IList<IWebElement> refaccionINPedidos = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='[object Object]']")));
            bool isFind=false;
            Reporter.LogTestStepForBugLogger(Status.Info,$"Buscando el SKU '{SKU}'");
            foreach (IWebElement RefaInList in refaccionINPedidos)
            {
                IList<IWebElement> tdRefacion = new List<IWebElement>(RefaInList.FindElements(By.TagName("td")));
               
                string t = tdRefacion[3].FindElement(By.TagName("div")).GetAttribute("data-sku");
                if (t== SKU)
                {
                    isFind = true;
                    break;
                }
            }
            return isFind;
        }
        //Funcion que elimina una refacion sin importar en que pedido este para despues eliminarla de la lista de articulos
        public void EliminaRefacionInPedidos(string SKU) {
            Thread.Sleep(1000);
            IList<IWebElement> refaccionINPedidos = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='[object Object]']")));
           
            foreach (IWebElement RefaInList in refaccionINPedidos)
            {
                IList<IWebElement> tdRefacion = new List<IWebElement>(RefaInList.FindElements(By.TagName("td")));
                string t = tdRefacion[3].FindElement(By.TagName("div")).GetAttribute("data-sku");
                if (t == SKU)
                {
                    IWebElement spanEliminar= RefaInList.FindElement(By.TagName("span"));
                    bool isInPedido=true;
                    try
                    {
                       isInPedido = spanEliminar.GetAttribute("data-shopping-order-id").Length>0;

                    }
                    catch (NullReferenceException a)
                    {
                        isInPedido=false;
                       
                    }
                    if (isInPedido)
                    {
                        Driver.FindElement(By.XPath($"//a[@data-order-id='{spanEliminar.GetAttribute("data-shopping-order-id")}']")).Click();
                        spanEliminar.Click();
                        clickSwal2Button("Aceptar");
                        SelectNavPedidoByOption(0);
                        FindRefacionListaArticulos("E12010139", "Eliminar");
                        clickSwal2Button("Aceptar");
                    }
                    else { 
                        Driver.FindElement(By.XPath("//*[@id='tabs-header']/li/a[@href='#articletab']")).Click();
                        spanEliminar.FindElement(By.TagName("i")).Click();
                       
                        clickSwal2Button("Aceptar");


                    }
                    break;
                }
            }
           
        }

        public string eliminarRefaccionPedidoActual() {
            IWebElement ListaArticulosOrden = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
            List<IWebElement> refaccionINPedidos = new List<IWebElement>(ListaArticulosOrden.FindElements(By.TagName("tr")));
            refaccionINPedidos.RemoveRange(0, 2);
           
            Random rnd = new Random();
            int numRand=rnd.Next(refaccionINPedidos.Count);
            IWebElement spanEliminar = refaccionINPedidos[numRand].FindElement(By.TagName("span"));
            IList<IWebElement> tdRefacion = new List<IWebElement>(refaccionINPedidos[numRand].FindElements(By.TagName("td")));
            string skuEliminado= tdRefacion[1].Text;
            Reporter.LogPassingTestStepForBugLogger($"Refaccion con el SKU '{skuEliminado}' fue eliminada");
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
            actions.ScrollToElement(spanEliminar);
            
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver1;
            js.ExecuteScript("arguments[0].setAttribute('style','display:none;');", Driver1.FindElement(By.XPath("//*[@id='site-navbar-collapse']")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='toast-container']")));
            try
            {
                spanEliminar.FindElement(By.XPath("//i[contains(@class,'deleteArticleFromShoppingOrder')]")).Click();
            }
            catch (ElementClickInterceptedException)
            {
               
                actions.ScrollToElement(spanEliminar);
                spanEliminar.FindElement(By.XPath("//i[contains(@class,'deleteArticleFromShoppingOrder')]")).Click();
            }
            
            clickSwal2Button("Aceptar");
            
            return skuEliminado; 
        }


        public string SelectRefaRandomInPedidoActual() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']")));
            IWebElement TablaArticulosPedidoActual = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaArticulosPedidoActual.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);

            Random random = new Random();
            int indexRand = random.Next(ListRowArti.Count-1);
            return ListRowArti[indexRand].GetAttribute("data-sku");
        }
        public IWebElement ReturnRefaInPedidoBySKU(string SKU) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']")));
            List<IWebElement> TablaArticulosPedidoActual = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']//table/tbody/tr")));
          
           
            IWebElement Refa = TablaArticulosPedidoActual[0];
            Reporter.LogTestStepForBugLogger(Status.Info, $"Buscando el SKU '{SKU}'");
            foreach (IWebElement item in TablaArticulosPedidoActual)
            {
                List<IWebElement> ListDataRefa = new List<IWebElement>(item.FindElements(By.TagName("td")));
                string skuRefa = ListDataRefa[1].Text;
                if (skuRefa == SKU)
                {
                    Refa = item;
                    break;
                }    
            }
            return Refa;
        }
        public string ReturnSkuRefaInPedido(IWebElement RefaRow) {
            Thread.Sleep(3000);

            ScrollToElement(RefaRow.Location.X, RefaRow.Location.Y,0,0);
            List<IWebElement> ListDataRefa = new List<IWebElement>(RefaRow.FindElements(By.TagName("td")));
            return ListDataRefa[1].Text;
        }
        public bool VerificarPrecioRefaInPedido(string SKU) {
            IWebElement RefaRow = ReturnRefaInPedidoBySKU(SKU);
            List<IWebElement> ListDataRefa;
            Thread.Sleep(3000);
            try
            {
                 ListDataRefa = new List<IWebElement>(RefaRow.FindElements(By.TagName("td")));
            }
            catch (StaleElementReferenceException)
            {

                ListDataRefa = new List<IWebElement>(RefaRow.FindElements(By.TagName("td")));
            }
            
            return ListDataRefa[6].Text == "$0.00";
        }
        public void PutPrecioYDescuentoRefa(string RefaRow, string precio,string descuento) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Thread.Sleep(500);
            IWebElement RefaAux = ReturnRefaInPedidoBySKU(RefaRow);
            List<IWebElement> ListDataRefa = new List<IWebElement>(RefaAux.FindElements(By.TagName("td")));
            IWebElement divEditableInputPrecio = ListDataRefa[6].FindElement(By.TagName("div"));
            string SKUAux=ListDataRefa[1].Text;
            if (divEditableInputPrecio.Text== "$0.00")
            {
                ScrollToElement(divEditableInputPrecio.Location.X, divEditableInputPrecio.Location.Y,0,0);
                divEditableInputPrecio.Click();
                IWebElement EditableInput = Driver1.FindElement(By.Id("ejbeatycelledit"));//F13012314
                Reporter.LogPassingTestStepForBugLogger($"Agregando el precio de {precio}");
                EditableInput.SendKeys(precio);
                ListDataRefa[1].Click();
            }
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Thread.Sleep(500);
            IWebElement auxRefaRow = ReturnRefaInPedidoBySKU(SKUAux);
            List <IWebElement> ListDataRefa2 = new List<IWebElement>(auxRefaRow.FindElements(By.TagName("td")));
            IWebElement divEditableInputDescuento = ListDataRefa2[7].FindElement(By.TagName("div"));
            if (divEditableInputDescuento.Text == "0%")
            { 
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver1;
                try
                {
                    divEditableInputDescuento.Click();

                }
                catch (StaleElementReferenceException)
                {
                    divEditableInputDescuento.Click();

                }
                //js.ExecuteScript("arguments[0].click();‌​", divEditableInputDescuento);

                js.ExecuteScript("document.getElementById('ejbeatycelledit').setAttribute('value', '"+descuento+"');");

               // IWebElement EditableInput2 = Driver1.FindElement(By.Id("ejbeatycelledit"));
                //EditableInput2.SendKeys(descuento);
                ListDataRefa2[9].Click();
                Reporter.LogPassingTestStepForBugLogger($"Agregando el precio de {precio}");
            }
            
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Thread.Sleep(1000);
        }

        public void PutPrecioYDescuentoAllRefas() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']")));
            IWebElement TablaArticulosPedidoActual = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaArticulosPedidoActual.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            Random random = new Random();
            
            for (int i = 0; i < ListRowArti.Count; i++)
            {
                int numPrecio = random.Next(1000);
                int numDes = random.Next(50);
                if (i!=0)
                {
                    IWebElement TablaByCiclo = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
                    List<IWebElement> ListRowArtiCiclo = new List<IWebElement>(TablaByCiclo.FindElements(By.TagName("tr")));
                    ListRowArtiCiclo.RemoveRange(0, 2);
                    List<IWebElement> ListRowArtiTd = new List<IWebElement>(ListRowArtiCiclo[i].FindElements(By.TagName("td")));
                    PutPrecioYDescuentoRefa(ListRowArtiTd[1].Text, numPrecio.ToString(), numDes.ToString());
                    Thread.Sleep(600);
                }
                else
                {
                    List<IWebElement> ListRowArtiTd = new List<IWebElement>(ListRowArti[0].FindElements(By.TagName("td"))); 
                    PutPrecioYDescuentoRefa(ListRowArtiTd[1].Text, numPrecio.ToString(), numDes.ToString());
                }
            }
            

        }
        public bool CheckPrecioDescuentoMontoCompra(string SKU) {
            IWebElement RefaRow = ReturnRefaInPedidoBySKU(SKU);
            Thread.Sleep(400);
            List<IWebElement> ListDataRefa = new List<IWebElement>(RefaRow.FindElements(By.TagName("td")));
            float precio = float.Parse(ListDataRefa[6].FindElement(By.TagName("div")).Text.Replace('$',' '), CultureInfo.InvariantCulture);
            float descuento = float.Parse(ListDataRefa[8].Text.Replace('$',' '), CultureInfo.InvariantCulture);
            float precioUniD = float.Parse(ListDataRefa[9].Text.Replace('$', ' '), CultureInfo.InvariantCulture);
            float montoCompra = float.Parse(ListDataRefa[11].Text.Replace('$', ' '), CultureInfo.InvariantCulture);
            int cantidad = Int32.Parse(ListDataRefa[10].FindElement(By.ClassName("amountToBuyClassOrder")).GetAttribute("value"));
            bool precioDescuento = (precio - descuento).ToString("0.00") == precioUniD.ToString("0.00");
            bool precioUniDCantidad = (precioUniD * cantidad) == montoCompra;
            List<string> Montos = new List<string>();
            Montos.Add(precio.ToString("0.00"));
            Montos.Add(descuento.ToString("0.00"));
            Montos.Add(precioUniD.ToString("0.00"));
            Montos.Add(montoCompra.ToString("0.00"));
            Montos.Add(cantidad.ToString("0.00"));

            Reporter.LogTestStepForBugLoggerJSON(Status.Info, Montos.ToJson().ToString());

            return precioDescuento && precioUniDCantidad;
        }
        public bool CheckEditCantidad(string SKU) {
            IWebElement RefaRow = ReturnRefaInPedidoBySKU(SKU);
            List<IWebElement> ListDataRefa = new List<IWebElement>(RefaRow.FindElements(By.TagName("td")));
            int randomCant = generateRandomIndex(10);
            ListDataRefa[10].FindElement(By.ClassName("amountToBuyClassOrder")).SendKeys(randomCant.ToString());
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            RefaRow.Click();
            Thread.Sleep(500);
            IWebElement RefaRowAux = ReturnRefaInPedidoBySKU(SKU);
            List<IWebElement> ListDataRefaAux = new List<IWebElement>(RefaRowAux.FindElements(By.TagName("td")));
            float montoCompra = float.Parse(ListDataRefaAux[11].Text.Replace('$', ' '));
            int cantidad = Int32.Parse(ListDataRefaAux[10].FindElement(By.ClassName("amountToBuyClassOrder")).GetAttribute("value"));
            float precioUniD = float.Parse(ListDataRefaAux[9].Text.Replace('$', ' '));

            return  (precioUniD * cantidad) == montoCompra;
        }
        public List<string> EliminarPedidoActual() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]")));
            IWebElement tablaPedidoActual = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]"));
            //Obtener tabla de articulos para obtener los sku del pedido actual
            IWebElement TablaArticulosPedidoActual = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaArticulosPedidoActual.FindElements(By.TagName("tr")));
            List<string> ListSKU = new List<string>();

            ListRowArti.RemoveRange(0, 2);
            foreach (IWebElement rowItem in ListRowArti)
            {
                IList<IWebElement> tdRefacion = new List<IWebElement>(rowItem.FindElements(By.TagName("td")));
                ListSKU.Add(tdRefacion[1].Text);
            }

            IWebElement btnEliminarPedido= tablaPedidoActual.FindElement(By.XPath("//button[contains(@class,'delete-shopping-order')]"));
            btnEliminarPedido.Click();
            clickSwal2Button("Aceptar");
            return ListSKU;

        }
        public bool CheckDetallesPedidoActual() {
            Thread.Sleep(250);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[contains(@id,'ShoppingDetail')]")));
            IWebElement TablaDetalles = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[contains(@id,'ShoppingDetail')]"));
            IWebElement TablaRefacciones = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
            Thread.Sleep(600);
            float CantidaSku = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'SparePartsCount')]")).Text, CultureInfo.InvariantCulture);
            float CantidaArticulos = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'ArticlesCount')]")).Text, CultureInfo.InvariantCulture);
            //float DescuentoPorcenaje= float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'PercentageDiscount')]")).Text);
            float Importe = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'Amount')]")).Text.Remove(0, 1), CultureInfo.InvariantCulture);
            float DescuentoMonto = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'Discounts')]")).Text.Remove(0, 1), CultureInfo.InvariantCulture);
            float SubTotal = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'Subtotal')]")).Text.Remove(0, 1), CultureInfo.InvariantCulture);
            float Taxes = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'Taxes')]")).Text.Remove(0, 1), CultureInfo.InvariantCulture);
            float MontoPagar = float.Parse(TablaDetalles.FindElement(By.XPath("//td[contains(@name,'AmountToPay')]")).Text.Remove(0, 1), CultureInfo.InvariantCulture);
            
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaRefacciones.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            float precio=0;
            float descuento=0;
            float descuentoPorcentaje=0;
            float montoCompra = 0;
            int cantidad = 0;
            int cantidadSKU= ListRowArti.Count;
            foreach (IWebElement article in ListRowArti)
            {
                IList<IWebElement> ListElementsArticle = new List<IWebElement>(article.FindElements(By.TagName("td")));
                int cantidadAux= Int32.Parse(ListElementsArticle[10].FindElement(By.ClassName("amountToBuyClassOrder")).GetAttribute("value"));
                cantidad += Int32.Parse(ListElementsArticle[10].FindElement(By.ClassName("amountToBuyClassOrder")).GetAttribute("value"));//cantidad articulos
                precio += float.Parse(ListElementsArticle[6].Text.Remove(0, 1), CultureInfo.InvariantCulture) * cantidadAux;//Importe
                descuento += float.Parse(ListElementsArticle[8].Text.Remove(0, 1), CultureInfo.InvariantCulture) * cantidadAux;//Descuento Monto
                string porcentajea = ListElementsArticle[7].Text;
                porcentajea = porcentajea.Remove(porcentajea.Length - 1, 1);
                descuentoPorcentaje += float.Parse(porcentajea, CultureInfo.InvariantCulture);
                montoCompra += float.Parse(ListElementsArticle[11].Text.Remove(0,1), CultureInfo.InvariantCulture);//SubTotal

            }
            double Impuestos = montoCompra * 0.16;
            double MontoPagarTotal = (precio - descuento)+Impuestos;

            return (
                    (cantidad.ToString()== CantidaArticulos.ToString())&& 
                    (MontoPagarTotal.ToString("0.00")== MontoPagar.ToString("0.00"))&& (Importe.ToString("0.00")==precio.ToString("0.00"))&&
                    (CantidaSku== cantidadSKU)&&
                    (descuento.ToString("0.00")== DescuentoMonto.ToString("0.00"))&&
                    (montoCompra.ToString("0.00")== SubTotal.ToString("0.00"))&&
                    (Impuestos.ToString("0.00")== Taxes.ToString("0.00"))&&
                    (MontoPagarTotal.ToString("0.00")== MontoPagar.ToString("0.00"))
                );
        }
        public void PutCommentsToPedido(string comentario) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//textarea")));
            IWebElement InputTextArea = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//textarea"));
            ScrollToElement(InputTextArea.Location.X, InputTextArea.Location.Y,0,30);
            Thread.Sleep(500);
            try
            {
                InputTextArea.SendKeys(comentario);
            }
            catch (ElementNotInteractableException)
            {

                InputTextArea.SendKeys(comentario);
            }
            Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[contains(@class,'form-group has-success')]")).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"Agregando comentario a pedido, '{comentario}'");

        }

        public void ReasignarRefa(string RFC) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']")));
            IWebElement TablaArticulosPedidoActual = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[@class='articlesOrderList']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaArticulosPedidoActual.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);

            Random random = new Random();
            int indexRand = random.Next(ListRowArti.Count);
            string clas= ListRowArti[indexRand].FindElement(By.XPath("//td[contains(@class,'select-checkbox all')]")).GetAttribute("class");

            IWebElement BtnReasignarProv = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//a[contains(@class,'reasign-provider')]"));
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
            List<IWebElement>  check = new List<IWebElement>( ListRowArti[indexRand].FindElements(By.TagName("td")));
            check[0].Click();
            actions.MoveToElement(BtnReasignarProv);
            BtnReasignarProv.Click();
            CaptureRFCProvedor(RFC);
            clickSwal2Button("Aceptar");
        }
        public int NavPedidosCount() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='tabs-header']")));
            IWebElement navPedidos = Driver1.FindElement(By.XPath("//*[@id='tabs-header']"));
            IList<IWebElement> linksNavs = new List<IWebElement>(navPedidos.FindElements(By.TagName("a")));
            return linksNavs.Count();
        }
        public int NumSkusInPedido() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[contains(@id,'ShoppingDetail')]")));
            IWebElement TablaDetalles = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[contains(@id,'ShoppingDetail')]"));
            string CantidaSku = TablaDetalles.FindElement(By.XPath("//td[contains(@name,'SparePartsCount')]")).Text;
            return Int32.Parse(CantidaSku);
        }
        public string ClickEnviarPedidoActual() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//button[contains(@id,'toAuthorize')]")));
            IWebElement btnEnviarPedidoActual = Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//button[contains(@id,'toAuthorize')]"));
            ScrollToElement(btnEnviarPedidoActual.Location.X, btnEnviarPedidoActual.Location.Y, 0, -70);
            Thread.Sleep(1000);
            Driver1.FindElement(By.XPath("//div[contains(@class,'active') and contains(@class,'order-content')]//div[contains(@id,'ShoppingDetail')]")).Click();
            string id = btnEnviarPedidoActual.GetAttribute("id").Split('-')[1];
            
            btnEnviarPedidoActual.Click();
            clickSwal2Button("Aceptar");
            return id;
        }

        public List<string> CreaPedido(string RFCProveedor,string TipoProveedor, int NumArticulos) {
            OpenNewTab();
            GoToPage();
            CrearPedido(RFCProveedor, 10, NumArticulos);
            List<string> list = new List<string>();
            string infoPedido =SelectNavPedidoLast();
            list.Add(infoPedido);
            SelectTipoCompraRand();
            if (!TipoProveedor.Equals("Central Fijo Italika"))
                PutPrecioYDescuentoAllRefas();
            PutCommentsToPedido("Pedido registrado por Automatizacion QA");
            string idOrden =ClickEnviarPedidoActual();
            list.Add(idOrden);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogTestStepForBugLoggerJSON(Status.Info, list.ToJson().ToString());
            CloseTab();
            return list;
        }

        public bool FindRefasById(List<string> ListIDRefas) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleTable']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='ArticleTable']"));
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);
            List<string> SKUEncontrados = new List<string>();
            Reporter.LogTestStepForBugLoggerJSON(Status.Info,ListIDRefas.ToJson().ToString());
            try
            {
                foreach (string idRefa in ListIDRefas)
                {
                    IWebElement refaRowa = Driver1.FindElement(By.XPath("//tr[@data-sku='"+idRefa+"']"));
                    SKUEncontrados.Add(refaRowa.GetAttribute("data-sku"));
                }
               
            }
            catch (NotFoundException a)
            {

                return false;
            }
            Reporter.LogTestStepForBugLoggerJSON(Status.Info, SKUEncontrados.ToJson().ToString());


            return true;
        }
    }
}
