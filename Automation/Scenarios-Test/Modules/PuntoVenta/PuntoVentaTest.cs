using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.LocalizadorRefacciones.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Pages.Modules.PuntoVenta.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using DocumentFormat.OpenXml.Spreadsheet;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using static Automation.Config.QA.ConfigQA;

namespace Automation.Scenarios_Test.Modules.PuntoVenta
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)]
    public class PuntoVentaTest : BaseTest
    {
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        ActionsSalesPoint actionsSalesPoint;
        Menu menu;

        [SetUp]
        public void Inicializar()
        {
            try
            {
                credentials = new ConfigQA.Credentials();
                actionsSalesPoint = new ActionsSalesPoint(Driver);
                actionsLogin = new ActionsLogin(Driver);
                menu = new Menu(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("280");
                menu.clikElementMenu("Punto de venta");
            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);

            }
        }

        [TestCase("Publico General",false, Author = "Jacob", Category = "Nuevo Cotizacion"), Property("ID", 35493)]
        [TestCase("Diferente a Publico General",false, Author = "Jacob", Category = "Nuevo Cotizacion"), Property("ID", 35494)]
        [TestCase("Publico General", true, Author = "Jacob", Category = "Editar Cotizacion"), Property("ID", 35493)]
        [TestCase("Diferente a Publico General", true, Author = "Jacob", Category = "Editar Cotizacion"), Property("ID", 35493)]
        public void VerDetalleCotizacion(string tipoCliente,bool IsEdit) {
           
            try
            {
                actionsSalesPoint.AddRefasToCaja(3, "Foco");
                if (tipoCliente.Equals("Publico General"))
                    actionsSalesPoint.SearchClientByKey("1");
                else
                    actionsSalesPoint.SearchClientByKey("CC4619-00000001");
                actionsSalesPoint.PutPrecioYCantidadAllRefas();
                actionsSalesPoint.EditAmountAllRefas();

                Assert.IsTrue(actionsSalesPoint.CheckPurchaseAmount());
                Assert.IsTrue(actionsSalesPoint.CheckAmountToPay());
                actionsSalesPoint.PutDescuentoAllRefas();
                Assert.IsTrue(actionsSalesPoint.CheckAmountToPay());
                actionsSalesPoint.UpdateTerms("Terminos y condiciones cambiados por Automatizacion QA");
                actionsSalesPoint.SelectTypeQuotation();
                string NumCotizacion=actionsSalesPoint.PrintQuotation();
                Assert.That(actionsSalesPoint.CloseQuotation(), Is.EqualTo(1));
                actionsSalesPoint.CloseQuotationSendMailModal();
                actionsSalesPoint.CloseTypeQuotation();
                actionsSalesPoint.ClicHistory();
                actionsSalesPoint.LookQuotationByFolio(NumCotizacion);
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                Assert.IsTrue(actionsSalesPoint.ValidInfoQuotation(NumCotizacion));

                if (tipoCliente.Equals("Publico General")&& IsEdit) {
                    ActionsEditQuotation actionsEditQuotation = new ActionsEditQuotation(Driver);
                    actionsEditQuotation.EditQuotation("GENERAL");
                    Assert.IsTrue(actionsEditQuotation.ValidateNewQuotation(NumCotizacion));
                }
                else if(!tipoCliente.Equals("Publico General") && IsEdit)
                {
                    ActionsEditQuotation actionsEditQuotation = new ActionsEditQuotation(Driver);
                    actionsEditQuotation.EditQuotation("DIFERENTE");
                    Assert.IsTrue(actionsEditQuotation.ValidateNewQuotation(NumCotizacion));

                }


            }
            catch (NoSuchElementException)
            {

              
            }
          

        }

        [TestCase("Monto exacto", Author = "Jacob", Category = "Nuevo Orden Venta-Cotizacion"), Property("ID", 36096)]
        [TestCase("Monto mayor", Author = "Jacob", Category = "Nuevo Orden Venta-Cotizacion"), Property("ID", 36097)]
        public void GenerarOrdenVenta(string tipo) {
          string NumCotizacion=  actionsSalesPoint.CreateQuotationWithInventory();
            actionsSalesPoint.ClicHistory();
            ActionsHistorySalesPoint actionsHistory = new ActionsHistorySalesPoint(Driver);
            actionsHistory.LookQuotationByFolio(NumCotizacion);
            //actionsHistory.LookSaleByFolio(NumCotizacion);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            ActionsDetalle actionsDetalle = new ActionsDetalle(Driver);
            actionsDetalle.ClicGenerarOrdenVenta();
            if (tipo.Equals("Monto exacto")) { 
                Assert.IsTrue(actionsDetalle.PaidSaleCompleteOrder());
                string FolioVenta = actionsDetalle.ImprimeTicket();
                Assert.That(actionsSalesPoint.CloseQuotation(), Is.EqualTo(2));
                actionsDetalle.CloseModalCompraFinalizada();
                actionsHistory.LookSaleByFolio(FolioVenta);
            }
            else if(tipo.Equals("Monto mayor"))
            {
                Assert.IsTrue(actionsDetalle.PaidMoreSaleOrder());
                actionsDetalle.DeleteQuotation();
                Assert.IsFalse(actionsHistory.LookQuotationByFolio(NumCotizacion));


            }
        }
       
        [TestCase( Author = "Jacob", Category = "Nuevo Orden Venta"), Property("ID", 36099)]
        public void GenerarOrdenVentaDirecta() {
            string NumCotizacion = actionsSalesPoint.CreateSaleWithInventory();
            actionsSalesPoint.ClicHistory();
            ActionsHistorySalesPoint actionsHistory = new ActionsHistorySalesPoint(Driver);
            actionsHistory.LookSaleByFolio(NumCotizacion);

        }
    
    }
}
