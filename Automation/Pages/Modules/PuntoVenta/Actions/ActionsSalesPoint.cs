using Automation.Pages.CommonElements;
using Automation.Pages.CommonElements.Modales;
using Automation.Reports;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;
namespace Automation.Pages.Modules.PuntoVenta.Actions
{
    public class ActionsSalesPoint:SalesPointPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public ActionsSalesPoint(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }

        private List<string> RefasWithInventory() { 
            List<string> ListRefas = new List<string>();
            string Refas = "C06070014,E08030018,F09030008,E08010130,F13020534,M04030013,E02040025,F13010337,F14020451,F02020129";
            string[] authorsList = Refas.Split(',');
            for (int i = 0; i < authorsList.Length; i++)
            {
                ListRefas.Add(authorsList[i]);
            }
            return ListRefas;
        }
        public void AddRefasToCaja(int numRefas, string RefaDescription) {
            AddRefaFromInventario();
            ModalBusquedaAvanzada busquedaAvanzada = new ModalBusquedaAvanzada(Driver1);
            busquedaAvanzada.AddRefasToListArticules(numRefas,RefaDescription);
        }
        public string CreateQuotationWithInventory() {
            AddManyRefaFromInventario();
            PutDescuentoAllRefas();
            UpdateTerms("Terminos y condiciones cambiados por Automatizacion QA");
            SelectTypeQuotation();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationSendMailModal']//h4")));
            IWebElement NumCotizacion = Driver1.FindElement(By.XPath("//*[@id='QuotationSendMailModal']//h4"));
            string NumCot = NumCotizacion.Text.Replace("Cotización", "");
            CloseQuotationSendMailModal();
            CloseTypeQuotation();
            return NumCot.Trim();
        }
        public string CreateSaleWithInventory()
        {
            AddManyRefaFromInventario();
            PutDescuentoAllRefas();
            UpdateTerms("Terminos y condiciones cambiados por Automatizacion QA");
            SalesPointEle.ClicBtnCobrar();
            PaidSaleOrderComplete();
            return CloseSaleChangeModal();
            
        }
        private string CloseSaleChangeModal() {
            ModalCompraFinalizada compraFinalizada = new ModalCompraFinalizada(Driver1);
            compraFinalizada.WaitModalCobro();
            string FolioVenta=compraFinalizada.GetFolioVenta();
            compraFinalizada.ClicBtnSalir();
            return FolioVenta;
        }
        private bool PaidSaleOrderComplete() {
            ModalCobro modalCobro = new ModalCobro(Driver1);
           return  modalCobro.PaidSaleCompleteOrder();
        }
        private void AddManyRefaFromInventario()
        {
            List<string> ListRefas=  RefasWithInventory();
            Random rand = new Random(); 
            int numRefas = rand.Next(ListRefas.Count);
            for (int i = 0; i < numRefas; i++)
            {
                int RandRefaIndex=rand.Next(ListRefas.Count);
                WaitSpinner();
                wait.Until(ExpectedConditions.ElementToBeClickable(SalesPointEle.InputSearchBySKU));
                SalesPointEle.PutSKUSearch(ListRefas[RandRefaIndex]);
                SalesPointEle.ClicBtnSearchBySku();
                WaitSpinner();
            }
            WaitSpinner();
            Thread.Sleep(1000);
        }
        private void AddRefaFromInventario() {
            WaitSpinner();
            SalesPointEle.PutSKUSearch("F09030008");
            SalesPointEle.ClicBtnSearchBySku();
            WaitSpinner();
            SalesPointEle.PutSKUSearch("E08010130");
            SalesPointEle.ClicBtnSearchBySku();
            WaitSpinner();
        }

