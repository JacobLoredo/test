using OpenQA.Selenium;
using SeleniumExtras.PageObjects;


namespace Automation.Pages.Modules.Login
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[@id='Username']")]
        public IWebElement inputUsuario { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='Password']")]
        public IWebElement inputPassword { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='loginButton']")]
        public IWebElement buttonSubmit { get; set; }


        [FindsBy(How = How.XPath, Using = "//img[@id='logoRadar']")]
        public IWebElement imagenRadar { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@data-valmsg-for='Password']")]
        public IWebElement spanErrorPassword { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@data-valmsg-for='Username']")]
        public IWebElement spanErrorUserName { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='validation-summary-errors text-danger']//ul//li")]
        public IWebElement spanErrorUserOrPasswordInvalid { get; set; }
        public bool isLoginPage() { 
            return imagenRadar.Displayed;
        }
        public bool isSpanErrorPasswordVisible() {
            return spanErrorPassword.Displayed;
        }
        public bool isSpanErrorUserNameVisible()
        {
            return spanErrorUserName.Displayed;
        }
        public bool isSpanErrorUserAndPassword() {
            return (spanErrorPassword.Displayed && spanErrorPassword.Displayed);
        }
        public bool isSpanErrorUserOrPasswordInvalid()
        {
            return spanErrorUserOrPasswordInvalid.Displayed;
        }

    }
}
