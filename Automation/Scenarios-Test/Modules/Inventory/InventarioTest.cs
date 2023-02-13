using Automation.Config.QA;
using Automation.Pages.CommonElements;
using Automation.Pages.Modules.Inventario.Actions;
using Automation.Pages.Modules.LocalizadorRefacciones.Actions;
using Automation.Pages.Modules.Login.Actions;
using Automation.Reports;
using AventStack.ExtentReports;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using Reporter = Automation.Reports.Reporter;

namespace Automation.Scenarios_Test.Modules.Inventory
{
    [TestFixture]
    [Parallelizable(scope: ParallelScope.Fixtures)]
    public class InventarioTest : BaseTest
    {
        ActionsInventory inventory;
        ConfigQA.Credentials credentials;
        ActionsLogin actionsLogin;
        Menu menu;
        [SetUp]
        public void Inicializar()
        {
            try
            {
                credentials = new ConfigQA.Credentials();
                inventory = new ActionsInventory(Driver);
                actionsLogin = new ActionsLogin(Driver);
                menu = new Menu(Driver);
                actionsLogin.LoginUser(credentials.user, credentials.password);
                Assert.IsTrue(actionsLogin.isSelectWorkshopPage());
                actionsLogin.SelectWorskshopByName("280");
                menu.clikElementMenu("Inventario");
            }
            catch (AssertionException a)
            {
                Reporter.LogTestStepForBugLogger(Status.Fail, a.Message + a.InnerException);

            }

        }

        [TestCase("Existente", Author = "Jacob", Category = "Inventario - Agregar"), Property("ID", 35753)]
        [TestCase("Completa", Author = "Jacob", Category = "Inventario - Agregar"), Property("ID", 35754)]
        public void AgregarRefaccionOriginal(string Refaccion)
        {
            List<string> ListSKU = inventory.GetSKUsFromExisten();
            if (Refaccion.Equals("Completa"))
            {
                string SKU = inventory.ReturnSKURefaRandom(ListSKU);
                inventory.AddRefaToInventory(SKU, "Refaccion QAutomation", 10, 49, Marcas: "Marca QA", StartYear: "01/01/2008", EndYear: "01/01/2021", MinStock: 15, MaxStock: 100, Category: "QA", Origin: "Automatizacion", Manufacture: "Automatizacion", Location: "LocationQA");
                Assert.IsTrue(inventory.IsToastSuccess());
                List<string> NewListSKU = inventory.GetSKUsFromExisten();
                Assert.Greater(NewListSKU.Count, ListSKU.Count);
            }
            else if (Refaccion.Equals("Existente"))
            {
                inventory.AddRefaToInventory(ListSKU[0], "Refaccion QAutomation", 10, 49, Marcas: "Marca QA", StartYear: "01/01/2008", EndYear: "01/01/2021", MinStock: 15, MaxStock: 100, Category: "QA", Origin: "Automatizacion", Manufacture: "Automatizacion", Location: "LocationQA", IsEdit: true);
                Assert.IsTrue(inventory.IsToastError());
                List<string> NewListSKU = inventory.GetSKUsFromExisten();
                Assert.That(ListSKU.Count, Is.EqualTo(NewListSKU.Count));
            }

        }

