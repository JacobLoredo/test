using Automation.Reports;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using System.Collections.Generic;
using SeleniumExtras.WaitHelpers;
using Automation.Pages.CommonElements;


namespace Automation.Pages.Modules.LocalizadorRefacciones.Actions
{
    public class ActionsLocalizador:LocalizadorPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public ActionsLocalizador(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        public void GoToPage()
        {
            Reporter.LogTestStepForBugLogger(Status.Info, "Ir a la pagina de Localizador de refacciones");
            Driver.Navigate().GoToUrl(config.UrlPage + "/SparepartLocator");
            Reporter.LogPassingTestStepForBugLogger($"Abriendo URL=>{config.UrlPage + "/SparepartLocator"}");

        }
        public void ClickDetalleResult(string SKU) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='SearchAdvancedTableContainer']")));
            IWebElement RowData = Driver1.FindElement(By.XPath("//table[@id='ArticleListTable']//tr[@id='"+SKU+"']"));
            string info = RowData.Text;
            IWebElement btnDetalle= RowData.FindElement(By.XPath("//a[@title='Ver detalle']"));
            btnDetalle.Click();
        }

        public void BuscarSKU(string SKU) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            BuscadorLocalizador buscador = new BuscadorLocalizador(Driver1);
            buscador.searchBySKU(SKU);
            
        }
        public void ClicFiltroMunicipio() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
           
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{LocalizadorElements.FiltroMunicipio.Text}'");
            LocalizadorElements.FiltroMunicipio.Click();
        }
        public void ClicFiltroEstado()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{LocalizadorElements.FiltroEstado.Text}'");
            LocalizadorElements.FiltroEstado.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        public void ClicFiltroZona()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{LocalizadorElements.FiltroZona.Text}'");
            LocalizadorElements.FiltroZona.Click();
        }
        public void ClicFiltroRegion()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{LocalizadorElements.FiltroRegion.Text}'");
            LocalizadorElements.FiltroRegion.Click();
        }
        public void ClicFiltroPais()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{LocalizadorElements.FiltroPais.Text}'");
            LocalizadorElements.FiltroPais.Click();
        }

        public List<IWebElement> ObtenerDataRow() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            IWebElement table = Driver1.FindElement(By.XPath("//*[@id='WorkShops']"));
            List<IWebElement> ListRow = new List<IWebElement>(table.FindElements(By.TagName("tr")));
            ListRow.RemoveRange(0, 2);

            return ListRow;

        }

        public bool CheckMunicipio(List<IWebElement> ListRows) {
            List<string> Municipio = new List<string>();
            foreach (IWebElement Row in ListRows)
            {
                List<IWebElement> ListTd = new List<IWebElement>(Row.FindElements(By.TagName("td")));
                if (!Municipio.Contains(ListTd[2].Text))
                {
                    Municipio.Add(ListTd[2].Text);
                }
            }
            return Municipio.Count == 1;
        }
        public bool CheckEstado(List<IWebElement> ListRows)
        {
            List<string> Estado = new List<string>();
            foreach (IWebElement Row in ListRows)
            {
                List<IWebElement> ListTd = new List<IWebElement>(Row.FindElements(By.TagName("td")));
                if (!Estado.Contains(ListTd[3].Text))
                {
                    Estado.Add(ListTd[3].Text);
                }
            }
            return Estado.Count == 1;
        }




    }
}
