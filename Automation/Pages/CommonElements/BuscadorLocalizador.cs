using Automation.Reports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.Pages.CommonElements
{
    
    public class BuscadorLocalizador:BasePage
    {
        public IWebDriver driver1;
        public BuscadorLocalizador(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);

        }
        
        [FindsBy(How = How.XPath, Using = "//a[@data-tab-name='SKU']")]
        public IWebElement tabSKU { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//a[@data-tab-name='Description']")]
        public IWebElement tabDescription { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//a[@data-tab-name='Advanced']")]
        public IWebElement tabAdvanced { get; set; }
        
        
        [FindsBy(How = How.XPath, Using = "//*[@id='skuSearchId']")]
        public IWebElement InputSearch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='BtnSearchBySku']")]
        public IWebElement BtnSearchBySku { get; set; }


        private void PutInputSearch(string search)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            Thread.Sleep(3500);
            Reporter.LogPassingTestStepForBugLogger($"Valor a buscar: '{search}'");
            InputSearch.SendKeys(search);
        }
        private void ClickBotonBuscar()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de buscar");
            driver1.FindElement(By.XPath("//*[@id='skuSearchId']")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        private void ClickTabSKU()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de buscar");
            tabSKU.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        private void ClickTabDescription()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de buscar");
            tabDescription.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        private void ClickTabAdvanced()
        {
            Reporter.LogPassingTestStepForBugLogger($"Click al boton de buscar");
            tabAdvanced.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
        public void searchBySKU(string SKU) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            ClickTabSKU();
            PutInputSearch(SKU);
            ClickBotonBuscar();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
        }
    }
}
