using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.Modules.CatalogoClientes.PageElements
{
    public class CatClientesElements
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='CustomerEdit']")]
        public IWebElement BtnEditCustumerModal { get; set; }
        [FindsBy(How = How.XPath, Using = "//h1[@class='page-title']")]
        public IWebElement h1Titulo { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='addCustomerBtn']")]
        public IWebElement BtnAddCustumer { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='CustomerSave']")]
        public IWebElement BtnAddCustumerModal { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class ='panel-body']/button")]

        public IWebElement BtnBackBusqueda { get; set; }

        #region Modal de alta clienten
            [FindsBy(How = How.XPath, Using = "//*[@id='Name']")]
            public IWebElement modalAltaInputNombre { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='Phone']")]
            public IWebElement modalAltaInputTel { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='Mail']")]
            public IWebElement modalAltaInputCorreo { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='CP']")]
            public IWebElement modalAltaInputCP { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContact']")]
            public IWebElement modalAltaCheckContacto2 { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='DataInvoicing']")]
            public IWebElement modalAltaCheckFacturacion { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='Credit']")]
            public IWebElement modalAltaCheckCredito { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='Descount']")]
            public IWebElement modalAltaCheckCDescuento { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactName']")]
            public IWebElement modalAltaInputContacto2Nombre { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactPhone']")]
            public IWebElement modalAltaInputContacto2Tel { get; set; }
            [FindsBy(How = How.XPath, Using = "//*[@id='SecondContactMail']")]
            public IWebElement modalAltaInputContacto2Correo { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingBusinessName']")]
            public IWebElement modalAltaInputRazonSocial { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingRFC']")]
            public IWebElement modalAltaInputRFC { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingCP']")]
            public IWebElement modalAltaInputCPDom { get; set; }


            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingStreet']")]
            public IWebElement modalAltaInputCalleDom { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingOutdoorNumber']")]
            public IWebElement modalAltaInputNumExt { get; set; }


            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingInteriorNumber']")]
            public IWebElement modalAltaInputNumInt { get; set; }



            [FindsBy(How = How.XPath, Using = "//*[@id='InvoicingSuburb']")]
            public IWebElement modalAltaInputColonia { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='CreditLineAmount']")]
            public IWebElement modalAltaInputMontoCredito { get; set; }

            [FindsBy(How = How.XPath, Using = "//*[@id='CreditDays']")]
            public IWebElement modalAltaInputDiasCredito { get; set; }


            [FindsBy(How = How.XPath, Using = "//*[@id='DescountCreditLineAmount']")]
            public IWebElement modalAltaInputDescuento { get; set; }

        #endregion
    }
}
