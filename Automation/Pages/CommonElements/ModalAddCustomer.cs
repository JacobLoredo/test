using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.CommonElements
{
    public class ModalAddCustomer:BasePage
    {
        public IWebDriver driver1;
        public ModalAddCustomer(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//div[@id='CustomerType']//div[contains(@class,'customer-type-card')]")]
        public IWebElement OptionCatItalika { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='LocalProviderOption']")]
        public IWebElement OptionOwnClient { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@data-dismiss='modal' and text()='Cancelar']")]
        public IWebElement BtnCancelar { get; set; }

        public void ClickAddClientItalika()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de '{OptionCatItalika.Text}'");
            OptionCatItalika.Click();
        }
        public void ClickOwnClient()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de '{OptionOwnClient.Text}'");
            OptionOwnClient.Click();
        }
        public void ClickBotonCancelar()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de cancelar");
            BtnCancelar.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }

    }
}
