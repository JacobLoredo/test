using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Automation.Pages.CommonElements
{
    public class ModalBusquedaAvanzada : BasePage
    {
        public IWebDriver driver1;
        public ModalBusquedaAvanzada(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//*[@id='searchAdvanced']")]
        private IWebElement BtnSearchAdvanced { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@data-tab-name='SKU']")]
        private IWebElement TabSKU { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@data-tab-name='Description']")]
        private IWebElement TabDescripcion { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@data-tab-name='Advanced']")]
        private IWebElement TabAvanzada { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchBySku-modal']")]
        private IWebElement InputSKU { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='searchByDescription-modal']")]
        private IWebElement InputDescription { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='BtnSearchBySku']")]
        private IWebElement BtnSearchBySKU { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='BtnSearchByDesc']")]
        private IWebElement BtnSearchByDescription { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='close']")]
        private IWebElement BtnCloseModal { get; set; }


        private void ClicBtnSearchAdvance() {
            wait.Until(ExpectedConditions.ElementToBeClickable(BtnSearchAdvanced));
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de busqueda por: '{BtnSearchAdvanced.Text}'");
            BtnSearchAdvanced.Click();  
        }
        private void ClicTabTypeSearch(string Busqueda)
        {
            Reporter.LogPassingTestStepForBugLogger($"Realizando busqueda por: '{Busqueda}'");
            switch (Busqueda.ToLower())
            {
                case ("sku"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(TabSKU));
                    TabSKU.Click();
                    break;
                case ("descripcion"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(TabDescripcion));
                    TabDescripcion.Click(); 
                    break;
                case ("avanzada"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(TabAvanzada));
                    TabAvanzada.Click();
                    break;
                default:
                    break;
            }
        }
        private void PutTextSearch(string TypeSearch,string TextSearch) {
            Thread.Sleep(2000);
            switch (TypeSearch.ToLower())
            {
                case ("sku"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(InputSKU));
                    InputSKU.SendKeys(TextSearch);
                    break;
                case ("descripcion"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(InputDescription));
                    InputDescription.SendKeys(TextSearch);
                    break;
                default:
                    break;
            }
        }

        private void ClicBtnSearchSKU() {
            BtnSearchBySKU.Click();
        }
        private void ClicBtnSearchDescription()
        {

            BtnSearchByDescription.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }

        public void SearchByDescription(string TextFind) {
            ClicBtnSearchAdvance();
            ClicTabTypeSearch("descripcion");
            PutTextSearch("descripcion", TextFind);

            if (!BtnSearchByDescription.Enabled)
            {
                ClicTabTypeSearch("sku");
                ClicTabTypeSearch("descripcion");
                PutTextSearch("descripcion", TextFind);
            }

            ClicBtnSearchDescription();
        }
        private int generateRandomIndex(int numMax)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, numMax - 1);
            return index;
        }
        private void SelectSKURandom(int NumRefas) {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ArticleListTable']")));
            IWebElement table = driver1.FindElement(By.XPath("//*[@id='ArticleListTable']"));
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);

            while (NumRefas > 0)
            {
                int randomIndex = generateRandomIndex(ListRow.Count);
                bool addRefa=AgregarListaCotizacion(ListRow[randomIndex]);
                if (addRefa)
                    NumRefas--;
            }
        }

        private bool AgregarListaCotizacion(IWebElement rowRefa) {
            string id = rowRefa.GetAttribute("id");
            try{
                IWebElement Element = rowRefa.FindElement(By.XPath("./td[contains(@class,' all')]/*"));
                string c = Element.Text;
                string text = Element.GetAttribute("class");
                string tag = Element.TagName;
                if (Element.TagName == "button")
                {
                    Element.Click();
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                    return true;
                }
                else
                    return false;
            }
            catch (ElementClickInterceptedException) {
                return false;
            }
            catch (NoSuchElementException){
                return false;
            }
            catch (StaleElementReferenceException){
                return false;
            }
        }

        public void AddRefasToListArticules(int NumRefas, string RefaSearch) {
            SearchByDescription(RefaSearch);
            SelectSKURandom(NumRefas);
            BtnCloseModal.Click();
        }
    }
}
