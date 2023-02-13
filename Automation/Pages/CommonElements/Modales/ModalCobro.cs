using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;
namespace Automation.Pages.CommonElements.Modales
{
    public class ModalCobro : BasePage
    {
        public IWebDriver driver1;
        public ModalCobro(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='AdvanceCollector']")]
        private IWebElement ModalC { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='AdvanceCollectorModal']//h4[text()='Importe total:']/../div/h4[1]")]
        private IWebElement ImporteTotal { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='totalReceived']")]
        private IWebElement TotalRecibido { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='remaining']")]
        private IWebElement Restante { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='change']")]
        private IWebElement Cambio { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@data-dismiss='modal' and contains(@class,'cancel-modal')]")]
        private IWebElement BtnCancelar { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='AdvanceCollectorButton']")]
        private IWebElement BtnConfirmarCobro { get; set; }


        public void ClicBtnCancelar()
        {
            ActionsSelenium actions = new ActionsSelenium(driver1);
            actions.MoveToElement(BtnCancelar).Click().Build().Perform();
        }
        public void ClicBtnConfirmarCobro()
        {
            ActionsSelenium actions = new ActionsSelenium(driver1);
            actions.MoveToElement(BtnConfirmarCobro).Click().Build().Perform();
        }
        public void WaitModalCobro()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(ModalC));
        }
        public string GetImporteTotal()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='AdvanceCollectorModal']//h4[text()='Importe total:']/../div/h4[1]")));
            string g = ImporteTotal.Text.Replace('$', ' ').Trim();
            float ImpT = float.Parse(g, CultureInfo.InvariantCulture);
            return string.Format("{0:0.00}", ImpT);
        }
        public string GetTotalRecibido()
        {
            return float.Parse(TotalRecibido.Text.Replace('$', ' '), CultureInfo.InvariantCulture).ToString("0.00");
        }
        public string GetRestante()
        {
            return float.Parse(Restante.Text.Replace('$', ' '), CultureInfo.InvariantCulture).ToString("0.00");
        }
        public string GetCambio()
        {
            return float.Parse(Cambio.Text.Replace('$', ' '), CultureInfo.InvariantCulture).ToString("0.00");
        }

        public void PutAmount(string PayMethod, string Amount)
        {
            ActionsSelenium actions = new ActionsSelenium(driver1);

            switch (PayMethod.ToUpper())
            {
                case "EFECTIVO":
                    IWebElement InputEfectivo = driver1.FindElement(By.XPath("//*[@id='AdvanceCollectorModal']//input[@id='01Input']"));
                    actions.MoveToElement(InputEfectivo).SendKeys(Keys.Delete).SendKeys(InputEfectivo, Amount).SendKeys(Keys.Enter).Build().Perform();
                    break;
                case "CHEQUE":
                    break;
                case "TRANSFERENCIA":
                    break;
                case "TARJETA CREDITO":
                    break;
                case "TARJETA DEBITO":
                    break;
                default:
                    break;
            }

        }

        public bool PaidSaleCompleteOrder()
        {
            string cobro = GetImporteTotal();
           PutAmount("EFECTIVO", cobro);
            string TotalRecibodo =GetTotalRecibido();
            string Restante =GetRestante();
            string Cambio = GetCambio();
            ClicBtnConfirmarCobro();
            WaitSpinner();
            return (TotalRecibodo == cobro) && (Restante == "0,00") && (Cambio == "0,00");
        }

    }
}
