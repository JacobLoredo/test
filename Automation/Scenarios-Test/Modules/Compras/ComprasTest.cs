using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.Compras.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Pages.Modules.AutorizacionesOrdenesCompra.Actions;
using NUnit.Framework;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using Automation.Reports;
using System.Collections.Generic;
using System.Linq;

namespace Automation.Scenarios_Test.Modules.Compras
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)] 
    public class ComprasTest : BaseTest
    {
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        ActionsCompras ActionsCompras;
        ActionsNuevoPedido actionsNuevoPedido;
        Menu menu;
        public void inicializar()
        {
            try
            {
                credentials = new ConfigQA.Credentials();
                actionsLogin = new ActionsLogin(Driver);
                ActionsCompras = new ActionsCompras(Driver);
                actionsNuevoPedido = new ActionsNuevoPedido(Driver);
                menu = new Menu(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("CESIT ANGELA GERALDINA ALVAREZ DE LUNA");
                menu.clikElementMenu("Compras");
                Assert.IsTrue(ActionsCompras.IsComprasPage());

            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
                throw;
            }
        }
        //Automatizar la busqueda por descripcion 
        [TestCase("SKU existente", Author = "Jacob", Category = "Nuevo Pedido - Busqueda"), Property("ID", 33733)]
        [TestCase("SKU no existente",Author = "Jacob", Category = "Nuevo Pedido - Busqueda"), Property("ID", 33735)]
        public void BusquedaSKUDirecta(string skuExistente) {
            
            bool skuExist = skuExistente.Contains("no") ? false : true;

            inicializar();
            ActionsCompras.ClickNewPedido();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
            if (skuExist) {
                actionsNuevoPedido.searchSKUDirect("LLLLLLLL");
                Assert.IsTrue(actionsNuevoPedido.NoFoundIsVisible());
            }
            else {
                actionsNuevoPedido.searchSKUDirect("123");
                Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
            }
        }
        [TestCase("Avanzada 1", Author = "Jacob", Category = "Nuevo Pedido - Busqueda"), Property("ID", 33748)]
        [TestCase("Avanzada 2",Author = "Jacob", Category = "Nuevo Pedido - Busqueda"), Property("ID", 33749)]
        public void nuevoPedidoBusquedaAvanzada(string busquedaA) {
            try
            {

                inicializar();
                ActionsCompras.ClickNewPedido();
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
                actionsNuevoPedido.ClickBtnBA();
                if (busquedaA.Equals("Avanzada 1"))
                    actionsNuevoPedido.FillBusquedaAvanzada("ITALIKA", "Línea Z", "250 C.C.", "2023", "250ZIII", "NEGRO MATE", "Motor", "Arranque");
                else
                    actionsNuevoPedido.FillBusquedaAvanzada("HERO", "Urbana", "160 C.C.", "2022", "HUNK160R", "ROJO", "Suspensión", "Amortiguación Delantera y Dirección");
                Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }

        [TestCase("Caso 1", Author = "Jacob", Category = "Nuevo Pedido - Busqueda",TestName ="Agregar Refaccion a Lista de pedido"), Property("ID", 33868)]
        [TestCase("Caso 2", Author = "Jacob", Category = "Nuevo Pedido - Busqueda", TestName = "No agregar refaccion descontinuada"), Property("ID", 33759)]
        [TestCase("Caso 3", Author = "Jacob", Category = "Nuevo Pedido - Busqueda",TestName ="Eliminar refaccion de Lista de pedido"), Property("ID", 33759)]
        public void nuevoPedidoVerificacionColumnas(string CasoDePrueba) {

            try
            {
                inicializar();
                ActionsCompras.ClickNewPedido();
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
                
                if (CasoDePrueba.Equals("Caso 1"))
                {
                    if (actionsNuevoPedido.FindRefaccionINPedidos("E12010139"))
                    {
                        actionsNuevoPedido.EliminaRefacionInPedidos("E12010139");
                    }
                    actionsNuevoPedido.ClickBtnBA();
                    actionsNuevoPedido.FillBusquedaAvanzada("ITALIKA", "Línea Z", "250 C.C.", "2023", "250ZIII", "NEGRO MATE", "Motor", "Arranque");
                    Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
                    Assert.IsTrue(actionsNuevoPedido.VerificarInformacionResult("E12010139", "MOTOR ARRANQUE", "Disponible", "Agregar"));
                    actionsNuevoPedido.ObtenerFilaResultadoB("Agregar", "E12010139");
                    //ASSER PARA OBTENER EN EL LISTA DE PEDIDO
                    //Assert.IsTrue(actionsNuevoPedido.FindRefaccionINPedidos("E12010139"));
                }
                else if (CasoDePrueba.Equals("Caso 2"))
                {
                    actionsNuevoPedido.ClickBtnBA();
                    actionsNuevoPedido.FillBusquedaAvanzada("ITALIKA", "Trabajo", "125 C.C.", "2017", "FT125PLATA", "PLATA", "Sistema Eléctrico", "Eléctrico");
                    Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
                    Assert.IsTrue(actionsNuevoPedido.VerificarInformacionResult("F06070005", "CLAXON", "Descontinuada", ""));
                } else if (CasoDePrueba.Equals("Caso 3")) {
                    if (actionsNuevoPedido.FindRefaccionINPedidos("C01040002"))
                    {
                        actionsNuevoPedido.EliminaRefacionInPedidos("C01040002");
                        Assert.IsFalse(actionsNuevoPedido.FindRefaccionINPedidos("C01040002"));
                    }
                    else
                    {
                        actionsNuevoPedido.ClickBtnBA();
                        actionsNuevoPedido.FillBusquedaAvanzada("HERO", "Urbana", "190 C.C.", "2022", "HUNK190R", "NEGRO", "Suspensión", "Amortiguación Delantera y Dirección");
                        Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
                        Assert.IsTrue(actionsNuevoPedido.VerificarInformacionResult("C01040002", "TAZA SUP", "Disponible", "Agregar"));
                        actionsNuevoPedido.ObtenerFilaResultadoB("Agregar", "C01040002");
                        actionsNuevoPedido.closeModalBA();
                        Assert.IsTrue(actionsNuevoPedido.FindRefaccionINPedidos("C01040002"));
                        actionsNuevoPedido.EliminaRefacionInPedidos("C01040002");
                        Assert.IsFalse(actionsNuevoPedido.FindRefaccionINPedidos("C01040002"));

                    }
                }

            }
            catch (AssertionException a)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
               
            }
        }
        [TestCase("Refaccion activa", Author = "Jacob", Category = "Nuevo Pedido - Busqueda"), Property("ID", 33759)]
        [TestCase("Refaccion descontinuada", Author = "Jacob", Category = "Nuevo Pedido - Busqueda"), Property("ID", 33759)]
        public void VerificarElementosModalRefacción(string refacStatus) {
            inicializar();
            ActionsCompras.ClickNewPedido();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
            List<string> CompatEspe = new List<string>();
            if (refacStatus.Equals("Refaccion activa"))
            {
                actionsNuevoPedido.searchSKUDirect("F13030027");
                Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
                IWebElement refaResult= actionsNuevoPedido.ObtenRowRefaBusqueda("F13030027");
                CompatEspe.Add("BLACKBIRD: 2021 - 2022");
                Assert.IsTrue(actionsNuevoPedido.VerificarModalRefa(refaResult, "Activo", "F13030027 BLOQUE GOMA DEC 1 TANQUE",1, CompatEspe));
            }
            else if (refacStatus.Equals("Refaccion descontinuada"))
            {
                actionsNuevoPedido.searchSKUDirect("F04030037");
                Assert.IsTrue(actionsNuevoPedido.IsResultVisible());
                IWebElement refaResult = actionsNuevoPedido.ObtenRowRefaBusqueda("F04030037");
                CompatEspe.Add("EX90T: 2006 - 2007 - 2008");
                CompatEspe.Add("PS90: 2010");
                Assert.IsTrue(actionsNuevoPedido.VerificarModalRefa(refaResult, "Descontinuada", "F04030037 CHICOTE DE ACELERADOR", 1, CompatEspe));
            }
        }

        [TestCase("Proveedor Local", Author = "Jacob", Category = "Nuevo Pedido - Enviar"), Property("ID", 33759)]
        [TestCase("Proveedor Central Fijo Italika", Author = "Jacob", Category = "Nuevo Pedido - Enviar"), Property("ID", 33759)]
        [TestCase("Proveedor Central Variable",Author = "Jacob", Category = "Nuevo Pedido - Enviar"), Property("ID", 33759)]
        [TestCase("Proveedor Central Fijo (MT)", Author = "Jacob", Category = "Nuevo Pedido - Enviar"), Property("ID", 33759)]
        public void EnviarPedidoAutorizar(string TipoProveedor) {
            try
            {
                inicializar();
                ActionsCompras.ClickNewPedido();
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
                if (TipoProveedor.Equals("Proveedor Local"))
                {
                    actionsNuevoPedido.CrearPedido("FUNK671228P16",10,5);//	F13012373 F13011230 F13012396
                }
                else if(TipoProveedor.Equals("Proveedor Central Fijo Italika"))
                {
                    actionsNuevoPedido.CrearPedido("XAXX010101001", 10, 5);
                }
                else if(TipoProveedor.Equals("Proveedor Central Variable"))
                {
                    actionsNuevoPedido.CrearPedido("GUGJ670305V5A", 10, 5);
                }
                else
                {
                    actionsNuevoPedido.CrearPedido("XAXX010101000", 10, 5);
                }
                string infoPedido=actionsNuevoPedido.SelectNavPedidoLast();
                //Assert para verificar clave 
                string tipoCompraSelect=actionsNuevoPedido.SelectTipoCompraRand();
                string skuRefaEliminada = actionsNuevoPedido.eliminarRefaccionPedidoActual();
                //Assert para verificar que la refaccion fue eliminada
                Assert.IsTrue(actionsNuevoPedido.IsToastSuccess());
                string refaSelect= actionsNuevoPedido.SelectRefaRandomInPedidoActual();
                string SKU = refaSelect;
                if (!TipoProveedor.Equals("Proveedor Central Fijo Italika"))
                {
                    //Assert.IsTrue(actionsNuevoPedido.VerificarPrecioRefaInPedido(SKU));
                    actionsNuevoPedido.PutPrecioYDescuentoRefa(SKU, "10", "10");
                    Assert.IsTrue(actionsNuevoPedido.CheckPrecioDescuentoMontoCompra(SKU));
                    Assert.IsTrue(actionsNuevoPedido.CheckEditCantidad(SKU));
                }
                if (!TipoProveedor.Equals("Proveedor Central Fijo Italika"))
                    actionsNuevoPedido.PutPrecioYDescuentoAllRefas();

                Assert.IsTrue(actionsNuevoPedido.CheckDetallesPedidoActual());
                actionsNuevoPedido.PutCommentsToPedido("Pedido registrado por Automatizacion QA");
                string idPedido= actionsNuevoPedido.ClickEnviarPedidoActual();

                AutoOrdenActions AutoOrdenActions = new AutoOrdenActions(Driver);
                string fecha = AutoOrdenActions.ObtenerFechaActual();
                AutoOrdenActions.CheckVerOrden(idPedido, fecha, fecha);
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                Assert.True(AutoOrdenActions.ValidaClaveYFecha(infoPedido, fecha));
            }
            catch (AssertionException a)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
               
            }
        }

        [TestCase(Author = "Jacob", Category = "Nuevo Pedido - Eliminar"), Property("ID", 34069)]
        public void EliminarPedido() {
            try
            {
                inicializar();
                ActionsCompras.ClickNewPedido();
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
                actionsNuevoPedido.CrearPedido("FUNK671228P16", 10, 5);
                string infoPedido = actionsNuevoPedido.SelectNavPedidoLast();
                List<string> ListSKU= actionsNuevoPedido.EliminarPedidoActual();
                Assert.IsTrue(actionsNuevoPedido.VerificarSKUSListaPrincipal(ListSKU));

            }
            catch (AssertionException a)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);

            }
        }
        
        
        [TestCase("Sin pedido en registro", Author = "Jacob", Category = "Nuevo Pedido - Asignacion"), Property("ID", 34072)]
        [TestCase("Con pedido en registro", Author = "Jacob", Category = "Nuevo Pedido - Asignacion"), Property("ID", 34073)]
        public void ReasignarPedidoAProveedor(string registro) {
            inicializar();
            ActionsCompras.ClickNewPedido();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            Assert.IsTrue(actionsNuevoPedido.IsNuevoPedidoPage());
            if (registro.Equals("Sin pedido en registro"))
            {
                actionsNuevoPedido.CrearPedido("DARJ890707TU1", 4, 2);
                int navInicio = actionsNuevoPedido.NavPedidosCount();
               
                string infoPedido = actionsNuevoPedido.SelectNavPedidoLast();

                actionsNuevoPedido.ReasignarRefa("BISE701125GM7");
                //Assert.That(actionsNuevoPedido.NavPedidosCount(), Is.EqualTo(navInicio + 1));
                actionsNuevoPedido.SelectNavPedidoLast();
                actionsNuevoPedido.EliminarPedidoActual();
                actionsNuevoPedido.SelectNavPedidoLast();
                actionsNuevoPedido.EliminarPedidoActual();
            }
            else if (registro.Equals("Con pedido en registro"))
            {
                actionsNuevoPedido.CrearPedido("FUNK671228P16", 4, 2);
                actionsNuevoPedido.SelectNavPedidoLast();
                int numArticulos1 = actionsNuevoPedido.NumSkusInPedido();
                actionsNuevoPedido.SelectNavPedidoByOption(0);

                actionsNuevoPedido.CrearPedido("GUGJ670305V5A", 4, 2);
                string infoPedido = actionsNuevoPedido.SelectNavPedidoLast();
               
                int numArticulos2 = actionsNuevoPedido.NumSkusInPedido();
                actionsNuevoPedido.ReasignarRefa("FUNK671228P16");
                Assert.IsTrue(actionsNuevoPedido.IsToastSuccess());
                actionsNuevoPedido.SelectNavPedidoLast();

                int numArticulos3 = actionsNuevoPedido.NumSkusInPedido();
                actionsNuevoPedido.SelectNavPedidoLast();
                actionsNuevoPedido.EliminarPedidoActual();
                Assert.IsTrue(actionsNuevoPedido.IsToastSuccess());
                actionsNuevoPedido.SelectNavPedidoLast();
//                Assert.That(numArticulos3, Is.EqualTo(numArticulos1 + 1));
                actionsNuevoPedido.EliminarPedidoActual();
                Assert.IsTrue(actionsNuevoPedido.IsToastSuccess());

            }
        }
    
    }
}
