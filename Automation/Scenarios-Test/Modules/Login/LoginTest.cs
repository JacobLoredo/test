using Automation.Config.QA;
using Automation.Pages;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.Login;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Automation.Scenarios_Test.Modules.Login
{
    
   // [Parallelizable]
    [Author("Jacob Loredo", "jacob.loredo@aumenta.mx")]
    public class LoginTest : BaseTest
    {
        
        //[Test, Description("Prueba de un login con credenciales validas"), Property("ID", 1)]
        [Category("Login")]
        [Author("Jacob Loredo", "jacob.loredo@aumenta.mx")]
        public void LoginPageSuccess()
        {
            ConfigQA.Credentials credentials = new ConfigQA.Credentials();  
            ActionsLogin actionsLogin = new ActionsLogin(Driver);
            Menu menu = new Menu(Driver);
            actionsLogin.LoginUser(credentials.user, credentials.password);
            indexPage indexPage = new indexPage(Driver);
            try
            {
               
                Assert.IsTrue(indexPage.isIndexPage());
                //menu.clikElementMenu("Catálogo de proveedores");
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
       // [Test, Description("Prueba de un login con credenciales no validas"), Property("ID", 2)]
        [Category("Login")]
        [Author("Jacob Loredo", "jacob.loredo@aumenta.mx")]
        public void LoginPageCredencialesInvalidas() {

            ConfigQA.Credentials credentials = new ConfigQA.Credentials();
            ActionsLogin actionsLogin = new ActionsLogin(Driver);
            actionsLogin.LoginUser(credentials.user+"aas", credentials.password);
            try
            {
                Assert.IsTrue(actionsLogin.isSpanErrorUserOrPasswordInvalid());
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail,a.Message);
                throw;
            }
        }

        //[Test, Description("Prueba de un login sin credenciales"), Property("ID", 3)]
        [Category("Login")]
        [Author("Jacob Loredo", "jacob.loredo@aumenta.mx")]
        public void LoginPageCredencialesVacio()
        {
            ActionsLogin actionsLogin = new ActionsLogin(Driver);
            actionsLogin.LoginUser("","");
            try
            {
                Assert.IsTrue(actionsLogin.isSpanErrorUserAndPassword());
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
        //[Test, Description("Prueba de un login ingresando contrasena vacia"), Property("ID", 4)]
        [Category("Login")]
        [Author("Jacob Loredo", "jacob.loredo@aumenta.mx")]
        public void LoginPagePasswordVacia()
        {
            ConfigQA.Credentials credentials = new ConfigQA.Credentials();
            ActionsLogin actionsLogin = new ActionsLogin(Driver);
            actionsLogin.LoginUser(credentials.user, "");
            try
            {
                Assert.IsTrue(actionsLogin.isSpanErrorPasswordVisible());
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
        //[Test, Description("Prueba de un login ingresando usuario vacia"), Property("ID", 5)]
        [Category("Login")]
        [Author("Jacob Loredo", "jacob.loredo@aumenta.mx")]
        public void LoginPageUserNamedVacia()
        {
            ConfigQA.Credentials credentials = new ConfigQA.Credentials();
            ActionsLogin actionsLogin = new ActionsLogin(Driver);
            actionsLogin.LoginUser("", credentials.password);
            try
            {
                Assert.IsTrue(actionsLogin.isSpanErrorUserNameVisible());
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
    }
}
