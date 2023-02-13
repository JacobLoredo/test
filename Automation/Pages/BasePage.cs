using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Reports;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SeleniumExtras.WaitHelpers;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using SeleniumExtras.PageObjects;
using System.Linq;
using System.IO;
using ExcelDataReader;
using System.Text;
using System.Data;
using OpenQA.Selenium.Support.Extensions;
//using Reporter = Automation.Reports.Reporter;

namespace Automation.Pages
{
    public class BasePage
    {
        public WebDriverWait wait { get; set; }      
        private IWebDriver Driver { get; set; }
        public ConfigQA config{get;set;}
        public Menu Menu { get; set; }
        public IWebElement TituloSelectTaller;
        public DefaultWait<IWebDriver> fluentWait;

        [FindsBy(How = How.XPath, Using = "//button[@class='swal-button swal-button--cancel']")]
        private IWebElement swalButtonCancel { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='swal-button swal-button--confirm']")]
        private IWebElement swalButtonAceptar { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'swal2-cancel')]")]
        private IWebElement swal2ButtonCancel { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'swal2-confirm')]")]
        public IWebElement swal2ButtonAceptar { get; set; }

        [FindsBy(How = How.XPath, Using = "//textarea[contains(@class,'swal2-textarea')]")]
        private IWebElement swal2InputComment { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@id='searchInput']")]
        public IWebElement searchInput { get; set; }
        public BasePage(IWebDriver driver)
        {
            config =  new ConfigQA();
            Driver = driver;
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
            PageFactory.InitElements(driver, this);
            fluentWait = new DefaultWait<IWebDriver>(Driver);
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
        }
        public void moveLoginPage()
        {
            Reporter.LogTestStepForBugLogger(Status.Info,"Ir a la pagina de login");
            Driver.Navigate().GoToUrl(config.UrlPage);
            Reporter.LogPassingTestStepForBugLogger($"Abriendo URL=>{config.UrlPage}");
        }
        public bool isSelectWorkshopPage() {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h3[contains(text(),'Busca un CESIT')]")));
            IWebElement searchResult = Driver.FindElement(By.XPath("//h3[contains(text(),'Busca un CESIT')]"));
            return searchResult.Text.Contains("Busca un CESIT");
        }
        public void WaitSpinner() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        public void SelectWorskshopByName(string workshopName) {
            
            IWebElement inputSe = Driver.FindElement(By.Id("workshop-filter"));
            IWebElement btnSear = Driver.FindElement(By.Id("buttonSearch"));
            IWebElement btnLimpiar = Driver.FindElement(By.XPath("//*[@id='clean-search-btn']"));
            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.XPath("//h3")));
            WaitSpinner();
            searchResult.Click();
            wait.Timeout = TimeSpan.FromMinutes(2);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            WaitSpinner();
            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript("arguments[0].value='"+workshopName+"';", inputSe);
            inputSe.Clear();
            btnLimpiar.Click();
            inputSe.SendKeys(workshopName);
            btnSear.Click();
            waitElementToClick(by:By.XPath("//*[@id='workshopForm']/a//div[@class='workshop-name' and " + "contains(text(),'" + workshopName + "')]"));
            
        }
        public IWebElement waitElementToClick([Optional] IWebElement element, [Optional] By by) {
       
            IWebElement worskshopResult = element==null?wait.Until(ExpectedConditions.ElementToBeClickable(by)): wait.Until(ExpectedConditions.ElementToBeClickable(element));
            worskshopResult.Click();
            return worskshopResult;
        }
        public  IWebElement waitElementToVisible(By by) {
            IWebElement worskshopResult = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            return worskshopResult;
        }
        public bool IsToastSuccess([Optional] string text)
        {
            //"Se guardó el proveedor correctamente.")
            IWebElement toast = waitElementToVisible(By.XPath("//*[@id='toast-container']"));
            IWebElement DivToast = toast.FindElement(By.XPath("./div"));
            if (text != null)
            {
                var texto = DivToast.Text;
                return (DivToast.GetAttribute("class").Equals("toast toast-success") && texto == text);
            }
            else
            {
                return DivToast.GetAttribute("class").Equals("toast toast-success");
            }

        }
        public bool MensajeSwalConfirmacion(string titulo, string mensaje)
        {
            IList<IWebElement> swalModal = Driver.FindElements(By.XPath("//div[@class='swal-modal']/div"));
            bool mensajeCorrecto = true;
            for (int i = 0; i < swalModal.Count; i++)
            {
                string t = swalModal[i].Text;
                if (swalModal[i].GetAttribute("class").Equals("swal-title") && !swalModal[i].Text.Contains(titulo))
                {
                    return false;
                }
                else if (swalModal[i].GetAttribute("class").Equals("swal-text") && !swalModal[i].Text.Contains(mensaje))
                {
                    return false;
                }
            }
            return mensajeCorrecto;
        }
        public bool IsToastError()
        {
            IWebElement toast = waitElementToVisible(By.XPath("//*[@id='toast-container']"));
            IWebElement DivToast = toast.FindElement(By.XPath("./div"));
            var texto = DivToast.Text;
            return DivToast.GetAttribute("class").Equals("toast toast-error");
        }
        public void clickSwalButton(string opcion)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='swal-modal']")));
            if (opcion == "Cancelar")
            {
                waitElementToClick(swalButtonCancel);
               
            }
            else
            {
                waitElementToClick(swalButtonAceptar);
               
            }
            Reporter.LogTestStepForBugLogger(Status.Info, $"Se realizo click sobre la accion '{opcion}'");
        }

        public void clickSwal2Button(string opcion) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            if (opcion == "Cancelar")
            {
                waitElementToClick(swal2ButtonCancel);

            }
            else
            {
                waitElementToClick(swal2ButtonAceptar);

            }
            Reporter.LogTestStepForBugLogger(Status.Info, $"Se realizo click sobre la accion '{opcion}'");
        }
        public void PutComentarioSwal2(string comentario) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//textarea[contains(@class,'swal2-textarea')]")));
            swal2InputComment.SendKeys(comentario);
            Reporter.LogTestStepForBugLogger(Status.Info, $"Se puso el siguiente comentario '{comentario}'");

        }

        public void implicitWait(IWebDriver d,int time) {
            d.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(time);
        }
        public void ScrollToElement(int pointX, int pointY,int extraX,int extraY) {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("window.scrollTo(" + (pointX - extraX) + "," + (pointY - extraY) + ")");

        }
        public void OpenNewTab() {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("window.open('"+config.UrlPage+ "','_blank');");
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
        }
        public void CloseTab() {
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            Driver.Close();
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
        }
        public string ObtenerFechaActual() {
            DateTime thisDay = DateTime.Today;
            return thisDay.ToString("dd/MM/yyyy");
        }
        public DataSet ExcelFileReader(string path) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var stream = File.Open(path, FileMode.Open,FileAccess.Read);
            var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet();
            stream.Close();
            return result;
        }
        public void DisplayNoneNavbar() {
            IWebElement Navbar = Driver.FindElement(By.XPath("//*[@id='navbar-radar']"));
            Driver.ExecuteJavaScript("arguments[0].style.display = 'none';", Navbar);
        }
        public int CloseQuotation()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            Driver.Close();
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            return Driver.WindowHandles.Count;
        }
        public string ReturnRandomMemberList(List<string> list) {
            Random random= new Random();
            int index= random.Next(list.Count);
            return list[index];
        }
        public void WaitToNewWindow() {
            var currentWindow = Driver.CurrentWindowHandle;
            wait.Until(d => d.WindowHandles.Count > 1);
            var newWindow = wait.Until(d =>
            {
                return d.WindowHandles.FirstOrDefault(x => x != currentWindow);
            });
        }
       
    }
}
 