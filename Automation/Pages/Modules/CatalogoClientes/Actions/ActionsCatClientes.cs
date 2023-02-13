using Automation.Pages.CommonElements;
using Automation.Pages.Modules.CatalogoClientes.PageElements;
using Automation.Pages.Modules.CatalogoProvedores.PageElements;
using Automation.Reports;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;

namespace Automation.Pages.Modules.CatalogoClientes.Actions
{
    public class ActionsCatClientes:CatClientesPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }

        public ActionsCatClientes(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        public void GoToPage()
        {
            Reporter.LogTestStepForBugLogger(Status.Info, "Ir a la pagina de Catalogo de clientes");
            Driver.Navigate().GoToUrl(config.UrlPage + "/CustomersCatalog");
            Reporter.LogPassingTestStepForBugLogger($"Abriendo URL=>{config.UrlPage + "/CustomersCatalog"}");

        }
        public bool IsCatalogoCliente() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[@class='page-title']")));
            return catClientesElements.h1Titulo.Text.Equals("Catálogo de clientes");
        }
        public bool IsSearchClientPage()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[@class='page-title']")));
            return catClientesElements.h1Titulo.Text.Equals("Buscar cliente en catálogo de Italika");
        }
        public void ClicBtnAddClient() {
            catClientesElements.BtnAddCustumer.Click();
        }
        public void ClicBtnBakcBusqueda()
        {
            ScrollToElement(0,0,0,0);
            catClientesElements.BtnBackBusqueda.Click();
        }

        public void SearchClientCatItalika(string name) {
            BuscadorCliente buscador = new BuscadorCliente(Driver1);
            ModalAddCustomer modalAddCustomer = new ModalAddCustomer(Driver1);
            modalAddCustomer.ClickAddClientItalika();
            buscador.SearchClientName(name);
        }
        public void ClicAddOwnClient() {
            ModalAddCustomer modalAddCustomer = new ModalAddCustomer(Driver1);
            modalAddCustomer.ClickOwnClient();
        }
        public void ClicCancelAddClient()
        {
            ModalAddCustomer modalAddCustomer = new ModalAddCustomer(Driver1);
            modalAddCustomer.ClickBotonCancelar();
            Thread.Sleep(1000);
        }
        public IWebElement SelectClientRandom() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> ListClients = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'central-customers-container')]/div")));
            Random rand = new Random();
            int num = rand.Next(ListClients.Count);
            return ListClients[num];

        }
        public IWebElement SelectClientListRandom() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> ListClients = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'row users-container')]/div[@data-central-customer-id and @data-customer-id]/div/div/div[@class='row d-flex align-items-lg-center text-to-disable ']/../../..")));
            Random rand = new Random();
            int num = rand.Next(ListClients.Count);
            return ListClients[num];
        }
        public bool FindClientLocalByName(string nameClient) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            List<IWebElement> ListClients = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'row users-container')]/div[not(@data-central-customer-id)]")));
            bool found = false;
            foreach (IWebElement client in ListClients)
            {
                try
                {
                    string idClient= client.GetAttribute("data-customer-id");
                    string xpath = "//b[@id-" + idClient+"-local-name='"+nameClient.ToUpper()+"']";
                    IWebElement NameClient = Driver1.FindElement(By.XPath(xpath));
                    found = true;
                    break;
                }
                catch (NoSuchElementException)
                {

                   
                }
            }
            return found;
        }

        public bool ActionsClientLocal(string nameClient, string action) {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'row users-container')]/div[not(@data-central-customer-id)]")));
            List<IWebElement> ListClients = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'row users-container')]/div[not(@data-central-customer-id)]")));
            bool found = false;
           
            foreach (IWebElement client in ListClients)
            {
                try
                {
                    Thread.Sleep(400);
                    string idClient = client.GetAttribute("data-customer-id");
                    string xpath = "//b[@id-" + idClient + "-local-name='" + nameClient.ToUpper() + "']";
                    IWebElement NameClient = Driver1.FindElement(By.XPath(xpath));
                    OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
                    WaitSpinner();
                    switch (action.ToLower())
                    {
                        case ("eliminar"):
                                IWebElement BtnEliminar = Driver1.FindElement(By.XPath("//i[contains(@class,'customer-delete') and @data-customer-id='" + idClient + "']"));
                                actions.MoveToElement(BtnEliminar);
                                BtnEliminar.Click();
                                waitElementToClick(swal2ButtonAceptar);
                                found = true;
                            break;
                        case ("editar"):
                                IWebElement BtnEditar = Driver1.FindElement(By.XPath("//i[contains(@class,'customer-edit') and @data-customer-id='" + idClient + "']"));
                                actions.MoveToElement(BtnEditar);
                                BtnEditar.Click();
                                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                                found = true;
                            break;
                        case ("activar"):
                                IWebElement BtnActivar = Driver1.FindElement(By.XPath("//input[@type='checkbox' and @class='js-switch-small' and @data-customer-id='" + idClient + "']/../span"));
                                actions.MoveToElement(BtnActivar);
                                BtnActivar.Click();
                                waitElementToClick(swal2ButtonAceptar);
                                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                            break;
                        default:
                            break;
                    }
                }
                catch (NoSuchElementException)
                {


                }
            }
            return found;
        }
        public bool DeleteClientLocalByName(string nameClient)
        {
            List<IWebElement> ListClients = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'row users-container')]/div[not(@data-central-customer-id)]")));
            bool found = false;
            foreach (IWebElement client in ListClients)
            {
                try
                {
                    string idClient = client.GetAttribute("data-customer-id");
                    string xpath = "//b[@id-" + idClient + "-local-name='" + nameClient.ToUpper() + "']";
                    IWebElement NameClient = Driver1.FindElement(By.XPath(xpath));
                    IWebElement BtnEliminar = Driver1.FindElement(By.XPath("//i[contains(@class,'customer-delete') and @data-customer-id='"+ idClient+"']"));
                    ScrollToElement(BtnEliminar.Location.X, BtnEliminar.Location.Y,0,-100);
                    BtnEliminar.Click();
                    waitElementToClick(swal2ButtonAceptar);
                    found = true;
                    break;
                }
                catch (NoSuchElementException)
                {


                }
            }
            return found;
        }
        public bool EditClientLocalByName(string nameClient) {
            List<IWebElement> ListClients = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[contains(@class,'row users-container')]/div[not(@data-central-customer-id)]")));
            bool found = false;
            foreach (IWebElement client in ListClients)
            {
                try
                {
                    string idClient = client.GetAttribute("data-customer-id");
                    string xpath = "//b[@id-" + idClient + "-local-name='" + nameClient.ToUpper() + "']";
                    IWebElement NameClient = Driver1.FindElement(By.XPath(xpath));
                    IWebElement BtnEditar = Driver1.FindElement(By.XPath("//i[contains(@class,'customer-edit') and @data-customer-id='" + idClient + "']"));
                    ScrollToElement(BtnEditar.Location.X, BtnEditar.Location.Y, 0, -100);
                    BtnEditar.Click();
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                    found = true;
                    break;
                }
                catch (NoSuchElementException)
                {


                }
            }
            return found;
        }
        public string ReturnCustomerIDC(IWebElement clientRow) {
            return clientRow.GetAttribute("data-customer-id");
        }
        public void ClickAddCustomer(IWebElement clientRow) {
            string id = clientRow.GetAttribute("data-customer-id");
            string xpath = "//button[@data-customer-id='" + id + "']";
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));
            IWebElement btnAddClient = clientRow.FindElement(By.XPath(xpath));
            ScrollToElement(btnAddClient.Location.X, btnAddClient.Location.Y,0,80);
            btnAddClient.Click();   
        }
        public bool IsClientInCatalogo(string idClient) {
            IWebElement clientRow;
            try
            {
                clientRow = Driver1.FindElement(By.XPath("//div[@data-central-customer-id='"+idClient+"']"));
                return clientRow.Displayed;
            }
            catch (NoSuchElementException)
            {

               return false;
            }
        
        }

        private void PutModalAltaNombre(string Nombre) {
            Reporter.LogPassingTestStepForBugLogger($"Dato general de nombre:  '{Nombre}'");
            catClientesElements.modalAltaInputNombre.Clear();
            catClientesElements.modalAltaInputNombre.SendKeys(Nombre);
        }
        private void PutModalAltaTelefono(string Tel)
        {
            Reporter.LogPassingTestStepForBugLogger($"Dato general de telefono:  '{Tel}'");
            catClientesElements.modalAltaInputTel.Clear();
            catClientesElements.modalAltaInputTel.SendKeys(Tel);
        }
        private void PutModalAltaCorreo(string Correo)
        {
            Reporter.LogPassingTestStepForBugLogger($"Dato general de correo:  '{Correo}'");
            catClientesElements.modalAltaInputCorreo.Clear();
            catClientesElements.modalAltaInputCorreo.SendKeys(Correo);
        }
        private void PutModalAltaCP(string CP)
        {
            Reporter.LogPassingTestStepForBugLogger($"Dato general de codigo postal:  '{CP}'");
            catClientesElements.modalAltaInputCPDom.Clear();
            catClientesElements.modalAltaInputCPDom.SendKeys(CP);
            Thread.Sleep(500);
        }

        private void PutModalAltaClasificacion(string Clasificacion) {

            switch (Clasificacion.ToLower())
            {
                case ("público general"):
                    IWebElement labelPublicG = Driver1.FindElement(By.XPath("//*[@id='1']//parent::div/label"));
                    labelPublicG.Click();
                    
                    break;
                case ("flotilla particular"):
                    IWebElement labelFlotilla = Driver1.FindElement(By.XPath("//*[@id='2']//parent::div/label"));
                    labelFlotilla.Click();
                    break;
                case ("flotilla baz"):
                    IWebElement labelFlotillaBAZ = Driver1.FindElement(By.XPath("//*[@id='3']//parent::div/label"));
                    labelFlotillaBAZ.Click();
                    break;
                default:
                    break;
            }

        }
        private void PutTipoCompra(string TipoCompra) {

            switch (TipoCompra.ToLower())
            {
                case ("mayoreo"):
                    IWebElement labelMayoreo = Driver1.FindElement(By.XPath("//*[@id='Wholesale']//parent::div/label"));
                    labelMayoreo.Click();
                    break;
                case ("menudeo"):
                    IWebElement labelMenudeo = Driver1.FindElement(By.XPath("//*[@id='Retail']//parent::div/label"));
                    labelMenudeo.Click();
                    break;
                default:
                    break;
            }
        }
        public void FillDatosGeneralesAlta(string nombre,string tel,string correo,string CP,string clasificacion,string  TipoCompra) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='Name']")));
            PutModalAltaNombre(nombre);
            PutModalAltaTelefono(tel);
            PutModalAltaCorreo(correo);
            PutCodigoPostal(CP);
            PutModalAltaClasificacion(clasificacion);
            PutTipoCompra(TipoCompra);
        }
        public void ClickContactoSecundario() {
            Reporter.LogPassingTestStepForBugLogger($"Habilitando secundo contacto");
            catClientesElements.modalAltaCheckContacto2.Click();
        }
        public void ClickDatosFacturacion()
        {
            Reporter.LogPassingTestStepForBugLogger($"Habilitando datos de facturacion");
            catClientesElements.modalAltaCheckFacturacion.Click();
        }
        public void ClickAddCredito()
        {
            Reporter.LogPassingTestStepForBugLogger($"Habilitando Credito ");
            catClientesElements.modalAltaCheckCredito.Click();
        }
        public void ClickAddDescuento()
        {
            Reporter.LogPassingTestStepForBugLogger($"Habilitando Credito ");
            catClientesElements.modalAltaCheckCDescuento.Click();
        }
        public void FillSecondContactAlta(string nombre,string telefono,string correo) {
            PutSecondContactName(nombre);
            PutSecondContactTel(telefono);
            PutSecondContactCorreo(correo);
        }
        private void PutSecondContactName(string name)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando nombre del secundo contacto:  '{name}'");
            catClientesElements.modalAltaInputContacto2Nombre.Clear();
            catClientesElements.modalAltaInputContacto2Nombre.SendKeys(name);

        }
        private void PutSecondContactTel(string tel)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando telefono del segundo contacto:  '{tel}'");
            catClientesElements.modalAltaInputContacto2Tel.Clear();
            catClientesElements.modalAltaInputContacto2Tel.SendKeys(tel);

        }
        private void PutSecondContactCorreo(string Correo)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando correo del segundo contacto:  '{Correo}'");
            catClientesElements.modalAltaInputContacto2Correo.Clear();
            catClientesElements.modalAltaInputContacto2Correo.SendKeys(Correo);

        }
        private void PutRazonSocial(string RazonSocial) {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando la razon social :  '{RazonSocial}'");
            catClientesElements.modalAltaInputRazonSocial.Clear();
            catClientesElements.modalAltaInputRazonSocial.SendKeys(RazonSocial);
        }
        private void PutRFC(string RFC)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando RFC :  '{RFC}'");

            catClientesElements.modalAltaInputRFC.Clear();
            ActionsSelenium actions = new ActionsSelenium(Driver1);
            actions.SendKeys(catClientesElements.modalAltaInputRFC, RFC).Build().Perform();
        }
        private void PutModalTipoPer(string TipoPer)
        {
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando tl tipo de persona: '{TipoPer}'");

            switch (TipoPer.ToLower())
            {
                case ("moral"):
                    IWebElement labelMoral = Driver1.FindElement(By.XPath("//*[@id='Moral']//parent::div/label"));
                    labelMoral.Click();
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='InvoicingRegime']")));
                    break;
                case ("física"):
                    IWebElement labelFisica = Driver1.FindElement(By.XPath("//*[@id='Physical']//parent::div/label"));
                    labelFisica.Click();
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='InvoicingRegime']")));
                    break;
                default:
                    break;
            }

        }
        private void PutModalRegimenFiscal(string RegimenF)
        {
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='InvoicingRegime']")));
            SelectElement ModalSelectRegiFis = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='InvoicingRegime']")));
            ModalSelectRegiFis.SelectByText(RegimenF);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el regimen fiscal '{RegimenF}'");
        }
        private void PutModalMunicipio(string Municipio)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='InvoicingCity']")));
            SelectElement ModalSelectMuni = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='InvoicingCity']")));
            ModalSelectMuni.SelectByText(Municipio.ToUpper());
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando colonia '{Municipio}'");
        }



        private void PutModalCFDI(string CFDI)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='InvoicingCFDI']")));
            SelectElement ModalSelectCFDI = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='InvoicingCFDI']")));
            ModalSelectCFDI.SelectByText(CFDI);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el CFDI '{CFDI}'");
        }
        private void PutCodigoPostal(string CP) {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando codigo postal :  '{CP}'");
            catClientesElements.modalAltaInputCP.Clear();
            catClientesElements.modalAltaInputCP.SendKeys(CP);

        }
        private void PutCalle(string Calle)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando calle  :  '{Calle}'");
            catClientesElements.modalAltaInputCalleDom.Clear();
            catClientesElements.modalAltaInputCalleDom.SendKeys(Calle);

        }
        private void PutNumExt(string NumExt)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando numero exterior  :  '{NumExt}'");
            catClientesElements.modalAltaInputNumExt.Clear();
            catClientesElements.modalAltaInputNumExt.SendKeys(NumExt);

        }
        private void PutNumInt(string NumInt)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando numero exterior  :  '{NumInt}'");
            catClientesElements.modalAltaInputNumInt.Clear();
            catClientesElements.modalAltaInputNumInt.SendKeys(NumInt);
        }
        private void PutColonia(string Colonia)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando colonia  :  '{Colonia}'");
            catClientesElements.modalAltaInputColonia.Clear();
            catClientesElements.modalAltaInputColonia.SendKeys(Colonia);
        }
        private void PutMunicipio(string PutMunicipio)
        {
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='InvoicingCity']")));
            SelectElement ModalSelectMunicipio = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='InvoicingCity']")));
            ModalSelectMunicipio.SelectByText(PutMunicipio);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el municipio '{PutMunicipio}'");

        }
        
        public void FillDatosFacturacion(string RazonSocial,string RFC,string TipoPersona,string RegimenFiscal,
            string CFDI,string CP,string calle,string numExt,string numInt,string colonia,string Municipio
            ) {
            PutRazonSocial(RazonSocial);
            PutRFC(RFC);
            PutModalTipoPer(TipoPersona);
            PutModalRegimenFiscal(RegimenFiscal);
            PutModalCFDI(CFDI);
            PutModalAltaCP(CP);
            PutCalle(calle);
            PutNumExt(numExt);
            PutNumInt(numInt);
            PutColonia(colonia);
            PutMunicipio(Municipio);
        }
       private void PutMontoCredito(string monto)
       {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando monto de credito  :  '{monto}'");
            catClientesElements.modalAltaInputMontoCredito.Clear();
            catClientesElements.modalAltaInputMontoCredito.SendKeys(monto);
       }
        private void PutDiasCredito(string dias)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando dias de credito  :  '{dias}'");
            catClientesElements.modalAltaInputDiasCredito.Clear();
            catClientesElements.modalAltaInputDiasCredito.SendKeys(dias);
        }
        public void FillCreditoCliente(string monto,string dias) {
            PutMontoCredito(monto);
            PutDiasCredito(dias);
        }
        public void PutDescuentoCliente(string descuento)
        {
            Reporter.LogPassingTestStepForBugLogger($"Ingresando % de descuento  :  '{descuento}'");
            catClientesElements.modalAltaInputDescuento.Clear();
            catClientesElements.modalAltaInputDescuento.SendKeys(descuento);
        }
        public void ClickBotonGuardarCliente() {
            catClientesElements.BtnAddCustumerModal.Click();
        }
        public void ClickBotonGuardarEditCliente()
        {
            Thread.Sleep(500);
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
            actions.MoveToElement(catClientesElements.BtnEditCustumerModal);
            catClientesElements.BtnEditCustumerModal.Click();
        }
        public void AddOwnClientTest() {
            ClicBtnAddClient();
            ClicAddOwnClient();
            FillDatosGeneralesAlta("Prueba 6", "4444444444", "prueba6@gmail.com", "78000", "flotilla baz", "Menudeo");
            ClickContactoSecundario();

            FillSecondContactAlta("CONTACTO PRUEBA 6", "4444444444", "contactoprueba6@gmail.com");
            ClickDatosFacturacion();
            FillDatosFacturacion(RazonSocial: "prueba 6 sa de cv", RFC: "H&E951128469", TipoPersona: "Moral", RegimenFiscal: "610 - Residentes en el Extranjero sin Establecimiento Permanente en México"
                , CFDI: "G03 - Gastos en general", CP: "78000", calle: "Centro", numExt: "8", numInt: "A", colonia: "centro", Municipio: "SAN LUIS POTOSÍ");

            ClickAddCredito();
            FillCreditoCliente("50000", "13");
            ClickAddDescuento();
            PutDescuentoCliente("12");
            ClickBotonGuardarCliente();
        }
    }
}