        [TestCase("Completa", Author = "Jacob", Category = "Inventario - Agregar"), Property("ID", 35754)]
        [TestCase("Existente", Author = "Jacob", Category = "Inventario - Agregar"), Property("ID", 35753)]
        public void AgregarRefaccionGenerica(string Refaccion)
        {
            List<string> ListSKU = inventory.GetSKUsFromExisten();
            if (Refaccion.Equals("Completa"))
            {
                int count = inventory.GenericCount(ListSKU);
                string SKUGeneric = "GenericQA" + count;
                string DescriptionGeneric = "Refaccion QAutomation-" + count;
                inventory.AddRefaToInventory(SKU: SKUGeneric, description: DescriptionGeneric, 10, 49, Marcas: "Marca QA", StartYear: "01/01/2008", EndYear: "01/01/2021", MinStock: 15, MaxStock: 100, Category: "QA", Origin: "Automatizacion", Manufacture: "Automatizacion", Location: "LocatQA");
                Assert.IsTrue(inventory.IsToastSuccess());
                List<string> NewListSKU = inventory.GetSKUsFromExisten();
                Assert.Greater(NewListSKU.Count, ListSKU.Count);
            }
            else if (Refaccion.Equals("Existente"))
            {
                int count = inventory.GenericCount(ListSKU);
                string SKUGeneric = "GenericQA" + (count - 1);
                inventory.AddRefaToInventory(SKUGeneric, "Refaccion QAutomation", 10, 49, Marcas: "Marca QA", StartYear: "01/01/2008", EndYear: "01/01/2021", MinStock: 15, MaxStock: 100, Category: "QA", Origin: "Automatizacion", Manufacture: "Automatizacion", Location: "LocatQA", IsEdit: true);
                Assert.IsTrue(inventory.IsToastError());
                List<string> NewListSKU = inventory.GetSKUsFromExisten();
                Assert.That(ListSKU.Count, Is.EqualTo(NewListSKU.Count));

            }

        }

        [TestCase("NOPERMITIDOS", Author = "Jacob", Category = "Inventario - Alta inventario inicial"), Property("ID", 35951)]
        [TestCase("Existentes", Author = "Jacob", Category = "Inventario - Alta inventario inicial"), Property("ID", 35950)]
        [TestCase("Nueva", Author = "Jacob", Category = "Inventario - Alta inventario inicial"), Property("ID", 35952)]
        [TestCase("SinDatosObligatorios", Author = "Jacob", Category = "Inventario - Alta inventario inicial"), Property("ID", 35986)]
        [TestCase("ValidoObligatorios", Author = "Jacob", Category = "Inventario - Alta inventario inicial"), Property("ID", 35980)]
        [TestCase("DatosCompletos", Author = "Jacob", Category = "Inventario - Alta inventario inicial"), Property("ID", 35981)]
        public void SubirPlantilla(string estado) {
            List<string> ListSKUInInventory = inventory.GetSKUsFromExisten();
            inventory.CreateExcel(ListSKUInInventory, estado, 2);
            inventory.ClicBtnUpdloadSparePart();
            inventory.PutFileExcelInventory();
            if (estado.Equals("NOPERMITIDOS") || estado.Equals("Existentes") || estado.Equals("SinDatosObligatorios"))
            {
                Assert.IsTrue(inventory.IsModalErrorDisplayed());
            }
            else
            {

                inventory.clickSwal2Button("Aceptar");
                List<string> NewListSKU = inventory.GetSKUsFromExisten();
                Assert.Greater(NewListSKU.Count, ListSKUInInventory.Count);
            }
        }

        [TestCase("Original", Author = "Jacob", Category = "Inventario - Modificacion"), Property("ID", 35981)]
        [TestCase("Multimarca", Author = "Jacob", Category = "Inventario - Modificacion"), Property("ID", 35981)]
        public void EditarRefacciones(string TipoRefaccion) {
            List<string> ListSKU = inventory.GetSKUsFromExisten();

            if (TipoRefaccion.Equals("Original"))
            {
                var matchingvalues = ListSKU
                 .Where(stringToCheck => !stringToCheck.Contains("GENERICQA"));
                int num = matchingvalues.Count();
                List<string> ListSKUa = matchingvalues.ToList();
                string SKURand = inventory.ReturnRandomMemberList(ListSKUa);
                inventory.ClicActionBySKU(SKURand, "Editar");
                inventory.EditRefaToInventory(TipoRefaccion);
                Assert.IsTrue(inventory.IsToastSuccess());
            }
            else
            {
                var matchingvalues = ListSKU.Where(stringToCheck => stringToCheck.Contains("GENERICQA"));
                int num = matchingvalues.Count();
                List<string> ListSKUa = matchingvalues.ToList();
                string SKURand = inventory.ReturnRandomMemberList(ListSKUa);
                inventory.ClicActionBySKU(SKURand, "Editar");
                inventory.EditRefaToInventory(TipoRefaccion);
                Assert.IsTrue(inventory.IsToastSuccess());
            }


        }

        public void OrdenTrabajoTaller() { 
            
        }
    
    }
}
