using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.AutorizacionesOrdenesCompra.Actions;
using Automation.Pages.Modules.Compras.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.Scenarios_Test.Modules.AutorizacionesOrdenes
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)]
    public class AutorOrdComTest:BaseTest
    {
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        ActionsCompras ActionsCompras;
        ActionsNuevoPedido actionsNuevoPedido;
        Menu menu;
        AutoOrdenActions AutoOrdenActions;

        public void Inicializar() {
            try
            {
                credentials = new ConfigQA.Credentials();
                actionsLogin = new ActionsLogin(Driver);
                actionsNuevoPedido = new ActionsNuevoPedido(Driver);
                AutoOrdenActions = new AutoOrdenActions(Driver);
                menu = new Menu(Driver);
                ActionsCompras = new ActionsCompras(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("CESIT ANGELA GERALDINA ALVAREZ DE LUNA");
                menu.clikElementMenu("Autorizaciones");
                Assert.IsTrue(AutoOrdenActions.IsAutoOrdenPage());
            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
                throw;
            }
            

        }
        [TestCase("Completa",Author = "Jacob", Category = "Pestaña ODC - Autorizar"), Property("ID", 34391)]
        [TestCase("Parcial", Author = "Jacob", Category = "Pestaña ODC - Autorizar"), Property("ID", 34390)]
        public void AutorizarOrdenCompra(string statusOrden) {
            Inicializar();
            try
            {
                int numArticulos = 5;
                List<string> infoOrdenPedido = actionsNuevoPedido.CreaPedido("XAXX010101001", "Central Fijo Italika", numArticulos);
                string fecha = AutoOrdenActions.ObtenerFechaActual();
                AutoOrdenActions.SearchOrden(fecha, fecha);
                AutoOrdenActions.ClickVerOrden(infoOrdenPedido[1]);
                Assert.True(AutoOrdenActions.ValidaClaveYFecha(infoOrdenPedido[0], fecha));
                if(statusOrden.Equals("Completa"))
                    AutoOrdenActions.AutorizarOrden("Completa");
                else if(statusOrden.Equals("Parcial"))
                    AutoOrdenActions.AutorizarOrden("Parcial", "Comentario de QA");


                Assert.IsTrue(ActionsCompras.CheckEstatusOrden("Autorizado",fecha, fecha,infoOrdenPedido[1]));
                //Verificar que adentro tenga el estatus de autorizado
                ActionsCompras.ClickVerOrden(infoOrdenPedido[1]);
                Assert.True(AutoOrdenActions.ValidaClaveYFecha(infoOrdenPedido[0], fecha));
                if (statusOrden.Equals("Completa"))
                    Assert.That(AutoOrdenActions.NumArticulosDetalle(), Is.EqualTo(numArticulos));



            }
            catch (AssertionException a)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
            }
        }
        [TestCase("Refacción no existente", Author = "Jacob", Category = "Pestaña ODC - Eliminar"), Property("ID", 34388)]
        [TestCase("Refacción existente", Author = "Jacob", Category = "Pestaña ODC - Eliminar"), Property("ID", 34389)]
        public void RechazarOrdenCompra(string statusOrden)
        {
            Inicializar();
            try
            {
                List<string> infoOrdenPedido = actionsNuevoPedido.CreaPedido("XAXX010101001", "Central Fijo Italika", 5);
                string fecha = AutoOrdenActions.ObtenerFechaActual();
                AutoOrdenActions.SearchOrden(fecha, fecha);
                AutoOrdenActions.ClickVerOrden(infoOrdenPedido[1]);
                Assert.True(AutoOrdenActions.ValidaClaveYFecha(infoOrdenPedido[0], fecha));
                List<string> refasDetalle= AutoOrdenActions.ObtenerSkuRefasDetalle();

                if (statusOrden.Equals("Refacción no existente"))
                {

                    AutoOrdenActions.RechazarOrden("Esto es un rechazon de QA");
                    Assert.IsTrue(ActionsCompras.CheckEstatusOrden("Rechazado", fecha, fecha, infoOrdenPedido[1]));
                    //Ir al la lista de pedidos y verificar que esten todas las refacciones
                    ActionsCompras.ClickNewPedido();
                    Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                    //Assert.IsTrue(actionsNuevoPedido.FindRefasById(refasDetalle));
                }
                else if (statusOrden.Equals("Refacción existente"))
                {
                    //Crar un nuevo pedido con al menos 1 refaccion

                    //Abrir una nueva ventana de nuevo pedido,buscar el sku, agregarlo a un pedido,cerrar la ventana, regresar al rechazar la orden
                    //Buscar el sku en el listado principal y no deberia de estar
                    string refaSelected = refasDetalle.First();
                    List<string> ListSku = new List<string>();
                    ListSku.Add(refaSelected);
                    actionsNuevoPedido.OpenNewTab();
                    actionsNuevoPedido.GoToPage();
                    actionsNuevoPedido.searchSKUDirect(refaSelected);
                    actionsNuevoPedido.ObtenerFilaResultadoB("Agregar",refaSelected);
                    actionsNuevoPedido.closeModalBA();
                    actionsNuevoPedido.FindRefacionListaArticulos(refaSelected, "Seleccionar");
                    actionsNuevoPedido.AsignarProveedor("XAXX010101001");
                    Driver.Close();
                    Driver.SwitchTo().Window(Driver.WindowHandles.Last());

                    AutoOrdenActions.RechazarOrden("Esto es un rechazon de QA");
                    Assert.IsTrue(ActionsCompras.CheckEstatusOrden("Rechazado", fecha, fecha, infoOrdenPedido[1]));
                    //Ir al la lista de pedidos y verificar que esten todas las refacciones
                    ActionsCompras.ClickNewPedido();
                    Driver.SwitchTo().Window(Driver.WindowHandles.Last());

                    //Assert.IsFalse(actionsNuevoPedido.FindRefasById(ListSku));
                }

              
            }
            catch (AssertionException a)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
            }
        }
        [TestCase("Autorizado", Author = "Jacob", Category = "Pestaña ODC - Modificar status"), Property("ID", 34614)]
        [TestCase("Rechazado", Author = "Jacob", Category = "Pestaña ODC - Modificar status"), Property("ID", 34614)]
        [TestCase("Cancelada por usuario", Author = "Jacob", Category = "Pestaña ODC - Modificar status"), Property("ID", 34614)]
        [TestCase("En tránsito", Author = "Jacob", Category = "Pestaña ODC - Modificar status"), Property("ID", 34614)]
        [TestCase("Pendiente de autorización", Author = "Jacob", Category = "Pestaña ODC - Modificar status"), Property("ID", 34614)]
        public void CambiarStatusODC(string Status) {
            Inicializar();

            try
            {
                switch (Status)
                {
                    case ("Rechazado"):
                            AutoOrdenActions.CrearYRechazarODC();
                        break;
                    case ("Pendiente de autorización"):
                        AutoOrdenActions.CrearODC();
                        break;
                    default:
                        break;
                }
                if (!Status.Equals("Rechazado")&&!Status.Equals("Pendiente de autorización"))
                {
                    AutoOrdenActions.CrearYAutorizarODC();

                }
                if (!Status.Equals("Pendiente de autorización"))
                    AutoOrdenActions.ClickChanceStatus();
               
                //Ya se creo el ODC y se autorizo
                if (!Status.Equals("Cancelada por usuario")&& !Status.Equals("En tránsito")&&!Status.Equals("Pendiente de autorización") ) { 
                    Assert.IsTrue(AutoOrdenActions.CheckEstatusActual(Status));
                    Assert.IsTrue(AutoOrdenActions.CheckStatusDisponibles(Status));
                }

                switch (Status)
                {
                    case ("Autorizado"):
                            Assert.IsTrue(AutoOrdenActions.ChangeStatus("Cancelada por usuario"));
                            Assert.IsTrue(AutoOrdenActions.ChangeStatus("En tránsito"));
                        break;
                    case ("Cancelada por usuario"):
                            Assert.IsTrue(AutoOrdenActions.ChangeStatus(Status));
                            AutoOrdenActions.ClickAceptChangeStatusBtn();
                            Assert.IsTrue(AutoOrdenActions.CheckEstatusActual(Status));
                        break;
                    case ("En tránsito"):
                            Assert.IsTrue(AutoOrdenActions.ChangeStatus(Status));
                            AutoOrdenActions.ClickAceptChangeStatusBtn();
                            Thread.Sleep(1000);
                            AutoOrdenActions.ClickChanceStatus();
                            Assert.IsTrue(AutoOrdenActions.CheckEstatusActual(Status));
                            Assert.IsTrue(AutoOrdenActions.CheckStatusDisponibles(Status));
                        break;
                    case ("Pendiente autorizacion"):
                        Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                        Assert.IsTrue(AutoOrdenActions.CheckEstatusActual(Status));
                        break;
                    default:
                        break;
                }



            }
            catch (Exception)
            {

                throw;
            }
        
        }
    }
}
