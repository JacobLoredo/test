using Automation.Pages.CommonElements;
using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;

namespace Automation.Pages.Modules.PuntoVenta.Actions
{
    public class ActionsEditQuotation : EditQuotationPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public ActionsEditQuotation(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }

        public void EditQuotation(string TypeClient)
        {
            EditQuotationEle.ClicBtnEditQuotation();
            WaitSpinner();
            AddRefasQuotation(2, "BASE");
            switch (TypeClient.ToUpper())
            {
                case ("GENERAL"):
                    SearchClientByKey("CC4619-00000001");
                    break;
                case ("DIFERENTE"):
                    SearchClientByKey("1");
                    break;
                default:
                    break;
            }
            DeleteRefaQuotation();
            Thread.Sleep(1000);
            EditAmountAllRefas();
            EditPrecioAllRefas();
            UpdateTerms("Actualizado por Automatizacion QA");
            Thread.Sleep(250);
            PutDescuentoAllRefas();
            Thread.Sleep(250);
            EditQuotationEle.ClicBtnSaveEditionQuotation();
            Thread.Sleep(250);
            clickSwal2Button("Aceptar");
        }
        public bool ValidateNewQuotation(string NumCot) {
            Thread.Sleep(4000);
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartDetailsTable']")));
            IWebElement h1NumCotizacion = Driver1.FindElement(By.XPath("//h1"));
            string text = h1NumCotizacion.Text;
            string h1NumCot= text.Substring(21,15);

            return h1NumCot != NumCot;
        }
        private void UpdateTerms(string newTerms)
        {
            WaitSpinner();
            EditQuotationEle.ClicUpdateTerms();
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='TermsAndConditionsModal']")));
            IWebElement TextBoxTerms = Driver1.FindElement(By.XPath("//*[@id='terms-text-area']"));
            TextBoxTerms.Clear();
            TextBoxTerms.Click();
            TextBoxTerms.SendKeys(newTerms);
            IWebElement BtnSaveChances = Driver1.FindElement(By.XPath("//*[@id='TermsAndConditionsModal']//button[contains(@class,'edit-terms')]"));
            BtnSaveChances.Click();
            WaitSpinner();
        }
        private void PutDescuentoAllRefas()
        {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@title='Aplicar descuento general']")));
            IWebElement BtnDescuento = Driver1.FindElement(By.XPath("//div[@title='Aplicar descuento general']"));

            BtnDescuento.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='DiscountModal']")));

            IWebElement InputPorcentaje = Driver1.FindElement(By.XPath("//*[@id='DiscountPercent']"));
            Thread.Sleep(400);
            //InputPorcentaje.Click();
            InputPorcentaje.SendKeys("5");
            WaitSpinner();
            IWebElement BtnAplicarDescuento = Driver1.FindElement(By.XPath("//*[@id='DiscountModal']//button[contains(@class,'save-discount')]"));
            BtnAplicarDescuento.Click();
            WaitSpinner();
        }
        public void AddRefasQuotation(int numRefas, string RefaDescription)
        {
            ModalBusquedaAvanzada busquedaAvanzada = new ModalBusquedaAvanzada(Driver1);
            busquedaAvanzada.AddRefasToListArticules(numRefas, RefaDescription);
        }
        public void SearchClientByKey(string ClientKey)
        {
            EditQuotationEle.PutClientKey(ClientKey);
            ActionsSelenium act = new ActionsSelenium(Driver);
            act.SendKeys(Keys.Enter);
            IWebElement Label = Driver1.FindElement(By.XPath("//label[@for='keyCustomerSearch']"));
            Label.Click();
            WaitSpinner();
        }
        private void DeleteRefaQuotation()
        {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartDetailsTable']/tbody")));
            List<IWebElement> TablaSellCart = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='ArticleSellCartDetailsTable']/tbody/tr")));
            Random random = new Random();
            int indexToDelete = random.Next(TablaSellCart.Count);
            IWebElement IconDelete = TablaSellCart[indexToDelete].FindElement(By.XPath("//i[contains(@class,'deleteArticleFromSaleCart')]"));
            ActionsSelenium act = new ActionsSelenium(Driver);
            act.MoveToElement(IconDelete);
            IconDelete.Click();
            clickSwal2Button("Aceptar");
            WaitSpinner();
        }

        private void EditPrecioAllRefas()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartDetailsTable']")));
            IWebElement TablaSellCart = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartDetailsTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaSellCart.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            Random random = new Random();
            for (int i = 0; i < ListRowArti.Count; i++)
            {
                Thread.Sleep(250);
                int numPrecio = random.Next(1000);
                if (i != 0)
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartDetailsTable']")));
                    IWebElement TablaByCiclo = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartDetailsTable']"));
                    List<IWebElement> ListRowArtiCiclo = new List<IWebElement>(TablaByCiclo.FindElements(By.TagName("tr")));
                    ListRowArtiCiclo.RemoveRange(0, 2);
                    PutPrecioCantidadRefa(ListRowArtiCiclo[i], numPrecio.ToString());
                    Thread.Sleep(600);
                }
                else
                {
                    IWebElement TablaByCiclo = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartDetailsTable']"));
                    List<IWebElement> ListRowArtiCiclo = new List<IWebElement>(TablaByCiclo.FindElements(By.TagName("tr")));
                    ListRowArtiCiclo.RemoveRange(0, 2);
                    PutPrecioCantidadRefa(ListRowArtiCiclo[0], numPrecio.ToString());
                }
            }
        }
        private void PutPrecioCantidadRefa(IWebElement RefaRow, string Precio)
        {
            WaitSpinner();
            string SKURefa = RefaRow.GetAttribute("data-sku");
            IWebElement RowRefaAux = Driver1.FindElement(By.XPath("//tr[@data-sku='" + SKURefa + "']"));
            List<IWebElement> ListRowArtiTd = new List<IWebElement>(RowRefaAux.FindElements(By.TagName("td")));
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementToBeClickable(ListRowArtiTd[5].FindElement(By.TagName("div"))));
            IWebElement divEditableInputPrecio = ListRowArtiTd[5].FindElement(By.TagName("div"));
            if (divEditableInputPrecio.Text != "$0.00")
            {
                ScrollToElement(divEditableInputPrecio.Location.X, divEditableInputPrecio.Location.Y, 0, -60);
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
                js.ExecuteScript("document.getElementById('ejbeatycelledit').setAttribute('value', '" + Precio + "');");
                // IWebElement EditableInput = Driver1.FindElement(By.Id("ejbeatycelledit"));
                Reporter.LogPassingTestStepForBugLogger($"Agregando el precio de {Precio}");
                //EditableInput.SendKeys(Precio);
                ListRowArtiTd[2].Click();
            }
            WaitSpinner();
        }
        public void EditAmountAllRefas()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ArticleSellCartDetailsTable']")));
            IWebElement TablaSellCart = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartDetailsTable']"));
            List<IWebElement> ListRowArti = new List<IWebElement>(TablaSellCart.FindElements(By.TagName("tr")));
            ListRowArti.RemoveRange(0, 2);
            Random random = new Random();
            for (int i = 0; i < ListRowArti.Count; i++)
            {
                int Cantidad = random.Next(10);
                IWebElement TablaSellCart2 = Driver1.FindElement(By.XPath("//*[@id='ArticleSellCartDetailsTable']"));
                List<IWebElement> ListRowArti2 = new List<IWebElement>(TablaSellCart2.FindElements(By.TagName("tr")));
                ListRowArti2.RemoveRange(0, 2);
                string SKU = ListRowArti2[i].GetAttribute("data-sku");
                IWebElement RowRefaAux2 = Driver1.FindElement(By.XPath("//tr[@data-sku='" + SKU + "']"));
                Thread.Sleep(500);
                IWebElement inputAmount = ListRowArti2[i].FindElement(By.ClassName("amountToSellClassOrder"));
                ActionsSelenium act = new ActionsSelenium(Driver1);
                act.MoveToElement(inputAmount);
                WaitSpinner();
                inputAmount.SendKeys(Cantidad.ToString());
                act.MoveToElement(RowRefaAux2);
                RowRefaAux2.Click();
                WaitSpinner();
                Thread.Sleep(500);

            }

        }
    }
}