        public void SearchClientByKey(string ClientKey) {
           SalesPointEle.PutClientKey(ClientKey);
            ActionsSelenium act = new ActionsSelenium(Driver);
            act.SendKeys(Keys.Enter);
            IWebElement Label = Driver1.FindElement(By.XPath("//label[@for='keyCustomerSearch']"));
            act.MoveToElement(Label).Perform();
            Label.Click();
            WaitSpinner();
        }
        public void PutPrecioYCantidadAllRefas([Optional] bool OrdenVenta) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartTable']")));
            IWebElement TablaSellCart = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaSellCart.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            Random random = new Random();
            for (int i = 0; i < ListRowArti.Count; i++)
            {
                int numPrecio = random.Next(1000);
                int numCantidad = 0;
                if (!OrdenVenta)
                    numCantidad = random.Next(50);
                else
                    numCantidad = 1;
                int numDes = random.Next(50);
                if (i != 0)
                {
                    IWebElement TablaByCiclo = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartTable']"));
                    List<IWebElement> ListRowArtiCiclo = new List<IWebElement>(TablaByCiclo.FindElements(By.TagName("tr")));
                    ListRowArtiCiclo.RemoveRange(0, 2);
                    PutPrecioCantidadRefa(ListRowArtiCiclo[i], numPrecio.ToString(), numDes.ToString(), numCantidad.ToString());
                    Thread.Sleep(600);
                }
                else
                {
                    IWebElement TablaByCiclo = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartTable']"));
                    List<IWebElement> ListRowArtiCiclo = new List<IWebElement>(TablaByCiclo.FindElements(By.TagName("tr")));
                    ListRowArtiCiclo.RemoveRange(0, 2);
                    PutPrecioCantidadRefa(ListRowArtiCiclo[0], numPrecio.ToString(), numDes.ToString(), numCantidad.ToString());
                }

            }

        }

        private void PutPrecioCantidadRefa(IWebElement RefaRow, string Precio,string descueto,string cantidad) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            string SKURefa = RefaRow.GetAttribute("data-sku");
            IWebElement RowRefaAux = Driver1.FindElement(By.XPath("//tr[@data-sku='" + SKURefa + "']"));
            List<IWebElement> ListRowArtiTd = new List<IWebElement>(RowRefaAux.FindElements(By.TagName("td")));
            Thread.Sleep(500);
            IWebElement divEditableInputPrecio = ListRowArtiTd[5].FindElement(By.TagName("div"));
            if (divEditableInputPrecio.Text=="$0.00")
            {
                ScrollToElement(divEditableInputPrecio.Location.X, divEditableInputPrecio.Location.Y, 0, 0);
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver1;
                try
                {
                    Thread.Sleep(500);
                    divEditableInputPrecio.Click();
                     IWebElement EditableInput = Driver1.FindElement(By.Id("ejbeatycelledit"));
                    Thread.Sleep(500);
                }
                catch (NoSuchElementException)
                {

                    divEditableInputPrecio.Click();
                }
                js.ExecuteScript("document.getElementById('ejbeatycelledit').setAttribute('value', '" + descueto + "');");
               // IWebElement EditableInput = Driver1.FindElement(By.Id("ejbeatycelledit"));
                Reporter.LogPassingTestStepForBugLogger($"Agregando el precio de {Precio}");
                //EditableInput.SendKeys(Precio);
                ListRowArtiTd[2].Click();
            }
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));

            /*
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            IWebElement RowRefaAux = Driver1.FindElement(By.XPath("//tr[@data-sku='" + SKURefa + "']"));
            List<IWebElement> ListDataRefa2 = new List<IWebElement>(RowRefaAux.FindElements(By.TagName("td")));
            IWebElement divEditableInputDescuento = ListDataRefa2[7].FindElement(By.TagName("div"));

            if (divEditableInputDescuento.Text == "0%")
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver1;
                try
                {
                    Thread.Sleep(500);
                    divEditableInputDescuento.Click();
                    Thread.Sleep(500);
                }
                catch (StaleElementReferenceException)
                {
                    divEditableInputDescuento.Click();

                }
                js.ExecuteScript("document.getElementById('ejbeatycelledit').setAttribute('value', '" + descueto + "');");
                ListDataRefa2[2].Click();
                Reporter.LogPassingTestStepForBugLogger($"Agregando el precio de {descueto}");
            }
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            
            */
        }
        public void EditAmountAllRefas() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartTable']")));
            IWebElement TablaSellCart = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaSellCart.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            Random random = new Random();
            for (int i = 0; i < ListRowArti.Count; i++)
            {
                int Cantidad = random.Next(10);
                IWebElement TablaSellCart2 = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartTable']"));
                List<IWebElement> ListRowArti2 = new List<IWebElement>(TablaSellCart2.FindElements(By.TagName("tr")));
                ListRowArti2.RemoveRange(0, 2);
                string SKU = ListRowArti2[i].GetAttribute("data-sku");
                IWebElement RowRefaAux2 = Driver1.FindElement(By.XPath("//tr[@data-sku='" + SKU + "']"));
                Thread.Sleep(600);
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("amountToSellClassOrder")));
                IWebElement inputAmount = ListRowArti2[i].FindElement(By.ClassName("amountToSellClassOrder"));
                ScrollToElement(inputAmount.Location.X, inputAmount.Location.Y,0,-60);
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                inputAmount.SendKeys(Cantidad.ToString());

                RowRefaAux2.Click();
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                Thread.Sleep(500);

            }
        }

        public bool CheckPurchaseAmount() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartTable']/tbody")));
            List<IWebElement>  TablaSellCart = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ArticleSellCartTable']/tbody/tr")));
            bool IsCorrect = true;
            for (int i = 0; i < TablaSellCart.Count; i++)
            {
                List<IWebElement> ListDataRefa = new List<IWebElement>(TablaSellCart[i].FindElements(By.TagName("td")));
                float DescuentoProdu = float.Parse(ListDataRefa[6].Text.Replace('%', ' '), CultureInfo.InvariantCulture);
                float descuento = 1 - (DescuentoProdu / 100);
                float precioUniD = float.Parse(ListDataRefa[5].Text.Replace('$', ' '), CultureInfo.InvariantCulture);
                int cantidad = Int32.Parse(ListDataRefa[9].FindElement(By.ClassName("amountToSellClassOrder")).GetAttribute("value"));
                float montoCompra = float.Parse(ListDataRefa[10].Text.Replace('$', ' '), CultureInfo.InvariantCulture);
                float PrecioDescuento = precioUniD * descuento;
                float ImporteTo = (PrecioDescuento * cantidad);
                bool precioUniDCantidad = ImporteTo.ToString("0.0") == montoCompra.ToString("0.0");
                float IsLessTo1Cent = ImporteTo - montoCompra;
                if (!precioUniDCantidad&& IsLessTo1Cent>=1.00) 
                    return false;
            }
            return IsCorrect;
        }

        public bool CheckAmountToPay() {
            Thread.Sleep(450);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartTable']/tbody")));
            List<IWebElement> TablaSellCart = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ArticleSellCartTable']/tbody/tr")));
            Double Total = 0;
            IWebElement TotalElement = Driver1.FindElement(By.XPath("//span[@name='AmountToPay']"));
            float TotalAmountToPay = float.Parse(TotalElement.Text.Replace('$', ' ').Replace("MXN",""), CultureInfo.InvariantCulture);
            for (int i = 0; i < TablaSellCart.Count; i++)
            {
                //Thread.Sleep(400);
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("td")));
                List<IWebElement> ListDataRefa = new List<IWebElement>(TablaSellCart[i].FindElements(By.TagName("td")));
                float DescuentoProdu = float.Parse(ListDataRefa[6].Text.Replace('%', ' '), CultureInfo.InvariantCulture);
                double descuento = 1 - (DescuentoProdu / 100);
                Double precioUniD = double.Parse(ListDataRefa[5].Text.Replace('$', ' '), CultureInfo.InvariantCulture);
                int cantidad = Int32.Parse(ListDataRefa[9].FindElement(By.ClassName("amountToSellClassOrder")).GetAttribute("value"));
                float montoCompra = float.Parse(ListDataRefa[10].Text.Replace('$', ' '), CultureInfo.InvariantCulture);
                Double PrecioDescuento = precioUniD * descuento;

                Double ImporteTo = (PrecioDescuento * cantidad);
                Total += ImporteTo;
            }
            Total += float.Parse(Driver1.FindElement(By.XPath("//span[@name='Taxes']")).Text.Replace('$', ' '), CultureInfo.InvariantCulture);
            bool IsEqualMount = TotalAmountToPay.ToString("0.00") == Total.ToString("0.00");
            Double Diff = TotalAmountToPay - Total;
            if (!IsEqualMount && Diff >= 1.00)
                return false;
            else
                return true;
        }

        public void PutDescuentoAllRefas() {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@data-original-title='Aplicar descuento general']")));
            IWebElement BtnDescuento = Driver1.FindElement(By.XPath("//*[@data-original-title='Aplicar descuento general']"));

            BtnDescuento.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='DiscountModal']")));
            IWebElement InputPorcentaje = Driver1.FindElement(By.XPath("//*[@id='DiscountPercent']"));
            wait.Until(ExpectedConditions.ElementToBeClickable(InputPorcentaje));
            ActionsSelenium actionsSelenium = new ActionsSelenium(Driver1);
            actionsSelenium.MoveToElement(InputPorcentaje).SendKeys(Keys.Delete).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(InputPorcentaje,"5").Build().Perform();
            WaitSpinner();
            IWebElement BtnAplicarDescuento = Driver1.FindElement(By.XPath("//*[@id='DiscountModal']//button[contains(@class,'save-discount')]"));
            wait.Until(ExpectedConditions.ElementToBeClickable(BtnAplicarDescuento));
            BtnAplicarDescuento.Click();
            WaitSpinner();
        }

        public void UpdateTerms(string newTerms) {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='update-terms']")));
            SalesPointEle.ClicUpdateTerms();
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='TermsAndConditionsModal']")));
            IWebElement TextBoxTerms = Driver1.FindElement(By.XPath("//*[@id='terms-text-area']"));
            TextBoxTerms.Click();
            TextBoxTerms.SendKeys(newTerms);
            IWebElement BtnSaveChances = Driver1.FindElement(By.XPath("//*[@id='TermsAndConditionsModal']//button[contains(@class,'edit-terms')]"));
            BtnSaveChances.Click();
            WaitSpinner();
        }
        public void SelectTypeQuotation() {
            SalesPointEle.ClicBtnCotizar();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationTypeModal']")));
            IWebElement CardCotizacionSimple = Driver1.FindElement(By.XPath("//*[@id='QuotationTypeModal']//div[contains(@class,'quotation-without-advance')]"));
            CardCotizacionSimple.Click();
            WaitSpinner();
        }
        public void CloseTypeQuotation()
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationTypeModal']//button[text()='Cancelar']")));
            IWebElement BtnCloseModal = Driver1.FindElement(By.XPath("//*[@id='QuotationTypeModal']//button[text()='Cancelar']"));
            BtnCloseModal.Click();
            WaitSpinner();
        }
        public string PrintQuotation() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationSendMailModal']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationSendMailModal']//button[contains(@class,'print-quotation')]")));
            IWebElement BtnPrintQuotation = Driver1.FindElement(By.XPath("//*[@id='QuotationSendMailModal']//button[contains(@class,'print-quotation')]"));
            IWebElement NumCotizacion = Driver1.FindElement(By.XPath("//*[@id='QuotationSendMailModal']//h4"));
            string NumCot = NumCotizacion.Text.Replace("Cotización","");
            BtnPrintQuotation.Click();  
            WaitSpinner();
            return NumCot.Trim();
        }

        public void  CloseQuotationSendMailModal() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationSendMailModal']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='QuotationSendMailModal']//button[contains(text(),'Salir')]")));
            IWebElement BtnCloseQuotationSendMailModal = Driver.FindElement(By.XPath("//*[@id='QuotationSendMailModal']//button[contains(text(),'Salir')]"));
            BtnCloseQuotationSendMailModal.Click();
            WaitSpinner();
        }
        public void ClicHistory() { 
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver1;
            try
            {
                js.ExecuteScript("document.getElementById('toast-container').style.display='none';");
                SalesPointEle.BtnConsultaHistorial.Click();
                WaitSpinner();
            }
            catch (JavaScriptException)
            {
                SalesPointEle.BtnConsultaHistorial.Click();
                WaitSpinner();

            }
          
        }
        public void LookQuotationByFolio(string Folio)
        {
            string FolioSub = Folio.Substring(7,7);
            BuscadorHistorialPuntoVenta buscadorHistorial = new BuscadorHistorialPuntoVenta(Driver1);
            buscadorHistorial.SearchQuotationByFolio(FolioSub);
            IWebElement RowQuotation = Driver1.FindElement(By.XPath("//*[@data-folio='"+Folio+"']"));
            RowQuotation.FindElement(By.TagName("a")).Click();
            WaitSpinner();
            
        }
        public bool ValidInfoQuotation(string Folio) {
            WaitSpinner();
            string fechaActual = ObtenerFechaActual();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartDetailsTable']")));
            IWebElement H1Titule = Driver1.FindElement(By.XPath("//h1"));
            return H1Titule.Text.Contains(Folio);
        }

        public void GetDetailsFromPedido() {
          
        }
        public void GetListSKUsFromTable() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartTable']/tbody")));
            List<IWebElement> TablaSellCart = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ArticleSellCartTable']/tbody/tr")));
            Dictionary<string, object> DetailsSKUs = new Dictionary<string, object>();

            foreach (IWebElement itemTable in TablaSellCart)
            {
                List<IWebElement> ListDataRefa = new List<IWebElement>(itemTable.FindElements(By.TagName("td")));
                // Convertir la lista en un diccionario
                Dictionary<string, IWebElement> dictionary = ListDataRefa .ToDictionary(x => x.Text, x => x);
                DetailsSKUs.Add(itemTable.GetAttribute("data-sku"), dictionary);
            }
        }
    
    }
}
