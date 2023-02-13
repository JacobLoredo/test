using Automation.Reports;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.Login.Actions
{
    public class ActionsLogin : LoginPage
    {
        private IWebDriver Driver;
        private LoginPage login;

        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public LoginPage Login { get => login; set => login = value; }

        public ActionsLogin(IWebDriver driver) : base(driver)
        {
            Driver = driver;
            login = new LoginPage(Driver);
        }
        public  void LoginUser(string username,string password) {
            Reporter.LogTestStepForBugLogger(Status.Info, "Ingresando para logear usuario");
            if (Driver.Url!=login.config.UrlPage)
                login.moveLoginPage();
            clearFields(login);
            login.inputUsuario.SendKeys(username);
            login.inputPassword.SendKeys(password);
            login.buttonSubmit.Click(); 
        }
        public void clearFields(LoginPage login) {
            login.inputPassword.Clear();
            login.inputUsuario.Clear();
        }
    }
}
