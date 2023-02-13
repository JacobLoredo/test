using Automation.Pages.CommonElements.Modales;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;

namespace Automation.Pages.Modules.PuntoVenta.Actions
{
    public class ActionsDetalle:DetallePage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }
        public ActionsDetalle(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        public void ClicGenerarOrdenVenta() {
            wait.Until(ExpectedConditions.ElementToBeClickable(DetalleEle.BtnGenerateOrder));
            DetalleEle.ClicBtnGenerateOrder();
            WaitSpinner();
        }

        public bool PaidSaleCompleteOrder() {
            ModalCobro modalCobro = new ModalCobro(Driver1);
            string cobro = modalCobro.GetImporteTotal();
            modalCobro.PutAmount("EFECTIVO", cobro);
            string TotalRecibodo = modalCobro.GetTotalRecibido();
            string Restante = modalCobro.GetRestante();
            string Cambio = modalCobro.GetCambio();
            modalCobro.ClicBtnConfirmarCobro();
            WaitSpinner();
            return (TotalRecibodo == cobro) && (Restante == "0,00") && (Cambio == "0,00");
        }
        public bool PaidMoreSaleOrder()
        {
            ModalCobro modalCobro = new ModalCobro(Driver1);
            string cobro = modalCobro.GetImporteTotal();
            Random randomExtraMoney = new Random();
            double doubleRan=randomExtraMoney.NextDouble();
            int cobrof = (int)Math.Round(double.Parse(cobro));
            double CobroMas = doubleRan * cobrof;
            string cobroMasString= (CobroMas + double.Parse(cobro)).ToString("0.00");
            modalCobro.PutAmount("EFECTIVO", cobroMasString);

            string TotalRecibodo = modalCobro.GetTotalRecibido();
            string Restante = modalCobro.GetRestante();
            string Cambio = modalCobro.GetCambio();
            modalCobro.ClicBtnCancelar();
            WaitSpinner();
            string sobraCambio = CobroMas.ToString("0.00");
            return (TotalRecibodo == cobroMasString) && (Restante == "0,00") && (Cambio == sobraCambio);
        }
        public string ImprimeTicket() {
            ModalCompraFinalizada compraFinalizada = new ModalCompraFinalizada(Driver1);
            compraFinalizada.WaitModalCobro();
            string FolioVentaCompleta=compraFinalizada.GetFolioVenta();

            compraFinalizada.ClicBtnPrintReceipt();
            WaitSpinner();
            WaitToNewWindow();
            return FolioVentaCompleta;
        }
        public void CloseModalCompraFinalizada() {
            ModalCompraFinalizada compraFinalizada = new ModalCompraFinalizada(Driver1);
            compraFinalizada.WaitModalCobro();
            compraFinalizada.ClicBtnSalir();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='SaleChange']")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='AdvanceCollector']")));
        }

        public void DeleteQuotation() {
            wait.Until(ExpectedConditions.ElementToBeClickable(DetalleEle.BtnCancelOrder));
            DetalleEle.ClicBtnCancelOrder();
            WaitSpinner();
            clickSwal2Button("Aceptar");
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='tabs-header']")));
        }

        public void GetDetailsFromPedido() {
            

        }
    }
}
