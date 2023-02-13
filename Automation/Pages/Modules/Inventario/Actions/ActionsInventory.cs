using Automation.Pages.CommonElements;
using AventStack.ExtentReports.Utils;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using SpreadsheetLight;
using System;
using System.Collections.Generic; 
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using ActionsSelenium = OpenQA.Selenium.Interactions.Actions;
namespace Automation.Pages.Modules.Inventario.Actions
{
    public class ActionsInventory:InventoryPage
    {
        private IWebDriver Driver;
        public IWebDriver Driver1 { get => Driver; set => Driver = value; }

       
        public ActionsInventory(IWebDriver driver) : base(driver)
        {
            Driver = driver;
        }
        private void ClicAddRefa() {
            DisplayNoneNavbar();
            ActionsSelenium actions = new ActionsSelenium(Driver1);
            actions.MoveToElement(InventoryEle.BtnAddSpareParts).Click().Build().Perform();
        }
        public void ClicBtnUpdloadSparePart() {
            DisplayNoneNavbar();
            ActionsSelenium actions = new ActionsSelenium(Driver1);
            actions.MoveToElement(InventoryEle.BtnUpdloadSparePart).Click().Build().Perform();
        }
        public string ReturnSKURefaRandom(List<string> ListSKU) {
            List<string> ListRefas= ReadExcelRefas();

            Random ran = new Random();
            string RefaRand=ListRefas[ran.Next(ListRefas.Count)];
            bool IsRepeat=CheckSKUDontExistInInventory(ListSKU, RefaRand);
            while (IsRepeat)
            {
                RefaRand = ListRefas[ran.Next(ListRefas.Count)];
                IsRepeat = CheckSKUDontExistInInventory(ListSKU, RefaRand);
            }
            return RefaRand;
        }
        private bool CheckSKUDontExistInInventory(List<string> ListInventory,string SKURefa) { 
            return ListInventory.Contains(SKURefa);
        }
        private List<string> ReadExcelRefas() {
            string ActualD = Directory.GetCurrentDirectory();
            string ExcelRefasDirectory = ActualD.Replace("bin\\Debug\\net5.0", "Documents\\CatRefacciones.xlsx");
            DataSet RefasDataSet = ExcelFileReader(ExcelRefasDirectory);
            List<string> ListRefas = DataSetToList(RefasDataSet);
            return ListRefas;
        }
        private List<string> DataSetToList(DataSet dataSet) {
            List<string> list = new List<string>();
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                list.Add(dr.ItemArray[0].ToString());
            }
            return list;
        }
        public List<string> GetSKUsFromExisten() {
            WaitSpinner();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='sparePartTable']")));
           
