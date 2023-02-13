

using Automation.Reports;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Automation.Pages.CommonElements
{
    public  class Menu:BasePage
    {
        public IWebDriver driver1;
        public Menu(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//ul[@id='menuContainer']/li[@class='site-menu-item'or @class='site-menu-item ']/*")]
        public IList<IWebElement> listMenuEnlaces { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[@id='menuContainer']/li[@class='site-menu-item'or @class='site-menu-item ']/a/span")]
        public IList<IWebElement> listMenuSpan { get; set; }
        public string spanSearchWE = "//span[contains(text(),'";

        public void clikElementMenu(string ElementMenu) {
            try
            {
                IWebElement ElementMenuBy = waitElementToClick(by: By.XPath(spanSearchWE + ElementMenu + "')]"));
                Reporter.LogPassingTestStepForBugLogger($"Llendo al modulo '{ElementMenu}'. ");
                   
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }

    }
}
