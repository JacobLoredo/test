using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.Compras.Actions;
using Automation.Pages.Modules.LocalizadorRefacciones.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Scenarios_Test.Modules.LocalizadorRefacciones
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)]
    public class LocalizadorRefaccionesTest:BaseTest
    {
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        ActionsLocalizador actionsLocalizador;
        
        Menu menu;

        public void Inicializar() {
            try
            {
                credentials = new ConfigQA.Credentials();
                actionsLocalizador = new ActionsLocalizador(Driver);
                actionsLogin = new ActionsLogin(Driver);
                menu = new Menu(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("280");
                menu.clikElementMenu("Localizador de refacciones");
            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
               
            }

        }
        [Test]
        public void BusquedaRefaExistente() {
            Inicializar();
            actionsLocalizador.BuscarSKU("C06070014");
            actionsLocalizador.ClickDetalleResult("C06070014");
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            List<IWebElement> LIST = actionsLocalizador.ObtenerDataRow();
            Assert.IsTrue(actionsLocalizador.CheckMunicipio(LIST));
            actionsLocalizador.ClicFiltroEstado();

            List<IWebElement> LIST2 = actionsLocalizador.ObtenerDataRow();
            Assert.IsTrue(actionsLocalizador.CheckEstado(LIST2));


        }
    }
}