            List<string>  ListSKU = new List<string>();
            List<IWebElement> Pagination = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[@id='sparePartsTableContainer']//ul[@class='pagination']/li")));
            int numPag = Pagination.Count() - 2;
            Thread.Sleep(400);
            Pagination[1].Click();
            while (numPag>0)
            {
              
                List<IWebElement> ListRefas = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='sparePartTable']/tbody/tr[@id]")));
                foreach (IWebElement Refa in ListRefas)
                {
                    List<IWebElement> ListTD = new List<IWebElement>(Refa.FindElements(By.TagName("td")));
                    ListSKU.Add(ListTD[1].Text);
                }
                numPag--;
                if (numPag>0)
                {
                    IWebElement nextPagination = Driver1.FindElement(By.XPath("//*[@id='sparePartTable_next']"));
                    ActionsSelenium actionsSel = new ActionsSelenium(Driver1);
                    actionsSel.MoveToElement(nextPagination).Perform();
                    nextPagination.Click();
                }
            }
            
           
            return ListSKU;
        }
        public int GenericCount(List<string> ListSKU)
        {
            var matchingvalues = ListSKU
            .Where(stringToCheck => stringToCheck.Contains("GENERICQA"));
          int c=  matchingvalues.Count();
            return c;
        }
        public void AddRefaToInventory(string SKU, string description, int CantStock, double Cost,
             [Optional] string Marcas, [Optional] string Model,
            [Optional] string StartYear, [Optional] string EndYear, [Optional] int MinStock, [Optional] int MaxStock,
             [Optional] string Category, [Optional] string Origin, [Optional] string Manufacture, [Optional] string Location,
             [Optional]bool IsEdit
            
            ) {
            ClicAddRefa();
            ModalAddNewRefaInv modalAddNewRefa = new ModalAddNewRefaInv(Driver1);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='addSparePartModal']")));
            modalAddNewRefa.AddRefaToInventory(SKU, description, CantStock, Cost, Marcas, Model, StartYear, EndYear, MinStock,
                MaxStock, Category, Origin, Manufacture, Location);
            if (IsEdit)
                modalAddNewRefa.ClicBtnCerrar();
         }
        public void EditRefaToInventory(string TipoRefa) {
            ModalAddNewRefaInv modalAddNewRefa = new ModalAddNewRefaInv(Driver1);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='editSuppliesModal']")));
            if (TipoRefa.Equals("Original"))
            {
                modalAddNewRefa.EditOriginalRefa(MinStock: 11, MaxStock: 100, Category: "Edicion_QA", Location: "EdicionQA");
            } else if (TipoRefa.Equals("Multimarca")) {
                modalAddNewRefa.EditMultiMarcalRefa(description: "Modificacion QA",Marcas:"Marcas Edicion-QA",Model:"Modelo-Edicion-QA",StartYear:"2021",EndYear:"2023",
                    MinStock: 15, MaxStock: 115,  Category: "Edicion_QA",Manufacture:"Edicion Automatizacion", Location: "EdicionQA");
            }
            WaitSpinner();
        }


        public void CreateExcel(List<string> ListSKUInInventory, string option, int cantidad)
        {
            string pathFile = AppDomain.CurrentDomain.BaseDirectory + "ArchivoEx.xlsx";
            SLDocument sLDocument = new SLDocument();
            sLDocument.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Originales");
            DataTable dataTabe = CreateDataTableRefasOriginales(ListSKUInInventory, option, cantidad);
           
            List<string>a= sLDocument.GetWorksheetNames();
            sLDocument.ImportDataTable(1,1,dataTabe, true);

            sLDocument.AddWorksheet("Multimarca");
            DataTable dataTabeMult=CreateDataTableRefasMultiMarca(ListSKUInInventory, option, cantidad);
            sLDocument.ImportDataTable(1, 1, dataTabeMult, true);
            sLDocument.SaveAs(pathFile);
        }
        private DataTable CreateDataTableRefasMultiMarca(List<string> ListSKUInInventory, string option, int cantidad) {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SKU (*)");
            dataTable.Columns.Add("Descripción (*)");
            if (option.Equals("NOPERMITIDOS"))
                dataTable.Columns.Add("Cantidad (*)");
            else
                dataTable.Columns.Add("Cantidad (*)",typeof(int));

            dataTable.Columns.Add("Costo Unitario (*)");
            dataTable.Columns.Add("Máximo");
            dataTable.Columns.Add("Mínimo");
            dataTable.Columns.Add("Ubicación");
            dataTable.Columns.Add("Categoría");
            dataTable.Columns.Add("Fabricante");

            switch (option.ToUpper())
            {
                case ("EXISTENTES"):
                    dataTable.Rows.Add("GENERICQA - 0", "REFACCION QAUTOMATION- 0",23,49.00);
                    break;
                case ("NOPERMITIDOS"):
                    dataTable.Rows.Add("PRUEBA1234", "PRUEBA", 10.20, 10.6528, 10.23, 8.40, "PRUEBA", "PRUEBA", "PRUEBA");
                    break;
                case ("NUEVA"):
                    dataTable = AddNewRefasMultiMarcaToDataTable(dataTable, ListSKUInInventory, cantidad);
                    break;
                case ("SINDATOSOBLIGATORIOS"):
                    dataTable = AddNewRefasMultiMarcaToDataTable(dataTable, ListSKUInInventory, cantidad, option);
                    break;
                case ("VALIDOOBLIGATORIOS"):
                    dataTable = AddNewRefasMultiMarcaToDataTable(dataTable, ListSKUInInventory, cantidad,option);
                    break;
                case ("DATOSCOMPLETOS"):
                    dataTable = AddNewRefasMultiMarcaToDataTable(dataTable, ListSKUInInventory, cantidad);
                    break;
                default:
                    break;
            }
            return dataTable;
        }
        private DataTable AddNewRefasMultiMarcaToDataTable(DataTable dataTable, List<string> ListSKUInInventory, int cantidad, [Optional] string option) {
            int count = GenericCount(ListSKUInInventory);
            for (int i = 0; i < cantidad; i++)
            {
                if (option.IsNullOrEmpty())
                {
                    string SKUGeneric = "GenericQA" + (count+i);
                    dataTable.Rows.Add(SKUGeneric, "Refaccion QAutomation", 10, 10, 80, 10, "LocatQA", "QA", "Automatizacion");
                }
                else if(option.Equals("ValidoObligatorios"))
                {
                    string SKUGeneric = "GenericQA" + (count + i);
                    dataTable.Rows.Add(SKUGeneric, "Refaccion QAutomation", 10, 10);
                }
                else
                    dataTable.Rows.Add(null, null, null, null, 80, 10, "LocatQA", "QA", "Automatizacion");
            }
          
            return dataTable;
        }
        private DataTable CreateDataTableRefasOriginales(List<string> ListSKUInInventory,string option,int cantidad)
        {
            List<string> ListSKUMaster = ReadExcelRefas();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SKU (*)");
            if (option.Equals("NOPERMITIDOS"))
                dataTable.Columns.Add("Cantidad (*)");
            else
                dataTable.Columns.Add("Cantidad (*)", typeof(int));
            dataTable.Columns.Add("Costo Unitario (*)");
            dataTable.Columns.Add("Máximo");
            dataTable.Columns.Add("Mínimo");
            dataTable.Columns.Add("Ubicación");
           
            switch (option.ToUpper())
            {
                case ("EXISTENTES"):
                    dataTable= AddRepetRefas(dataTable, ListSKUInInventory,cantidad);
                    break;
                case ("NOPERMITIDOS"):
                    dataTable.Rows.Add("C01010003", 20.3, 12.182, 20.5, 10.2, "PRUEBA");
                    break;
                case ("NUEVA"):
                    dataTable = AddNewRefasToDataTable(dataTable, ListSKUInInventory, cantidad);
                    break;
                case ("SINDATOSOBLIGATORIOS"):
                    dataTable = AddNewRefasToDataTable(dataTable, ListSKUInInventory, cantidad, option);
                    break;
                case ("VALIDOOBLIGATORIOS"):
                    dataTable = AddNewRefasToDataTable(dataTable, ListSKUInInventory, cantidad);
                    break;
                case ("DATOSCOMPLETOS"):
                    dataTable = AddNewRefasToDataTable(dataTable, ListSKUInInventory, cantidad);
                    break;
                default:
                    break;
            }
            return dataTable;

        }
        private DataTable AddRepetRefas(DataTable dataTable, List<string> ListSKUInInventory, int cantidad)
        {
            Random randCanti = new Random();
            Random randCosto = new Random();
            Random RandRefa = new Random();

            for (int i = 0; i < cantidad; i++)
            {
                int refaIndex = RandRefa.Next(ListSKUInInventory.Count);

                double costo = (randCosto.Next(500));
                dataTable.Rows.Add(ListSKUInInventory[refaIndex], randCanti.Next(30), costo);

            }
            return dataTable;
        }

        private DataTable AddNewRefasToDataTable(DataTable dataTable, List<string> ListSKUInInventory, int cantidad, [Optional]string opcion) {
            Random randCanti = new Random();
            Random randCosto = new Random();
            Random RandRefa = new Random();
            List<string> ListSKUMaster = ReadExcelRefas();

            for (int i = 0; i < cantidad; i++)
            {
                int refaIndex = RandRefa.Next(ListSKUMaster.Count);
               bool IsRepeat= CheckSKUDontExistInInventory(ListSKUInInventory, ListSKUMaster[refaIndex]);
                if (!IsRepeat)
                {
                    if (opcion.IsNullOrEmpty())
                    {
                        string costo = (randCosto.NextDouble() * 2000).ToString("0.00");
                        dataTable.Rows.Add(ListSKUMaster[refaIndex], randCanti.Next(30), costo, 80, 10, "Location-QA");

                    }
                    else
                    {
                        dataTable.Rows.Add(null, null, null, 80, 10, "Location-QA");
                    }
                }

            }
            return dataTable;
        }

        public void PutFileExcelInventory() {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='excelUpload']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='MultiUploadFormId']")));
            IWebElement InputFile = Driver1.FindElement(By.XPath("//input[@type='file' and @class='dz-hidden-input']"));
            string g = AppDomain.CurrentDomain.BaseDirectory + "ArchivoEx.xlsx";
            InputFile.SendKeys(g) ;
            
        }
        public bool IsModalErrorDisplayed() {
            Thread.Sleep(1500);
            IWebElement ModalErrors = Driver1.FindElement(By.XPath("//*[@id='excel-error-list']"));
            return ModalErrors.Displayed;
        }
        public void ClicActionBySKU(string SKU, string Accion) {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='sparePartTable']/tbody")));
           
            List<IWebElement> Pagination = new List<IWebElement>(Driver1.FindElements(By.XPath("//div[@id='sparePartsTableContainer']//ul[@class='pagination']/li")));
            int numPag = Pagination.Count() - 2;
            Pagination[1].Click();
            while (numPag>0)
            {
                List<IWebElement> ListSKU = new List<IWebElement>(Driver1.FindElements(By.XPath("//*[@id='sparePartTable']/tbody/tr[@id]")));
                foreach (IWebElement ListElement in ListSKU)
                {
                    List<IWebElement> List= new List<IWebElement>(ListElement.FindElements(By.TagName("td")));
                    if (List[1].Text.Equals(SKU))
                    {
                        ActionsSelenium actions = new ActionsSelenium(Driver1);
                        actions.MoveToElement(ListElement);
                        DisplayNoneNavbar();
                        ListElement.FindElement(By.TagName("button")).Click();
                        if (Accion.Equals("Editar"))
                        {
                            List<IWebElement>  a= new List<IWebElement>(List[11].FindElements(By.TagName("a")));
                            actions.MoveToElement(a[0]).Click().Build().Perform();
                            //a[0].Click();
                            numPag = 0;
                            Thread.Sleep(1000);
                            break;
                        }
                        else if(Accion.Equals("Detalle"))
                        {

                        }
                    }
                }
                 numPag--;
                if (numPag>0)
                {
                    IWebElement Next = Driver1.FindElement(By.XPath("//*[@id='sparePartTable_next']"));
                    Next.Click();
                }
            }
        }
    }
}
