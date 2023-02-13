using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.CatalogoProvedores.PageElements;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;

namespace Automation.Pages.Modules.CatalogoProvedores.Actions
{
    public class ActionsCatProv : CatalogoProvedoresPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }

        public ActionsCatProv(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        public bool IsCatalogoProvPage() {
            Thread.Sleep(2000);
            return CatProvElements.h1Titulo.Displayed;
        }
        public void SelectTypeProvedorItalika() {
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{CatProvElements.modalProviderCatalogoPItalika.Text}'");
            CatProvElements.modalProviderCatalogoPItalika.Click();
        }
        public void SelectTypeProvedorPropio()
        {
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{CatProvElements.modalProviderAddPropio.Text}'");
            CatProvElements.modalProviderAddPropio.Click();
            Driver1.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }
        public void BuscarProvedorBy(string TipoBusqueda) {
            try
            {
                //Thread.Sleep(2000);
                Driver1.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                IWebElement ElementMenuBy = Driver1.FindElement(By.XPath(CatProvElements.typeSearchS + "[contains(text(), '" + TipoBusqueda + "')]"));
                if (CatProvElements.typesSearch.Contains(ElementMenuBy)) {
                    var indexElement = CatProvElements.typesSearch.IndexOf(ElementMenuBy);
                    Reporter.LogPassingTestStepForBugLogger($"Realizando busqueda de proveedor por '{ElementMenuBy.Text}'.");
                    CatProvElements.typesSearch[indexElement].Click();

                }
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }

        }
        public void ClickAddProvedor() {
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{CatProvElements.btnAddProvider.Text}'");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@id='addProviderBtn']")));
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='toast-container']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("window.scrollTo(0, 0)");
            WaitSpinner();
            CatProvElements.btnAddProvider.Click();
            Thread.Sleep(2000);
        }


        public IWebElement ChoiceProvedorCentral(string TipoBusqueda) {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='providerListContainer']")));
            IList<IWebElement> provedoresCentrales = new List<IWebElement>();
            for (int i = 0; i < CatProvElements.ListProvedoresCentrales.Count; i++)
            {
                int index = (TipoBusqueda == "RFC") ? 2 : (TipoBusqueda == "Clave del proveedor") ? 3 : (TipoBusqueda == "Nombre del proveedor") ? 0 : 0;
                var textTipey = (TipoBusqueda == "RFC") ? "span" : "b";
                var t2t = CatProvElements.ListProvedores[i].FindElement(By.XPath("./div[@class='providers-card']/div/div/div[" + (index + 1) + "]/" + textTipey));
                string RFC = t2t.Text;
                if (RFC != ""&& RFC!= "XAXX010101000"&& RFC!= "XAXX010101001")
                {
                    provedoresCentrales.Add(CatProvElements.ListProvedoresCentrales[i]);
                }
            }
            int randomIndex;
            try
            {
                int min = 0;
                var max = provedoresCentrales.Count;
                Random rnd = new Random();
                randomIndex = rnd.Next(max);

            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }

            return BuscaProveedorCentral(provedoresCentrales[randomIndex]);

        }
        private IWebElement BuscaProveedorCentral(IWebElement proveedorCentral) {
            var NombreProveedorCentral = proveedorCentral.Text.Replace("\n", "").Split('\r').ToList<string>();
            for (int i = 0; i < CatProvElements.ListProvedores.Count; i++)
            {
                var nombreProvedor = CatProvElements.ListProvedores[i].Text.Replace("\n", "").Split('\r').ToList<string>();
                if (NombreProveedorCentral[0] == nombreProvedor[0])
                {
                    return CatProvElements.ListProvedores[i];
                }
            }
            return proveedorCentral;
        }
        public IWebElement BuscaProveedor(string nameProveedor)
        {
            Thread.Sleep(1000);
            IWebElement proveedorFound = null;
            for (int i = 0; i < CatProvElements.ListProvedores.Count; i++)
            {
                var nombreProvedor = CatProvElements.ListProvedores[i].Text.Replace("\n", "").Split('\r').ToList<string>();
                if (nameProveedor == nombreProvedor[1])
                {
                    proveedorFound = CatProvElements.ListProvedores[i];
                    return proveedorFound;
                }
            }
            return proveedorFound;
        }
        public IWebElement ChoiceProvedorLocal() {
            IList<IWebElement> listaProveLocales=new List<IWebElement>();
            for (int i = 0; i < CatProvElements.ListProvedores.Count; i++)
            {
               IWebElement proveedor= CatProvElements.ListProvedores[i].FindElement(By.XPath("./div"));
               string idProv= proveedor.GetAttribute("id");
                if (idProv.Contains("local"))
                {
                    listaProveLocales.Add(CatProvElements.ListProvedores[i]);
                }
            }
            Random r = new Random();
            var num = r.Next(listaProveLocales.Count);
            return listaProveLocales[num];

        }
        public static string ObtenerDatoBuscar(List<string> provedorBuscado, string TipoBusqueda) {
            //Filtrar los provedores que sean buscados que tengan rfc o clave ya sea el caso 
            int Type = 6;
            string NameProvedor = "";
            switch (TipoBusqueda)
            {
                case ("RFC"):
                    Type = 2;
                    break;
                case ("Clave del proveedor"):
                    Type = 3;
                    break;
                case ("Nombre del proveedor"):
                    Type = 0;
                    NameProvedor = provedorBuscado[Type].Split("-")[0];
                    break;
            }
            if (Type == 0)
            {
                return NameProvedor;
            }
            else
            {
                return Type == 6 ? "" : provedorBuscado[Type];

            }
        }
        public bool ProvedorIsFound(List<string> provedorBuscado, string TipoBusqueda) {
            bool found = false;
            int Type = 6;
            switch (TipoBusqueda)
            {
                case ("RFC"):
                    Type = 2;
                    break;
                case ("Clave del proveedor"):
                    Type = 3;
                    break;
                case ("Nombre del proveedor"):
                    Type = 0;
                    Thread.Sleep(3000);
                    break;
            }
            if (Type != 6)
            {
                foreach (IWebElement provedorEncontrado in CatProvElements.ListProvedoresSearch)
                {
                    var text = provedorEncontrado.Text.Replace("\n", "").Split('\r').ToList<string>();
                    if (Type != 0)
                    {
                        if (provedorBuscado[Type] == text[Type])
                        {
                            Reporter.LogTestStepForBugLogger(Status.Info, $"Provedor buscado por '{TipoBusqueda}', encontrado con '{text[Type]}' '");
                            found = true;

                            break;
                        }

                    }
                    else
                    {
                        found = true;
                        var prov = provedorBuscado[Type].Split('-')[0];

                        if (prov != text[Type].Split('-')[0])
                        {
                            Reporter.LogTestStepForBugLogger(Status.Fail, $"Provedor buscado por '{TipoBusqueda}', no fue encontrado con '{prov}' 'Fallo en: {provedorBuscado[Type]}");
                            found = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, $"Busqueda por '{TipoBusqueda}' no esta soportada");

            }
            return found;
        }

        public IWebElement ChoiceProvedorCentralSearch()
        {
            Thread.Sleep(5000);
            int randomIndex;
            try
            {
                int min = 1;
                var max = CatProvElements.ListProvedoresSearch.Count;
                Random rnd = new();
                randomIndex = rnd.Next(min, max);
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }

            return CatProvElements.ListProvedoresSearch[randomIndex];
        }
        public void ClickAddProveedorCentralSearch(IWebElement proveedorCentralSearch) {
            var indexProv = CatProvElements.ListProvedoresSearch.IndexOf(proveedorCentralSearch);
            Thread.Sleep(1000);
            IWebElement btnAddProveedor = Driver1.FindElement(By.CssSelector("button[data-provider-id='" + indexProv + "']"));
            //IWebElement heal = Driver1.FindElement(By.XPath("//*[@id='launcher']"));
            Thread.Sleep(2000);
           // Driver1.ExecuteJavaScript("arguments[0].style.display = 'none';", heal);
            if (btnAddProveedor.Enabled)
            {
                //Point a = btnAddProveedor.Location;
                waitElementToClick(btnAddProveedor);
                //btnAddProveedor.Click();
            }
            else
            {
                IWebElement btnAddProveedor2 = Driver1.FindElement(By.CssSelector("button[data-provider-id='" + indexProv+1 + "']"));
                btnAddProveedor2.Click();
            }
            Thread.Sleep(2000);
        }
        public void ClickToButtonsModalProveedor(string option) {
            var index = (option == "Aceptar" ? 2 : option == "Cancelar"?0:1);
            Reporter.LogPassingTestStepForBugLogger($"Click al botón '{CatProvElements.botonesAddProvedorModal[index].Text}'");
            CatProvElements.botonesAddProvedorModal[index].Click();
            Thread.Sleep(1000);
        }


        public void PutModalRazonSocial(string razonSocial) {
            Reporter.LogPassingTestStepForBugLogger($"Agregando razon Social'{razonSocial}'");
            if (razonSocial.Length <= 60)
            {
                CatProvElements.inputModalRazonSocial.SendKeys(razonSocial);
            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, "La razon social tiene un tamaño mayor a 60 caracteres.");
            }
        }
        public void PutModalNameProvider(string nameProvider) {
            Reporter.LogPassingTestStepForBugLogger($"Agregando nombre del proveedor'{nameProvider}'");
            if (nameProvider.Length <= 60)
            {
                ActionsSelenium actionsSelenium = new ActionsSelenium(Driver1);
                CatProvElements.inputModalNombreProv.Clear();
                actionsSelenium.MoveToElement(CatProvElements.inputModalNombreProv)
                    .SendKeys(CatProvElements.inputModalNombreProv, nameProvider).Build().Perform();
            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, "El nombre del provedor tiene un tamaño mayor a 60 caracteres.");
            }
        }

        public void PutModalRFC(string RFC) {
            Reporter.LogPassingTestStepForBugLogger($"Agregando RFC: '{RFC}'");
            if (RFC.Length >= 13)
            {
                CatProvElements.inputModalRFC.SendKeys(RFC);
            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, "El RFC tiene un tamaño mayor a 13 caracteres.");
            }
        }
        //Moral o Física
        public void PutModalTipoPer(string TipoPer) {
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando tl tipo de persona: '{TipoPer}'");
            for (int i = 0; i < CatProvElements.ModalTypesPerson.Count; i++)
            {
                var typeP = CatProvElements.ModalTypesPerson[i].GetAttribute("data-text");
                if (typeP == TipoPer)
                {
                    IWebElement label = Driver1.FindElement(By.XPath("//div[@id='PersonTypeContainer']/div/label[text()='" + typeP + "']"));
                    label.Click();
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
                }
            }
            Driver1.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }
        //Proveedor o Acreedor
        public void PutModalTipoProv(string TipoProv)
        {
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el tipo de persona: '{TipoProv}'");
            for (int i = 0; i < CatProvElements.ModalTypeProvider.Count; i++)
            {
                var typeP = CatProvElements.ModalTypeProvider[i].GetAttribute("data-text");
                if (typeP == TipoProv)
                {
                    

                    IWebElement label = Driver1.FindElement(By.XPath("//div[@id='ProviderTypesContainer']/div/label[text()='" + typeP + "']"));
                    waitElementToClick(label);
                   
                    //label.Click();
                }
            }
        }
        //605 - Sueldos y Salarios e Ingresos Asimilados a Salarios
        public void PutModalRegimenFiscal(string RegimenF) {
            Thread.Sleep(1000);


            SelectElement ModalSelectRegiFis = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='FiscalRegime']")));
            ModalSelectRegiFis.SelectByText(RegimenF);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el regimen fiscal '{RegimenF}'");
        }
        //G01 - Adquisición de mercancias
        public void PutModalCFDI(string CFDI)
        {
            SelectElement ModalSelectCFDI = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='CFDIUse']")));
            ModalSelectCFDI.SelectByText(CFDI);
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el CFDI '{CFDI}'");
        }
        //
        public void PutModalGrupo(string tipoGrupo, [Optional] bool isEdit)
        {
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando el grupo '{tipoGrupo}'");
            if (isEdit)
            {
                for (int i = 0; i < CatProvElements.ModalTypesGrupo.Count; i++)
                {
                    CatProvElements.ModalTypesGrupo[i].Click();
                }
            }
            for (int i = 0; i < CatProvElements.ModalTypesGrupo.Count; i++)
            {
                if (tipoGrupo == "Todos")
                {
                    CatProvElements.ModalTypesGrupo[i].Click();
                }
                else
                {
                    var tipoG = CatProvElements.ModalTypesGrupo[i].GetAttribute("data-text");
                   
                    if (tipoG == tipoGrupo)
                    {
                        CatProvElements.ModalTypesGrupo[i].Click();

                        break;
                    }

                }
            }

        }
        public void ClickModalNext()
        {
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{CatProvElements.ModalBtnNext.Text}'");
            CatProvElements.ModalBtnNext.Click();
            Thread.Sleep(2000);
        }
        public void ClickModalAddProviderOwn()
        {
            Reporter.LogPassingTestStepForBugLogger($"click al botón '{CatProvElements.modalBtnAddProviderOwn.Text}'");
            CatProvElements.modalBtnAddProviderOwn.Click();
            Thread.Sleep(2000);
        }
        //Informacion General
        public void FillAddProviderLocalStep1([Optional] string RazonSocial, string NombreProv, [Optional] string RFC, string Tpersona, string Tprove, [Optional] string RF, [Optional] string CFDI, string Grupo, [Optional] bool isEdit) {
            if (RazonSocial != null)
                PutModalRazonSocial(RazonSocial);
            if (isEdit==false)
                PutModalNameProvider(NombreProv);
            if (RFC != null)
                PutModalRFC(RFC);
            PutModalTipoPer(Tpersona);
            PutModalTipoProv(Tprove);
            if (RF != null)
                PutModalRegimenFiscal(RF);
            if (CFDI != null)
                PutModalCFDI(CFDI);
            PutModalGrupo(Grupo,isEdit:isEdit);

        }

        //Contacto
        public bool IsModalInfoGeneral() {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='PearlGeneralInformation']")));
            return CatProvElements.modalPerlInfoGen.GetDomAttribute("aria-expanded").Equals("true");
        }
        public bool IsModalContacto()
        {
            Thread.Sleep(800);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='PearlContactInformation']")));
            return CatProvElements.modalPerlContactoInfo.GetDomAttribute("aria-expanded").Equals("true");
        }
        public bool IsModalCredito()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='PearlCreditInformation']")));
            return CatProvElements.modalPerlInfoCredito.GetDomAttribute("aria-expanded").Equals("true");
        }
        public bool IsModalConfirmacion()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='PearlConfirmInformation']")));
            return CatProvElements.modalPerlConfirmacion.GetDomAttribute("aria-expanded").Equals("true");
        }
        public void PutModalCodePostal(string CP) {
            CatProvElements.modalPostalCode.Clear();
            CatProvElements.modalPostalCode.SendKeys(CP);
            CatProvElements.modalCalle.Click();
            Thread.Sleep(3000);
            Reporter.LogPassingTestStepForBugLogger($"Agregando codigo postal '{CP}'");
        }
        public void PutModalCalle(string calle)
        {

            CatProvElements.modalCalle.Clear();
            if (calle.Length <= 120)
            {
                CatProvElements.modalCalle.SendKeys(calle);
                Reporter.LogPassingTestStepForBugLogger($"Agregando calle '{calle}'");
            }
            else {
                Reporter.LogTestStepForBugLogger(Status.Fail, $"La calle '{calle}' tiene mas de 120 caracteres");
            }
        }
        public void PutModalNumExt(string numExt)
        {
            if (numExt.Length <= 6)
            {
                CatProvElements.modalNumExterno.SendKeys(numExt);
                Reporter.LogPassingTestStepForBugLogger($"Agregando numero exterior '{numExt}'");

            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, $"El numero exterior '{numExt}' tiene mas de 6 caracteres");
            }
        }
        public void PutModalNumInt(string numInt)
        {
            if (numInt.Length <= 6)
            {
                CatProvElements.modalNumInterno.SendKeys(numInt);
                Reporter.LogPassingTestStepForBugLogger($"Agregando numero exterior '{numInt}'");
            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, $"El numero interior '{numInt}' tiene mas de 6 caracteres");
            }
        }
        public void PutModalColonia(string Colonia)
        {
            if (Colonia.Length <= 60)
            {
                CatProvElements.modalColonia.Clear();
                CatProvElements.modalColonia.SendKeys(Colonia);
                Reporter.LogPassingTestStepForBugLogger($"Agregando colonia '{Colonia}'");
            }
            else
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, $"La Colonia '{Colonia}' tiene mas de 60 caracteres");
            }
        }
        public void PutModalMunicipio(string Municipio)
        {
            SelectElement ModalSelectMuni = new SelectElement(Driver1.FindElement(By.XPath("//*[@id='CitySelect']")));
            ModalSelectMuni.SelectByText(Municipio.ToUpper());
            Reporter.LogPassingTestStepForBugLogger($"Seleccionando colonia '{Municipio}'");
        }
        public void PutModalContacto1(string Contacto1)
        {
            CatProvElements.modalContacto1.Clear();
            CatProvElements.modalContacto1.SendKeys(Contacto1);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de contacto 1 '{Contacto1}'");
        }
        public void PutModalContacto2(string Contacto2)
        {
            CatProvElements.modalContacto2.Clear();
            CatProvElements.modalContacto2.SendKeys(Contacto2);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de contacto 2 '{Contacto2}'");
        }
        public void PutModalCorreo1(string Correo1)
        {
            CatProvElements.modalCorreo1.Clear();
            CatProvElements.modalCorreo1.SendKeys(Correo1);
            Reporter.LogPassingTestStepForBugLogger($"Agregando Correo de contacto 1 '{Correo1}'");
        }
        public void PutModalCorreo2(string Correo2)
        {
            CatProvElements.modalCorreo2.Clear();
            CatProvElements.modalCorreo2.SendKeys(Correo2);
            Reporter.LogPassingTestStepForBugLogger($"Agregando Correo de contacto 2 '{Correo2}'");
        }
        public void PutModalTel1(string Tel1)
        {
            CatProvElements.modalPhone1.SendKeys(Tel1);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de telefono 1 '{Tel1}'");
        }
        public void PutModalTel2(string Tel2)
        {
            CatProvElements.modalPhone2.SendKeys(Tel2);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de telefono 2 '{Tel2}'");
        }
        public void PutModalTelContac2_1(string TelContac2)
        {
            CatProvElements.modalPhoneContac2_1.Clear();
            CatProvElements.modalPhoneContac2_1.SendKeys(TelContac2);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de telefono 1 '{TelContac2}'");
        }
        public void PutModalTelContac2_2(string TelContac2_2)
        {
            CatProvElements.modalPhoneContac2_2.SendKeys(TelContac2_2);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de telefono 2 '{TelContac2_2}'");
        }
        public void ClickToAddContact2() {
            var a = CatProvElements.modalCheckBoxAddContac2.GetAttribute("checked");
            string tag = CatProvElements.modalCheckBoxAddContac2.TagName;
            if (CatProvElements.modalCheckBoxAddContac2.GetAttribute("checked")==null)
            {
                CatProvElements.modalCheckBoxAddContac2.Click();
            }
            Reporter.LogPassingTestStepForBugLogger($"Click al checkbox para agregar datos de 2 contacto");
        }
        public void PutModalWeb(string Web)
        {
            CatProvElements.modalWebUrl.SendKeys(Web);
            Reporter.LogPassingTestStepForBugLogger($"Agregando URL de sitio web '{Web}'");
        }
        public void PutDays(string days, [Optional]bool isEdit)
        {
            string[] daysArray = days.Split(',');

            if (!isEdit)
            {
                for (int i = 0; i < CatProvElements.ModalDias.Count; i++)
                {
                    if (daysArray.Contains(CatProvElements.ModalDias[i].GetAttribute("data-description")))
                    {
                        var dayUpper = CatProvElements.ModalDias[i].GetAttribute("data-description").ToUpper();
                        if (dayUpper == "MIÉRCOLES")
                            dayUpper = "MIERCOLES";
                        IWebElement label = Driver1.FindElement(By.XPath("//div[@id='ScheduleSectionContainer']/div/label[@for='" + dayUpper + "']"));
                        label.Click();
                        Reporter.LogPassingTestStepForBugLogger($"Agregando el dia '{label.GetAttribute("for")}'");

                    }

                }
            }else{
                IList<IWebElement> daysArray2 = Driver1.FindElements(By.XPath("//*[@id='SelectedScheduleContainer']/div/div[1]/span"));
                List<string> daysWithoutSpace= new List<string>();
                for (int k = 0; k < daysArray2.Count; k++)
                {
                    daysWithoutSpace.Add(daysArray2[k].Text.ToString().Trim());
                }
                for (int j = 0; j < daysArray.Length; j++)
                {
                    if (!daysWithoutSpace.Contains(daysArray[j]))
                    {
                        var dayUpper = CatProvElements.ModalDias[j].GetAttribute("data-description").ToUpper();
                        if (dayUpper == "MIÉRCOLES")
                            dayUpper = "MIERCOLES";
                        if (dayUpper == "SÁBADO")
                            dayUpper = "SABADO";
                        IWebElement label = Driver1.FindElement(By.XPath("//div[@id='ScheduleSectionContainer']/div/label[@for='" + dayUpper + "']"));
                        label.Click();
                        Reporter.LogPassingTestStepForBugLogger($"Agregando el dia '{label.GetAttribute("for")}'");
                    }
                }
            }
        }

        public void PutHoraToDay(string day, string start, string end, [Optional] bool copiar) {
            var data_id = 0;

            switch (day.ToUpper())
            {
                case "LUNES":
                    data_id = 1;
                    break;
                case "MARTES":
                    data_id = 2;
                    break;
                case "MIERCOLES":
                    data_id = 3;
                    break;
                case "JUEVES":
                    data_id = 4;
                    break;
                case "VIERNES":
                    data_id = 5;
                    break;
                case "SABADO":
                    data_id = 6;
                    break;
                case "DOMINGO":
                    data_id = 7;
                    break;
            }

            IList<IWebElement> ListDias = Driver1.FindElements(By.XPath("//div[@id='SelectedScheduleContainer']/div"));
            for (int i = 0; i < ListDias.Count; i++)
            {
                if (Int32.Parse(ListDias[i].GetAttribute("data-id")) == data_id) {
                    IList<IWebElement> StartEnd = ListDias[i].FindElements(By.XPath("./div/input"));
                    StartEnd[0].Clear();
                    StartEnd[0].SendKeys(start);
                    StartEnd[1].Clear();
                    StartEnd[1].SendKeys(end);
                    StartEnd[1].Click();
                    Reporter.LogPassingTestStepForBugLogger($"Agregando Hora al dia '{day}', inicio '{start}' a '{end}'");
                    if (copiar != null && copiar) {
                        IWebElement CopiarH = ListDias[i].FindElement(By.XPath("./a"));
                        CopiarH.Click();
                    }
                    break;
                }
            }
        }

        public void FillAddProviderLocalStep2(string CP, string Calle, string NumExt, [Optional] string NumInter,
            string Colonia, string Municipio, string NombreContacto1, string Correo1, string Telefono1, [Optional] string Telefono2,
            [Optional] string SitioWeb, string Dias,
            [Optional] bool isContacto2, [Optional] string NombreContacto2, [Optional] string Correo2, [Optional] string Telefono2_1, [Optional] string Telefono2_2, [Optional]bool isEdit)
        {
            PutModalCodePostal(CP);
            PutModalCalle(Calle);
            PutModalNumExt(NumExt);
            if (NumInter != null)
                PutModalNumInt(NumInter);
            PutModalColonia(Colonia);
            PutModalMunicipio(Municipio);
            PutModalContacto1(NombreContacto1);
            PutModalCorreo1(Correo1);
            PutModalTel1(Telefono1);
            if (Telefono2 != null)
                PutModalTel2(Telefono2);
            if (isContacto2) {
                ClickToAddContact2();

                PutModalContacto2(NombreContacto2);
                if (Correo2 != null)
                    PutModalCorreo2(Correo2);
                if (Telefono2_1 != null)
                    PutModalTelContac2_1(Telefono2_1);
                if (Telefono2_2 != null)
                    PutModalTelContac2_2(Telefono2_2);
            }

            if (SitioWeb != null)
                PutModalWeb(SitioWeb);
            PutDays(Dias, isEdit);
        }
        public void LineaCredito(string Option)
        {
            IWebElement Credito;
            Thread.Sleep(1000);
            if (Option == "Si")
            {//*[@id='creditLineContainer']/button[@creditline='S']
                Credito = Driver1.FindElement(By.XPath("//*[@id='CreditLineContainer']/div[1]/label"));
            }
            else
            {
                Credito = Driver1.FindElement(By.XPath("//*[@id='CreditLineContainer']/div[2]/label"));
            }
            Credito.Click();
            Reporter.LogPassingTestStepForBugLogger($"Agregando linea de credito con la opcion'{Option}'");
        }

        public void LineaCreditoEditar(string Option) {
            IWebElement Credito;
            Thread.Sleep(1000);
            if (Option == "Si")
            {//*[@id='creditLineContainer']/button[@creditline='S']
                Credito = Driver1.FindElement(By.XPath("//*[@id='creditLineContainer']/button[@creditline='" + Option[0] + "']"));
            }
            else
            {
                Credito = Driver1.FindElement(By.XPath("//*[@id='creditLineContainer']/button[@creditline='" + Option[0] + "']"));
            }
            Credito.Click();
            Reporter.LogPassingTestStepForBugLogger($"Agregando linea de credito con la opcion'{Option}'");
        }
        public string LineaCreditoSeleccionada() {
            IList<IWebElement> btnCredito = Driver1.FindElements(By.XPath("//*[@id='creditLineContainer']/button"));
            string opselect = "";
            for (int i = 0; i < btnCredito.Count; i++)
            {
                if (btnCredito[i].GetAttribute("class").Contains("option-actived"))
                    opselect = btnCredito[i].Text;
            }
            return opselect;
        }
        public void ModificarLineaCredito(string nuevoMonto,string nuevoDias, string nuevoTentrega) {
            IWebElement MontoC = Driver1.FindElement(By.XPath("//*[@id='creditAmountCentralProvider']"));
            IWebElement DiasC = Driver1.FindElement(By.XPath("//*[@id='creditDaysCentralProvider']"));
            IWebElement Tentrega = Driver1.FindElement(By.XPath("//*[@id='deliveryPromiseCentralProvider']"));
            Reporter.LogPassingTestStepForBugLogger($"Agregando monto de linea de credito : '{nuevoMonto}'");
            Reporter.LogPassingTestStepForBugLogger($"Agregando dias de credito : '{DiasC}'");
            Reporter.LogPassingTestStepForBugLogger($"Agregando tiempo promesa de entrega : '{Tentrega}'");
            MontoC.SendKeys(nuevoMonto);
            DiasC.SendKeys(nuevoDias);
            Tentrega.SendKeys(nuevoTentrega);
        }
        public bool MontoYDiasCreditoBlock(){
            IWebElement MontoC= Driver1.FindElement(By.XPath("//*[@id='creditAmountCentralProvider']"));
            IWebElement DiasC= Driver1.FindElement(By.XPath("//*[@id='creditDaysCentralProvider']"));

            Reporter.LogPassingTestStepForBugLogger($"El campo 'Monto de linea de credito' esta en estado activo como: '{MontoC.Enabled}'");
            Reporter.LogPassingTestStepForBugLogger($"El campo 'Dias de credito'  esta en estado activo como: '{MontoC.Enabled}'");
            return (MontoC.Enabled&&DiasC.Enabled);
        }
        public void ClickBtnGuardarLineC() {
            IWebElement btnGuardar = Driver1.FindElement(By.XPath("//*[@id='saveCreditLine']"));
            Reporter.LogPassingTestStepForBugLogger($"Dando click al boton: '{btnGuardar.Text}'");
            btnGuardar.Click();
        }
        public void MontoLineaCredito(string montoC) {
            CatProvElements.modalCredito.Clear();
            CatProvElements.modalCredito.SendKeys(montoC);
            Reporter.LogPassingTestStepForBugLogger($"Agregando monto de credito '{montoC}'");
        }
        public void DiasLineaCredito(string DiasC)
        {
            CatProvElements.modalDiasCredito.Clear();  
            CatProvElements.modalDiasCredito.SendKeys(DiasC);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de dias para el credito '{DiasC}'");
        }
        public void TiempoEntregaLineaCredito(string tiempoEntrega)
        {
            CatProvElements.modalTiempoEntregaCredito.Clear();
            CatProvElements.modalTiempoEntregaCredito.SendKeys(tiempoEntrega);
            Reporter.LogPassingTestStepForBugLogger($"Agregando numero de dias de entrega para el credito '{tiempoEntrega}'");
        }
        public void FillAddProviderLocalStep3(string isCredito,[Optional] string montoC, [Optional] string DiasC, string tiempoEntrega) {
            LineaCredito(isCredito);
            if (isCredito=="Si")
            {
                MontoLineaCredito(montoC);
                DiasLineaCredito(DiasC);
            }
             TiempoEntregaLineaCredito(tiempoEntrega);
        
        }
        public void SelecAccionProveedor(IWebElement Provedor, string accion,[Optional] string idProvider, [Optional] bool isCentral) {
            //./div/div/div/div[contains(@class,'providers-options')/span
            IList<IWebElement> accionesProv;
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver1);
            Thread.Sleep(2000);
            if (accion == "Activar" || accion == "Desactivar")
            {
                string xpath = isCentral==true?"//*[@id='central-" + idProvider + "']/div/div[1]/div[6]/div/span": "//*[@id='local-" + idProvider + "']/div/div[1]/div[6]/div/span";
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath(xpath)));
                IWebElement switchActiion = Driver1.FindElement(By.XPath(xpath));
                actions.MoveToElement(switchActiion);
                switchActiion.Click();
                Reporter.LogTestStepForBugLogger(Status.Info, $"Seleccionando la opcion '{accion}' del proveedor con el id {idProvider}");
            }
            else {
                Thread.Sleep(1000);
                 accionesProv = Provedor.FindElements(By.XPath("./div/div/div/div[contains(@class,'providers-options')]/span"));
                for (int i = 0; i < accionesProv.Count; i++)
                {
                    var accionProv=accionesProv[i].GetAttribute("data-original-title");
                    if (accionProv == accion) {
                        IWebElement iconAccion= accionesProv[i].FindElement(By.XPath("./i"));
                        actions.ScrollToElement(iconAccion);
                        string acc = iconAccion.GetAttribute("data-provider-id");
                        iconAccion.Click();
                        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='toast-container']")));
                        Reporter.LogTestStepForBugLogger(Status.Info,$"Seleccionando la opcion '{accion}' del proveedor con el id {acc}");
                        break;
                    }
                }
            }
        }
        public bool IsModalVerDetallesProvedor() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='ProviderDetail']/div/div/div/h4")));
            IWebElement title = Driver1.FindElement(By.XPath("//div[@id='ProviderDetail']/div/div/div/h4"));
            Reporter.LogPassingTestStepForBugLogger($"Verificando estar en el modal de detalles {title.Text.Equals("Detalles del proveedor")}");
            return title.Text.Equals("Detalles del proveedor");
        }
        public void ClickCloseModalDetalles() {
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='ProviderDetail']/div/div/div[@class='modal-body']/div/button")));
            IWebElement btnClose = Driver1.FindElement(By.XPath("//*[@id='ProviderDetail']/div/div/div[@class='modal-body']/div/button"));
            Reporter.LogTestStepForBugLogger(Status.Info,"Cerrando modal de detalles");
            btnClose.Click();   
        }
        public bool IsModalEditarProveedorCentral() {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SpinnerLoader']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='editCentralProviderModal']")));
            IWebElement title = Driver1.FindElement(By.XPath("//*[@id='editCentralProviderModal']/div/div/div[1]/h4"));
            Reporter.LogPassingTestStepForBugLogger($"Verificando estar en el modal de editar {title.Text.Equals("Editar linea de crédito")}");
            return title.Text.Equals("Editar linea de crédito");
        }
        public bool IsModalEditarProveedorLocal() {
            Thread.Sleep(1000);
            IWebElement title = Driver1.FindElement(By.XPath("//*[@id='LocalProviderModal']/div/div/div/h4"));
            Reporter.LogPassingTestStepForBugLogger($"Verificando estar en el modal de editar {title.Text}");
            return title.Text.Equals("Editar proveedor");
        }
        public void ClickCancelarModalEditar() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='editCentralProviderModal']//button[@aria-label='Close']")));
            IWebElement btnClose = Driver1.FindElement(By.XPath("//*[@id='editCentralProviderModal']//button[@aria-label='Close']"));
            Reporter.LogTestStepForBugLogger(Status.Info, "Cerrando modal de editar");
            Thread.Sleep(500);
            btnClose.Click();
        }
        public static int NumAccionsByProvider(IWebElement Provedor) {
            IList<IWebElement> accionesProv = Provedor.FindElements(By.XPath("./div/div/div/div[contains(@class,'providers-options')]/*"));
            return accionesProv.Count;
        }
        public void ClickBtnModalCancel() {
            CatProvElements.ModalBtnCancel.Click();
        }

        public void ClickReturnListado() {
           CatProvElements.btnReturnList.Click();
        }
        public void AgregarProveedorCentralAzar() {
            Menu menu = new Menu(Driver);
            try
            {
                menu.clikElementMenu("Catálogo de proveedores");
                Assert.That(IsCatalogoProvPage(), Is.True);
                ClickAddProvedor();
                SelectTypeProvedorItalika();
                Reporter.LogTestStepForBugLogger(Status.Info, $"Buscando por Nombre CESIT");
                BuscarProvedorBy("Nombre del proveedor");
                searchInput.SendKeys("CESIT");
                CatProvElements.btnSearch.Click();
                //Escoger un provedor para agregar a mis provedores
                IWebElement ProvedorSearch = ChoiceProvedorCentralSearch();
                ClickAddProveedorCentralSearch(ProvedorSearch);
                ClickToButtonsModalProveedor("Cancelar");
                ClickAddProveedorCentralSearch(ProvedorSearch);
                ClickToButtonsModalProveedor("Aceptar");
                ClickReturnListado();
            }
            catch (NoSuchElementException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message);
                throw;
            }

        }
        public bool FillsNoEditablesProvider() {
           
            return (!CatProvElements.inputModalRazonSocial.Enabled && !CatProvElements.inputModalNombreProv.Enabled && !CatProvElements.inputModalRFC.Enabled);
        }
        public void AgregarProveedorLocal() {
            Menu menu = new Menu(Driver1);
            menu.clikElementMenu("Catálogo de proveedores");
            Assert.That(IsCatalogoProvPage(), Is.True);
            ClickAddProvedor();
            SelectTypeProvedorPropio();
            Assert.That(IsModalInfoGeneral(), Is.True);
            FillAddProviderLocalStep1(RazonSocial: "PROVEEDOR 999 SA DE CV", NombreProv: "Proveedor 999", Tpersona: "Física", Tprove: "Proveedor", Grupo: "Todos");
            ClickModalNext();
            Assert.That(IsModalContacto(), Is.True);
            FillAddProviderLocalStep2(CP: "78000", Calle: "Reforma", NumExt: "128", Colonia: "Centro", Municipio: "SAN LUIS POTOSÍ",
                NombreContacto1: "LUIS ORTEGA", Correo1: "dev.radarcontroltotal@gmail.com", Telefono1: "4444444444", Dias: "Lunes,Martes,Miércoles,Jueves,Viernes", isContacto2: true, NombreContacto2: "Jacob Loredo", Correo2: "Jacob.loredo@hotmail.com", Telefono2_1: "4422552211", Telefono2_2: "1234567890");
            PutHoraToDay("Lunes", "08:00:00", "18:00:00", true);

            ClickModalNext();
            Assert.That(IsModalCredito(), Is.True);
            FillAddProviderLocalStep3(isCredito: "No", tiempoEntrega: "12");
            ClickModalNext();
            Assert.That(IsModalConfirmacion(), Is.True);
            ClickModalAddProviderOwn();
            Assert.That(IsToastSuccess(text: "Se guardó el proveedor correctamente."), Is.True);
        }
    }
}

