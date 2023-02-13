using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.CatalogoClientes.Actions;
using Automation.Pages.Modules.CatalogoProvedores.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Threading;

namespace Automation.Scenarios_Test.Modules.CatalogoClientes
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)]
    public class CatalogoClientesTest:BaseTest
    {
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        ActionsCatClientes catClientes;
        Menu menu;
     
        public void Inicializar() {
            try
            {
                credentials = new ConfigQA.Credentials();
                actionsLogin = new ActionsLogin(Driver);
                catClientes = new ActionsCatClientes(Driver);
                menu = new Menu(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("CESIT ANGELA GERALDINA ALVAREZ DE LUNA");
                menu.clikElementMenu("Catálogo de clientes");
                Assert.IsTrue(catClientes.IsCatalogoCliente());

            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
                throw;
            }
        }


        [Test]
        public void AgregarNuevoCliente()
        {
            Inicializar();
            try
            {
                catClientes.ClicBtnAddClient();
                catClientes.SearchClientCatItalika("Cesit");
                Assert.IsTrue(catClientes.IsSearchClientPage());
                IWebElement firtsClient = catClientes.SelectClientRandom();
                catClientes.ClickAddCustomer(firtsClient);
                catClientes.clickSwal2Button("Cancelar");
                Thread.Sleep(600);
                IWebElement SecondClient = catClientes.SelectClientRandom();
                string idClient = catClientes.ReturnCustomerIDC(SecondClient);
                catClientes.ClickAddCustomer(SecondClient);
                catClientes.clickSwal2Button("Aceptar");
                catClientes.WaitSpinner();
                catClientes.ClicBtnBakcBusqueda();
                catClientes.WaitSpinner();
                Assert.IsTrue(catClientes.IsCatalogoCliente());
                Assert.IsTrue(catClientes.IsClientInCatalogo(idClient));

            }
            catch (System.Exception a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
                throw;
            }

        }

        [TestCase( Author = "Jacob", Category = "Nuevo Cliente - Agregar"), Property("ID", 33733)]
        public void AgregarNuevoClientePropio()
        {
            Inicializar();
            try
            {
                catClientes.ClicBtnAddClient();
                catClientes.ClicAddOwnClient();
                catClientes.FillDatosGeneralesAlta("Prueba 6", "4444444444", "prueba6@gmail.com", "78000", "flotilla baz","Menudeo");
                catClientes.ClickContactoSecundario();

                catClientes.FillSecondContactAlta("CONTACTO PRUEBA 6", "4444444444", "contactoprueba6@gmail.com");
                catClientes.ClickDatosFacturacion();
                catClientes.FillDatosFacturacion(RazonSocial: "prueba 6 sa de cv",RFC: "H&E951128469",TipoPersona: "Moral",RegimenFiscal: "610 - Residentes en el Extranjero sin Establecimiento Permanente en México"
                    ,CFDI: "G03 - Gastos en general",CP: "78000",calle: "Centro",numExt: "8",numInt: "A",colonia: "centro",Municipio: "SAN LUIS POTOSÍ");

                catClientes.ClickAddCredito();
                catClientes.FillCreditoCliente("50000","13");
                catClientes.ClickAddDescuento();
                catClientes.PutDescuentoCliente("12");
                catClientes.ClickBotonGuardarCliente();
                Assert.IsTrue(catClientes.IsToastSuccess());
                catClientes.ClicCancelAddClient();
                Assert.IsTrue(catClientes.FindClientLocalByName("Prueba 6"));
                Assert.IsTrue(catClientes.ActionsClientLocal("Prueba 6","Eliminar"));

            }
            catch (System.Exception a)
            {
                 Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
                throw;  
                
            }

        }

        [TestCase("Local", Author = "Jacob", Category = "Nuevo Cliente - Edicion"), Property("ID", 34972)]
        public void EditarCliente(string TipoCliente) {
            Inicializar();
            if (TipoCliente.Equals("Local"))
            {
                catClientes.AddOwnClientTest();
                Assert.IsTrue(catClientes.IsToastSuccess());
                catClientes.ClicCancelAddClient();
                //desactivar cliente
                
                catClientes.ActionsClientLocal("Prueba 6", "activar");
                Assert.IsTrue(catClientes.IsToastSuccess());
                //activar cliente
                catClientes.ActionsClientLocal("Prueba 6", "activar");
                Assert.IsTrue(catClientes.IsToastSuccess());
                catClientes.ActionsClientLocal("Prueba 6","Editar");
                catClientes.FillDatosGeneralesAlta("Prueba 1 editado", "0000000000", "prueba1editado@gmail.com", "78049", "flotilla particular", "Menudeo");
                catClientes.FillSecondContactAlta("contacto prueba 1", "0000000000", "contacto@gmail.com");
                catClientes.FillDatosFacturacion(RazonSocial: "prueba1 sa de cv", RFC: "funk671228PH6", TipoPersona: "Física", RegimenFiscal: "605 - Sueldos y Salarios e Ingresos Asimilados a Salarios"
                   , CFDI: "G01 - Adquisición de mercancias", CP: "78049", calle: "Reforma", numExt: "12", numInt: "B", colonia: "Reforma", Municipio: "SAN LUIS POTOSÍ");
                catClientes.FillCreditoCliente("50000","12");
                catClientes.PutDescuentoCliente("10");
                catClientes.ClickBotonGuardarEditCliente();
                Assert.IsTrue(catClientes.IsToastSuccess());
                Thread.Sleep(1000);
                Assert.IsTrue(catClientes.FindClientLocalByName("PRUEBA 1 EDITADO"));
                Assert.IsTrue(catClientes.ActionsClientLocal("Prueba 1 editado", "Eliminar"));
            }
            else if (TipoCliente.Equals("Central"))
            {

            }
        }
    }
}
