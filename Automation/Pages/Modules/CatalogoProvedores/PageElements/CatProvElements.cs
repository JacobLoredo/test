
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

namespace Automation.Pages.Modules.CatalogoProvedores.PageElements
{
    public class CatProvElements
    {

        [FindsBy(How = How.XPath, Using = "//h1[contains(text(),'Catálogo de proveedores')]")]
        public IWebElement h1Titulo { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@id='addProviderBtn']")]
        public IWebElement btnAddProvider { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='modalProviderContainer']/div[@id='ProviderType']")]
        public IWebElement modalProviderType { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='LocalProviderOption']//preceding::div[contains(@class,'provider-type-card')]")]
        public IWebElement modalProviderCatalogoPItalika { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='LocalProviderOption']")]
        public IWebElement modalProviderAddPropio { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='panel']//div//button[1]")]
        public IWebElement btnReturnList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'searchOptions')]//button")]
        public IList<IWebElement> typesSearch { get; set; }
        //Elementos del modal Add provider propio
        #region

        [FindsBy(How = How.XPath, Using = "//div[@id='LocalProviderModal']")]
        public IWebElement modalLocalProvider { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='PearlContactInformation']")]
        public IWebElement modalPerlContactoInfo { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='PearlGeneralInformation']")]
        public IWebElement modalPerlInfoGen { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='PearlCreditInformation']")]
        public IWebElement modalPerlInfoCredito { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@id='PearlConfirmInformation']")]
        public IWebElement modalPerlConfirmacion { get; set; }
        //Informacion Adicional
        #region
        [FindsBy(How = How.XPath, Using = "//input[@id='CompanyName']")]
            public IWebElement inputModalRazonSocial { get; set; }
            [FindsBy(How = How.XPath, Using = "//input[@id='ProviderName']")]
            public IWebElement inputModalNombreProv { get; set; }
            [FindsBy(How = How.XPath, Using = "//input[@id='RFC']")]
            public IWebElement inputModalRFC { get; set; }

            [FindsBy(How = How.XPath, Using = "//div[@id='PersonTypeContainer']/div/input")]
            public IList<IWebElement> ModalTypesPerson { get; set; }

            [FindsBy(How = How.XPath, Using = "//div[@id='ProviderTypesContainer']/div/input")]
            public IList<IWebElement> ModalTypeProvider { get; set; }

            [FindsBy(How = How.XPath, Using = "//div[@id='ProviderGroupsContainer']/div/input")]
            public IList<IWebElement> ModalTypesGrupo { get; set; }

            [FindsBy(How = How.XPath, Using = "//a[@id='nextBtn']")]
            public IWebElement  ModalBtnNext { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='WizardCancelButton']")]
            public IWebElement ModalBtnCancel { get; set; }

        #endregion

        //Contacto y Horarios
        #region
            [FindsBy(How = How.XPath, Using = "//*[@id='PostalCode']")]
            public IWebElement modalPostalCode { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='StreetName']")]
            public IWebElement modalCalle { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='ExternalNumber']")]
            public IWebElement modalNumExterno { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='InternalNumber']")]
            public IWebElement modalNumInterno { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='SuburbName']")]
            public IWebElement modalColonia { get; set; }
            //Municipio es un select
            [FindsBy(How = How.XPath, Using = "//*[@id='FirstContactName']")]
            public IWebElement modalContacto1 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactName']")]
            public IWebElement modalContacto2 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='FirstContactMail']")]
            public IWebElement modalCorreo1 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactMail']")]
            public IWebElement modalCorreo2 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='FirstContactPhone1']")]
            public IWebElement modalPhone1 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='FirstContactPhone2']")]
            public IWebElement modalPhone2 { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactPhone1']")]
            public IWebElement modalPhoneContac2_1 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactPhone2']")]
            public IWebElement modalPhoneContac2_2 { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='WebsiteUrl']")]
            public IWebElement modalWebUrl { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactCheckbox']")]
            public IWebElement modalCheckBoxAddContac2 { get; set; }

            [FindsBy(How = How.XPath, Using = "//div[@id='ScheduleSectionContainer']/div/input")]
            public IList<IWebElement> ModalDias { get; set; }

            
        #endregion
        //Credito y entregas
        #region
            [FindsBy(How = How.XPath, Using = "//*[@id='CreditAmount']")]
            public IWebElement modalCredito { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='CreditDays']")]
            public IWebElement modalDiasCredito { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='AverageDeliveryTime']")]
            public IWebElement modalTiempoEntregaCredito { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='addUserBtn']")]
            public IWebElement modalBtnAddProviderOwn { get; set; }


        #endregion
        #endregion

        [FindsBy(How = How.XPath, Using = "//button[@id='searchBtn']")]
        public IWebElement btnSearch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='provider-key-search-container']/div/input[2]")]
        public IWebElement InputClaveProv { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='providerListContainer']/div[@class='row providers-container']/div")]
        public IList<IWebElement> ListProvedores { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='GlobalProvidersContainer']/div/div/div/div")]

        
        public IList<IWebElement> ListProvedoresSearch { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='providerListContainer']/div[@class='row providers-container']/div/div/div/div/div/span[@data-original-title='Proveedor central']/parent::div/b")]
        public IList<IWebElement> ListProvedoresCentrales { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'swal2-container')]//div[contains(@class,'swal2-actions')]//button")]
        public IList<IWebElement> botonesAddProvedorModal { get; set; }


        public string typeSearchS = "//div[contains(@class,'searchOptions')]//button";//')]
    }
}
