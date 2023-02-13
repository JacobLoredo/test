using Automation.Reports;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Pages.CommonElements
{
    public class ModalAddNewRefaInv : BasePage
    {
        public IWebDriver driver1;
        public ModalAddNewRefaInv(IWebDriver driver) : base(driver)
        {
            driver1 = driver;
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//*[@id='skuSparePart']")]
        private IWebElement InputSKU { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='supplyDescriptionSP']")]
        private IWebElement InputDescription { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sparePartQuantity']")]
        private IWebElement InputSparePartQuantity { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sparePartCost']")]
        private IWebElement InputSparePartCost { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='brandSparePart']")]
        private IWebElement InputBrandSparePart { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='modelSparePart']")]
        private IWebElement InputModelSparePart { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='yearStartSP']")]
        private IWebElement InputYearStart { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='yearEndSP']")]
        private IWebElement InputYearEnd { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='minQuantityStockSP']")]
        private IWebElement InputMinQuantityStock { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='maxQuantityStockSP']")]
        private IWebElement InputMaxQuantityStock { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sparePartBrand']")]
        private IWebElement InputSparePartBrand { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sparePartOrigin']")]
        private IWebElement InputSparePartOrigin { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sparePartManufacturer']")]
        private IWebElement InputSparePartManufacturer { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sparePartLocation']")]
        private IWebElement InputSparePartLocation { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='saveSparePartSupply']")]
        private IWebElement BtnSave { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='CompatibleBrandsContainer']")]
        private IWebElement ContainerBrands { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='CompatibleModelsContainer']")]
        private IWebElement ContainerModels { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='dateStartContainer']")]
        private IWebElement ContainerDateStart { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='dateEndContainer']")]
        private IWebElement ContainerDateEnd { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='addSparePartModal']//button[@class='close']")]
        private IWebElement BtnCloseModal { get; set; }


        public void ClicBtnCerrar() {
            BtnCloseModal.Click();
        }
        private void ClicSaveBtn() {
            BtnSave.Click();
        }
        public void AddRefaToInventory(string SKU,string description,int CantStock,double Cost,
             [Optional] string Marcas, [Optional] string Model,
            [Optional]string StartYear, [Optional] string EndYear, [Optional] int MinStock, [Optional] int MaxStock,
             [Optional] string Category, [Optional] string Origin, [Optional] string Manufacture, [Optional] string Location
            ) {
            PutInputSKU(SKU);
            if(InputDescription.Enabled)
               PutInputDescription(description);
            PutInputSparePartQuantity(CantStock);
            PutInputSparePartCost(Cost);
            if (Marcas != null && ContainerBrands.Displayed)
                PutInputBrandSparePart(Marcas);
            if (Model != null&& ContainerModels.Displayed)
                PutInputModelSparePart(Model);
            if (StartYear != null&& ContainerDateStart.Displayed)
                PutInputYearStart(StartYear);
            if (EndYear != null && ContainerDateEnd.Displayed)
                PutInputYearEnd(EndYear);
            if (MinStock != 0)
                PutInputMinQuantityStock(MinStock);
            if (MaxStock != 0)
                PutInputMaxQuantityStock(MaxStock);
            if (Category != null)
                PutInputSparePartBrand(Category);
            if (Origin != null&& InputSparePartOrigin.Enabled)
                PutInputSparePartOrigin(Origin);
            if (Manufacture != null&&InputSparePartManufacturer.Enabled)
                PutInputSparePartManufacturer(Manufacture);
            if (Location != null)
                PutInputSparePartLocation(Location);
            ClicSaveBtn();
            WaitSpinner();
        }

        public void EditOriginalRefa([Optional] int MinStock, [Optional] int MaxStock, [Optional] string Category, [Optional] string Location) {
            if (MinStock != 0)
                PutInputMinQuantityStock(MinStock);
            if (MaxStock != 0)
                PutInputMaxQuantityStock(MaxStock);
            if (Category != null)
                PutInputSparePartBrand(Category);
            if (Location != null)
                PutInputSparePartLocation(Location);
            ClicSaveBtn();
            WaitSpinner();
        }
        public void EditMultiMarcalRefa([Optional] string description, [Optional] string Marcas, [Optional] string Model,
              [Optional] string StartYear, [Optional] string EndYear,
            [Optional] int MinStock, [Optional] int MaxStock, [Optional] string Category, [Optional] string Manufacture, [Optional] string Location)
        {
            if (InputDescription.Enabled)
                PutInputDescription(description);
            if (Marcas != null && ContainerBrands.Displayed)
                PutInputBrandSparePart(Marcas);
            if (Model != null && ContainerModels.Displayed)
                PutInputModelSparePart(Model);
            if (StartYear != null && ContainerDateStart.Displayed)
                PutInputYearStart(StartYear);
            if (EndYear != null && ContainerDateEnd.Displayed)
                PutInputYearEnd(EndYear);
            if (MinStock != 0)
                PutInputMinQuantityStock(MinStock);
            if (MaxStock != 0)
                PutInputMaxQuantityStock(MaxStock);
            if (Category != null)
                PutInputSparePartBrand(Category);
            if (Manufacture != null && InputSparePartManufacturer.Enabled)
                PutInputSparePartManufacturer(Manufacture);
            if (Location != null)
                PutInputSparePartLocation(Location);
            ClicSaveBtn();
            WaitSpinner();
        }

        private void PutInputSparePartLocation(string Location)
        {
            InputSparePartLocation.Clear();
            InputSparePartLocation.SendKeys(Location);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Localizacion   : '{Location}'");
        }

        private void PutInputSparePartManufacturer(string Manufacture)
        {
            InputSparePartManufacturer.Clear();
            InputSparePartManufacturer.SendKeys(Manufacture);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Fabricante   : '{Manufacture}'");
        }

        private void PutInputSparePartOrigin(string Origin)
        {
            InputSparePartOrigin.Clear();
            InputSparePartOrigin.SendKeys(Origin);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Origen   : '{Origin}'");
        }

        private void PutInputSparePartBrand(string Category)
        {
            InputSparePartBrand.Clear();
            InputSparePartBrand.SendKeys(Category);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Categoria   : '{Category}'");
        }

        private void PutInputMaxQuantityStock(int MaxStock)
        {
            InputMaxQuantityStock.Clear();
            InputMaxQuantityStock.SendKeys(MaxStock.ToString());
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Stock Maximo   : '{MaxStock}'");
        }
        private void PutInputMinQuantityStock(int MinStock)
        {
            InputMinQuantityStock.Clear();
            InputMinQuantityStock.SendKeys(MinStock.ToString());
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Stock minimo   : '{MinStock}'");
        }
        private void PutInputYearEnd(string year)
        {
           
            InputYearEnd.SendKeys(year);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con año de fin  : '{year}'");
        }

        private void PutInputYearStart(string year)
        {
            InputYearStart.SendKeys(year);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con año de inicio  : '{year}'");
        }

        private void PutInputModelSparePart(string Model)
        {
            InputModelSparePart.Clear();
            InputModelSparePart.SendKeys(Model);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con Modelos compatible  : '{Model}'");
        }

        private void PutInputBrandSparePart(string brand)
        {
            InputBrandSparePart.Clear();
            InputBrandSparePart.SendKeys(brand);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion sus marcas compatibles  : '{brand}'");
        }

        private void PutInputSparePartCost(double Cost)
        {
            InputSparePartCost.Clear();
            InputSparePartCost.SendKeys(Cost.ToString());
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con costo  : '{Cost}'");
        }

        private void PutInputSparePartQuantity(int Quantity)
        {
            InputSparePartQuantity.Clear();
            InputSparePartQuantity.SendKeys(Quantity.ToString());
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con cantidad  : '{Quantity}'");
        }
        private void PutInputDescription(string description)
        {
            WaitSpinner();
            InputDescription.Clear();
            InputDescription.SendKeys(description);
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con descripcion  : '{description}'");
        }
        private void PutInputSKU(string sku)
        {
            InputSKU.Clear();
            InputSKU.SendKeys(sku);
            IWebElement LabelSKU = driver1.FindElement(By.XPath("//div[@id='addSparePartModal']//label[@for='skuSparePartSP']"));
            LabelSKU.Click();
            WaitSpinner();
            Reporter.LogPassingTestStepForBugLogger($"Agregando refaccion con SKU  : '{sku}'");
        }

    }
}
