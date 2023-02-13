using Automation.Pages.CommonElements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages
{
    public class indexPage : BasePage
    {

        public indexPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//div[@class='panel']/div/div/div[@class='texto1']/strong")]
        public IWebElement textBienvenida { get; set; }


        public bool isIndexPage() {
            return textBienvenida.Displayed;
        }

    }
}
