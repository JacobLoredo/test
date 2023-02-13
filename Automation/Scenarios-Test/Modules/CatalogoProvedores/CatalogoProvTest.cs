using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.CatalogoProvedores.Actions;
using Automation.Pages.Modules.Compras.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;
using static Automation.Config.QA.ConfigQA;


namespace Automation.Scenarios_Test.Modules.CatalogoProvedores
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)]
    public class CatalogoProvTest : BaseTest
    {
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        ActionsCatProv actionsCatProv;

        Menu menu;
        public void inicializar()
        {
            try
            {
                credentials = new ConfigQA.Credentials();
                actionsLogin = new ActionsLogin(Driver);
                actionsCatProv = new ActionsCatProv(Driver);
                menu = new Menu(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("CESIT ANGELA GERALDINA ALVAREZ DE LUNA");
                menu.clikElementMenu("Catálogo de proveedores");
                Assert.IsTrue(actionsCatProv.IsCatalogoProvPage());

            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);
                throw;
            }
        }
        //[Test, Description("Realizar busqueda de proveedor por Nombre existente"), Property("ID", 33275), Order(3)]
        //[Category("Busqueda")]
        [TestCase("RFC", Author = "Jacob", Category = "Busqueda"), Property("ID", 33273), Order(1)]
        [TestCase("Clave del proveedor", Author = "Jacob", Category = "Busqueda"), Property("ID", 33274)]
        [TestCase("Nombre del proveedor", Author = "Jacob", Category = "Busqueda"), Property("ID", 33275)]
        public void BusquedaNombreExistente(string busquedaX)
        {
            inicializar();

            try
            {
                IWebElement act = actionsCatProv.ChoiceProvedorCentral(busquedaX);
                var text = act.Text.Replace("\n", "").Split('\r').ToList<string>();
                actionsCatProv.ClickAddProvedor();
                actionsCatProv.SelectTypeProvedorItalika();
                actionsCatProv.BuscarProvedorBy(busquedaX);
                var textBuscar = ActionsCatProv.ObtenerDatoBuscar(text, busquedaX);
                Reporter.LogTestStepForBugLogger(Status.Info, $"Buscando {busquedaX} '{textBuscar}'");
                if (!busquedaX.Equals("Clave del proveedor"))
                    actionsCatProv.searchInput.SendKeys(textBuscar);
                else {
                    string textSplit = textBuscar.Remove(0, 2);
                    actionsCatProv.CatProvElements.InputClaveProv.SendKeys(textSplit.Split('-')[0]);

                }
                Reporter.LogPassingTestStepForBugLogger($"Click en el boton '{actionsCatProv.CatProvElements.btnSearch.Text}'");
                actionsCatProv.CatProvElements.btnSearch.Click();
                Thread.Sleep(4000);

                Assert.IsTrue(actionsCatProv.ProvedorIsFound(text, busquedaX));
                Reporter.LogPassingTestStepForBugLogger($"El provedor central fue encontrado por {busquedaX}");

            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
        //[Test, Description("Agregar un nuevo proveedor central"), Property("ID", 33303), Order(3)]
        [Test]
        [Category("Agregar"), Property("ID", 33280)]
        public void agregarNuevoProveedorCentral() {
            inicializar();

            try
            {
                actionsCatProv.ClickAddProvedor();
                actionsCatProv.SelectTypeProvedorItalika();
                Reporter.LogTestStepForBugLogger(Status.Info, $"Buscando por Nombre CESIT");
                actionsCatProv.BuscarProvedorBy("Nombre del proveedor");
                actionsCatProv.searchInput.SendKeys("CESIT");
                actionsCatProv.CatProvElements.btnSearch.Click();
                //Escoger un provedor para agregar a mis provedores
                IWebElement ProvedorSearch = actionsCatProv.ChoiceProvedorCentralSearch();
                actionsCatProv.ClickAddProveedorCentralSearch(ProvedorSearch);
                actionsCatProv.ClickToButtonsModalProveedor("Cancelar");
                actionsCatProv.ClickAddProveedorCentralSearch(ProvedorSearch);
                actionsCatProv.ClickToButtonsModalProveedor("Aceptar");

                Assert.IsTrue(actionsCatProv.IsToastSuccess());

            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
        [TestCase("todos los datos", Author = "Jacob", Category = "Agregar"), Property("ID", 33306)]
        [TestCase("datos fiscales existentes", Author = "Jacob", Category = "Agregar"), Property("ID", 33525)]
        public void agregarProveedorPropioPaso1(string expectError) {
            bool expError = (expectError == "datos fiscales existentes") ? true : expectError == "todos los datos" ? false : false;
            inicializar();
            try
            {
                IWebElement RepitP = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                if (RepitP != null && !expError)
                {
                    actionsCatProv.SelecAccionProveedor(RepitP, "Eliminar");
                    actionsCatProv.clickSwal2Button("Aceptar");
                }
                else if (expError && RepitP == null)
                {
                    actionsCatProv.AgregarProveedorLocal();
                }

                actionsCatProv.ClickAddProvedor();
                actionsCatProv.SelectTypeProvedorPropio();
                Assert.IsTrue(actionsCatProv.IsModalInfoGeneral());
                actionsCatProv.FillAddProviderLocalStep1(RazonSocial:"PROVEEDOR 999 SA DE CV",NombreProv: "Proveedor 999",RFC:"RLJA671228PH6", "Física", "Proveedor", "605 - Sueldos y Salarios e Ingresos Asimilados a Salarios", "G01 - Adquisición de mercancias", "Todos",isEdit:false);
                actionsCatProv.ClickModalNext();
                if (!expError)
                {
                    //Assert.IsTrue(actionsCatProv.IsModalContacto());
                    actionsCatProv.FillAddProviderLocalStep2(CP: "78000", Calle: "Reforma", NumExt: "128", Colonia: "Centro", Municipio: "SAN LUIS POTOSÍ", NombreContacto1: "LUIS ORTEGA", Correo1: "dev.radarcontroltotal@gmail.com", Telefono1: "4444444444", Dias: "Lunes,Martes,Miércoles,Jueves,Viernes");
                    actionsCatProv.PutHoraToDay("Lunes", "08:00:00", "18:00:00", true);
                    actionsCatProv.ClickModalNext();
                    Assert.IsTrue(actionsCatProv.IsModalCredito());
                    actionsCatProv.FillAddProviderLocalStep3(isCredito: "No", tiempoEntrega: "12");
                    actionsCatProv.ClickModalNext();
                    Assert.IsTrue(actionsCatProv.IsModalConfirmacion());
                    actionsCatProv.ClickModalAddProviderOwn();
                    Assert.IsTrue(actionsCatProv.IsToastSuccess(text: "Se guardó el proveedor correctamente."));

                }
                else
                {
                    Assert.IsTrue(actionsCatProv.IsToastError());
                    actionsCatProv.ClickBtnModalCancel();
                    actionsCatProv.clickSwal2Button("Aceptar");
                    RepitP = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                    actionsCatProv.SelecAccionProveedor(RepitP, "Eliminar");
                    actionsCatProv.clickSwal2Button("Aceptar");
                }

            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }

        }

        [Test]
        [Author("Jacob")]
        [Category("Agregar"), Property("ID", 33420)]
        public void agregarProveedorPropioPaso2() {
            inicializar();
           
            try
            {
               
                IWebElement RepitP = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                if (RepitP != null)
                {
                    actionsCatProv.SelecAccionProveedor(RepitP, "Eliminar");
                    actionsCatProv.clickSwal2Button("Aceptar");
                }

                actionsCatProv.ClickAddProvedor();
                actionsCatProv.SelectTypeProvedorPropio();
                Assert.IsTrue(actionsCatProv.IsModalInfoGeneral());
                actionsCatProv.FillAddProviderLocalStep1(RazonSocial:"PROVEEDOR 999 SA DE CV", NombreProv: "Proveedor 999", Tpersona: "Física", Tprove:"Proveedor",Grupo:"Todos");
                actionsCatProv.ClickModalNext();
                Thread.Sleep(2000);
                Assert.IsTrue(actionsCatProv.IsModalContacto());
                actionsCatProv.FillAddProviderLocalStep2(CP: "78000", Calle: "Reforma", NumExt: "128", Colonia: "Centro", Municipio: "SAN LUIS POTOSÍ", 
                    NombreContacto1: "LUIS ORTEGA", Correo1: "dev.radarcontroltotal@gmail.com", Telefono1: "4444444444", Dias: "Lunes,Martes,Miércoles,Jueves,Viernes",isContacto2:true,NombreContacto2:"Jacob Loredo",Correo2:"Jacob.loredo@hotmail.com",Telefono2_1:"4422552211",Telefono2_2:"1234567890");
                actionsCatProv.PutHoraToDay("Lunes", "08:00:00", "18:00:00", true);

                actionsCatProv.ClickModalNext();
                Assert.IsTrue(actionsCatProv.IsModalCredito());
                actionsCatProv.FillAddProviderLocalStep3(isCredito: "No", tiempoEntrega: "12");
                actionsCatProv.ClickModalNext();
                Assert.IsTrue(actionsCatProv.IsModalConfirmacion());
                actionsCatProv.ClickModalAddProviderOwn();

                Assert.IsTrue(actionsCatProv.IsToastSuccess(text:"Se guardó el proveedor correctamente."));

                RepitP = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                actionsCatProv.SelecAccionProveedor(RepitP, "Eliminar");
                actionsCatProv.clickSwal2Button("Aceptar");
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }

        [TestCase("Proveedor sin linea de credito", Author = "Jacob", Category = "Agregar"), Property("ID", 33501)]
        [TestCase("Proveedor con linea de credito", Author = "Jacob", Category = "Agregar"), Property("ID", 33502)]
        public void agregarProveedorPropioPaso3(string lineaCredito)
        {
            bool expError = (lineaCredito == "Proveedor sin linea de credito") ? true : lineaCredito == "Proveedor con linea de credito" ? false : false;
            inicializar();
            try
            {
                IWebElement RepitP = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                if (RepitP != null)
                {
                    actionsCatProv.SelecAccionProveedor(RepitP, "Eliminar");
                    actionsCatProv.clickSwal2Button("Aceptar");
                }

                actionsCatProv.ClickAddProvedor();
                actionsCatProv.SelectTypeProvedorPropio();
                Assert.IsTrue(actionsCatProv.IsModalInfoGeneral());
                actionsCatProv.FillAddProviderLocalStep1("PROVEEDOR 999 SA DE CV", "Proveedor 999", "ARLJ671228PH6", "Física", "Proveedor", "605 - Sueldos y Salarios e Ingresos Asimilados a Salarios", "G01 - Adquisición de mercancias", "Todos");
                actionsCatProv.ClickModalNext();
//                Assert.IsTrue(actionsCatProv.IsModalContacto());
                actionsCatProv.FillAddProviderLocalStep2(CP: "78000", Calle: "Reforma", NumExt: "128", Colonia: "Centro", Municipio: "SAN LUIS POTOSÍ", NombreContacto1: "LUIS ORTEGA", Correo1: "dev.radarcontroltotal@gmail.com", Telefono1: "4444444444", Dias: "Lunes,Martes,Miércoles,Jueves,Viernes");
                actionsCatProv.PutHoraToDay("Lunes", "08:00:00", "18:00:00", true);
                actionsCatProv.ClickModalNext();
                Assert.IsTrue(actionsCatProv.IsModalCredito());
                if (expError)
                {
                    actionsCatProv.FillAddProviderLocalStep3(isCredito: "No", tiempoEntrega: "12");
                }
                else
                {
                    actionsCatProv.FillAddProviderLocalStep3(isCredito: "Si", tiempoEntrega: "3",montoC: "5000", DiasC:"14");
                }
                actionsCatProv.ClickModalNext();
                Assert.IsTrue(actionsCatProv.IsModalConfirmacion());

            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }

        }

        [Test]
        [TestCase("Proveedor local", Author = "Jacob", Category = "Visualizacion"), Property("ID", 33298)]
        [TestCase("Proveedor variable",Author = "Jacob", Category = "Visualizacion"), Property("ID", 33297)]
        [TestCase("Proveedor Fijo", Author = "Jacob", Category = "Visualizacion"), Property("ID", 33298)]
        public void visualizacionListadoPrincipal(string tipoProvedor) {
            inicializar();
            try
            {
               
                //proveedor central(variable)
                IWebElement act;
                if (tipoProvedor.Equals("Proveedor variable"))
                {
                    act = actionsCatProv.ChoiceProvedorCentral("RFC");
                    actionsCatProv.SelecAccionProveedor(act, "Ver detalles");
                    Assert.IsTrue(actionsCatProv.IsModalVerDetallesProvedor());
                    actionsCatProv.ClickCloseModalDetalles();
                    actionsCatProv.SelecAccionProveedor(act, "Editar");
                    Thread.Sleep(500);
                    Assert.IsTrue(actionsCatProv.IsModalEditarProveedorCentral());
                    actionsCatProv.ClickCancelarModalEditar();
                    actionsCatProv.SelecAccionProveedor(act, "Eliminar");
                    Thread.Sleep(800);
                    actionsCatProv.clickSwal2Button("Cancelar");
                }
                else if (tipoProvedor.Equals("Proveedor local"))
                {
                    actionsCatProv.AgregarProveedorLocal();
                    IWebElement ProveedorLocal = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                    actionsCatProv.SelecAccionProveedor(ProveedorLocal, "Ver detalles");
                    Assert.IsTrue(actionsCatProv.IsModalVerDetallesProvedor());
                    actionsCatProv.ClickCloseModalDetalles();
                    actionsCatProv.SelecAccionProveedor(ProveedorLocal, "Editar");
                    Assert.IsTrue(actionsCatProv.IsModalEditarProveedorLocal());
                    actionsCatProv.ClickBtnModalCancel();
                    actionsCatProv.SelecAccionProveedor(ProveedorLocal, "Eliminar");
                    actionsCatProv.clickSwal2Button("Cancelar");
                } else if (tipoProvedor.Equals("Proveedor Fijo")) {
                    IWebElement ProveedorFijo = actionsCatProv.BuscaProveedor("ITALIKA");
                    actionsCatProv.SelecAccionProveedor(ProveedorFijo, "Ver detalles");
                    Assert.IsTrue(actionsCatProv.IsModalVerDetallesProvedor());
                    actionsCatProv.ClickCloseModalDetalles();
                    actionsCatProv.SelecAccionProveedor(ProveedorFijo, "Editar");
                    Assert.IsTrue(actionsCatProv.IsModalEditarProveedorCentral());
                    actionsCatProv.ClickCancelarModalEditar();
                }
                //If si es provedor local en con cadena 'Editar proveedor' con el central es 'Editar linea de credito'

            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }

        [Test, Description("Realizar la eliminacion de un proveedor central sin relaciones")]
        [TestCase("proveedor central variable sin registros relacionados", Author = "Jacob", Category = "Eliminacion"), Property("ID", 33299)]
        //[TestCase("proveedor central variable con registros relacionados", Author = "Jacob", Category = "Eliminacion"), Property("ID", 33300)]
        [TestCase("proveedor local sin registros relacionados", Author = "Jacob", Category = "Eliminacion"), Property("ID", 33301)]
        //[TestCase("proveedor local con registros relacionados", Author = "Jacob", Category = "Eliminacion"), Property("ID", 33302)]
        public void EliminarProveedor(string tprovedor)
        {
            inicializar();
            try
            {
                if (tprovedor.Equals("proveedor central variable sin registros relacionados")|| tprovedor.Equals("proveedor central variable con registros relacionados"))
                {
                    IWebElement proveedorCentral = actionsCatProv.ChoiceProvedorCentral("RFC");
                    if(proveedorCentral == null)
                    {
                        actionsCatProv.AgregarProveedorCentralAzar();
                    }
                    var text = proveedorCentral.Text.Replace("\n", "").Split('\r').ToList<string>();
                    actionsCatProv.SelecAccionProveedor(proveedorCentral, "Eliminar");
                    actionsCatProv.clickSwal2Button("Aceptar");
                    if (actionsCatProv.IsToastError())
                    {
                        IWebElement ProvAux = actionsCatProv.ChoiceProvedorCentral("RFC");
                        ProvAux = actionsCatProv.ChoiceProvedorCentral("RFC");
                        text = ProvAux.Text.Replace("\n", "").Split('\r').ToList<string>();
                        actionsCatProv.SelecAccionProveedor(ProvAux, "Eliminar");
                        actionsCatProv.clickSwal2Button("Aceptar");
                    }
                    Thread.Sleep(2000);
                    IWebElement ProveedorLocalNoFound = actionsCatProv.BuscaProveedor(text[1]);
                    if (tprovedor.Equals("proveedor central variable sin registros relacionados"))
                        Assert.That(ProveedorLocalNoFound, Is.EqualTo(null));
                    else
                        Assert.True(actionsCatProv.IsToastError());
                }
                else if (tprovedor.Equals("proveedor local sin registros relacionados") || tprovedor.Equals("proveedor local con registros relacionados"))
                {
                    if (tprovedor.Equals("proveedor local sin registros relacionados"))
                    {
                        IWebElement RepitP = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                        if (RepitP != null)
                        {
                            actionsCatProv.SelecAccionProveedor(RepitP, "Eliminar");
                            actionsCatProv.clickSwal2Button("Aceptar");
                            IWebElement ProveedorLocalNoFound = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                            Assert.That(ProveedorLocalNoFound, Is.EqualTo(null));
                        }
                        else
                        {
                            actionsCatProv.AgregarProveedorLocal();
                            IWebElement ProveedorLocal = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                            actionsCatProv.SelecAccionProveedor(ProveedorLocal, "Eliminar");
                            Thread.Sleep(2000);
                            actionsCatProv.clickSwal2Button("Aceptar");
                            Thread.Sleep(2000);
                            IWebElement ProveedorLocalNoFound = actionsCatProv.BuscaProveedor("PROVEEDOR 999 SA DE CV");
                            Assert.That(ProveedorLocalNoFound, Is.EqualTo(null));
                        }
                    }
                }


            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }



        [TestCase("Editar", Author = "Jacob", Category = "Edicion"), Property("ID", 33521)]
        [TestCase("Eliminar", Author = "Jacob", Category = "Eliminacion"), Property("ID", 33522)]
        public void LineaCreditoProveedorCentral(string lineaCredito) {
            inicializar();
            try
            {
                IWebElement act;
               
                act = actionsCatProv.ChoiceProvedorCentral("RFC");
                actionsCatProv.SelecAccionProveedor(act, "Editar");
                Assert.IsTrue(actionsCatProv.IsModalEditarProveedorCentral());
                if (actionsCatProv.LineaCreditoSeleccionada() == "No" && lineaCredito.Equals("Editar"))
                {
                    actionsCatProv.LineaCreditoEditar("Si");
                    actionsCatProv.ModificarLineaCredito("120000", "60", "12");
                    actionsCatProv.ClickBtnGuardarLineC();
                    Assert.IsTrue(actionsCatProv.IsToastSuccess());

                }
                else if (actionsCatProv.LineaCreditoSeleccionada() == "Si" && lineaCredito.Equals("Editar")) { 
                    actionsCatProv.ModificarLineaCredito("120000", "60", "12");
                    actionsCatProv.ClickBtnGuardarLineC();
                    Assert.IsTrue(actionsCatProv.IsToastSuccess());

                }
                else if (actionsCatProv.LineaCreditoSeleccionada() == "No" && lineaCredito.Equals("Eliminar")) { 
                    Assert.IsFalse(actionsCatProv.MontoYDiasCreditoBlock());
                    actionsCatProv.LineaCreditoEditar("Si");
                    actionsCatProv.ModificarLineaCredito("150000", "20", "35");
                    actionsCatProv.ClickBtnGuardarLineC();
                    Assert.IsTrue(actionsCatProv.IsToastSuccess());
                    actionsCatProv.SelecAccionProveedor(act, "Editar");
                    actionsCatProv.LineaCreditoEditar("No");
                    Assert.IsFalse(actionsCatProv.MontoYDiasCreditoBlock());
                    actionsCatProv.ClickBtnGuardarLineC();
                    Assert.IsTrue(actionsCatProv.IsToastSuccess());
                }
                else
                {
                    Assert.IsTrue(actionsCatProv.MontoYDiasCreditoBlock());
                    actionsCatProv.LineaCreditoEditar("No");
                    Assert.IsFalse(actionsCatProv.MontoYDiasCreditoBlock());
                    actionsCatProv.ClickBtnGuardarLineC();

                }
               


            }
            catch (NoSuchElementException a)
            {

                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }

        /*
        [TestCase("Desactivar","Proveedor central variable", Author = "Jacob", Category = "Acciones"), Property("ID", 33512)]
        [TestCase("Activar", "Proveedor central variable", Author = "Jacob", Category = "Acciones"), Property("ID", 33515)]
        [TestCase("Desactivar", "Proveedor local", Author = "Jacob", Category = "Acciones"), Property("ID", 33514)]
        [TestCase("Activar", "Proveedor local", Author = "Jacob", Category = "Acciones"), Property("ID", 33516)]
        */
        public void ActivarDesactivarProveedor(string Accion, string Tproveedor) {
            inicializar();
            try
            {
               
                IWebElement proveedor=null;
                string idProvider="";
                bool isProvCentral = true;
                if (Tproveedor.Equals("Proveedor central variable"))
                {

                    proveedor = actionsCatProv.ChoiceProvedorCentral("RFC");
                }
                else
                {
                    isProvCentral = false;
                    proveedor = actionsCatProv.ChoiceProvedorLocal();
                }
                idProvider = proveedor.GetAttribute("data-provider-id");
                actionsCatProv.SelecAccionProveedor(proveedor, Accion, idProvider: idProvider,isCentral: isProvCentral);
                if (Accion.Equals("Desactivar"))
                {
                    if (actionsCatProv.MensajeSwalConfirmacion("Desactivar proveedor", "¿Desea desactivar a este proveedor?"))
                    {
                        Assert.IsTrue(actionsCatProv.MensajeSwalConfirmacion("Desactivar proveedor", "¿Desea desactivar a este proveedor?"));
                        actionsCatProv.clickSwal2Button("Aceptar");
                        Assert.IsTrue(actionsCatProv.IsToastSuccess());

                    }
                    else {
                        actionsCatProv.clickSwal2Button("Aceptar");
                        Thread.Sleep(2500);
                        actionsCatProv.SelecAccionProveedor(proveedor, Accion, idProvider: idProvider, isCentral: isProvCentral);
                        Assert.IsTrue(actionsCatProv.MensajeSwalConfirmacion("Desactivar proveedor", "¿Desea desactivar a este proveedor?"));
                        actionsCatProv.clickSwal2Button("Aceptar");
                    }
                }
                else
                {
                    if (actionsCatProv.MensajeSwalConfirmacion("Activar proveedor", "¿Deseas activar a este proveedor?")) {
                        Assert.IsTrue(actionsCatProv.MensajeSwalConfirmacion("Activar proveedor", "¿Deseas activar a este proveedor?"));
                        actionsCatProv.clickSwal2Button("Aceptar");
                    }
                    else
                    {
                        actionsCatProv.clickSwal2Button("Aceptar");
                        Thread.Sleep(2500);
                           
                        actionsCatProv.SelecAccionProveedor(proveedor, Accion,idProvider: idProvider, isCentral: isProvCentral);
                        Assert.IsTrue(actionsCatProv.MensajeSwalConfirmacion("Activar proveedor", "¿Deseas activar a este proveedor?"));
                        actionsCatProv.clickSwal2Button("Aceptar");
                    }
                    Thread.Sleep(1000);
                       
                    Assert.IsTrue(actionsCatProv.IsToastSuccess());
                }

              
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }

        [TestCase("Campos editables", Author = "Jacob", Category = "Edicion"), Property("ID", 33677)]
        [TestCase("Campos no editables", Author = "Jacob", Category = "Edicion"), Property("ID", 33678)]
        public void EditarTodosLosCamposProveedorLocal(string editables) {
            inicializar();
            try
            {
                
                IWebElement proveedor = actionsCatProv.ChoiceProvedorLocal();
                actionsCatProv.SelecAccionProveedor(proveedor, "Editar");
                Assert.IsTrue(actionsCatProv.IsModalInfoGeneral());
                if (editables.Equals("Campos no editables"))
                {
                    Assert.IsTrue (actionsCatProv.FillsNoEditablesProvider());
                }
                else
                {
                    actionsCatProv.FillAddProviderLocalStep1(NombreProv:"Proveedor 999",Tpersona:"Física",Tprove:"Proveedor", Grupo: "Refacciones", isEdit:true);
                    actionsCatProv.ClickModalNext();
                    actionsCatProv.FillAddProviderLocalStep2(CP: "03820",Calle: "16 DE SEPTIEMBRE  EDITADO",NumExt: "8",NumInter:"8", Colonia: "CENTRO EDITADO",Municipio:"TLALPAN",
                        NombreContacto1: "LUIS ORTEGA MENA EDITADO",Correo1: "EDITADO.dev.radarcontroltotal@gmail.com",
                        Telefono1: "8888888888", isContacto2:true,Telefono2: "8888888888",NombreContacto2: "LUIS ORTEGA MENA  2 EDITADO",
                        Correo2: "EDITADO.dev2.radarcontroltotal@gmail.com",Telefono2_1: "9999999999",Telefono2_2: "9999999999",
                        SitioWeb: "editado.italika.com.mx", Dias: "Lunes,Martes,Miércoles,Jueves,Viernes,Sabado,Domingo",isEdit:true);

                    actionsCatProv.PutHoraToDay("Lunes", "09:00:00", "17:00:00", true);
                    actionsCatProv.ClickModalNext();
                    //Assert.IsTrue(actionsCatProv.IsModalCredito());
                    actionsCatProv.FillAddProviderLocalStep3(isCredito: "Si", tiempoEntrega: "5", montoC: "500000",DiasC: "30");
                    actionsCatProv.ClickModalNext();
                    Assert.IsTrue(actionsCatProv.IsModalConfirmacion());
                    actionsCatProv.ClickModalAddProviderOwn();
                    Assert.IsTrue(actionsCatProv.IsToastSuccess(text: "Se guardó el proveedor correctamente."));
                }

            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }
        }
    }
}
