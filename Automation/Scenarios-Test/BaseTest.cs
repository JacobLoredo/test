using Automation.Reports;
using AventStack.ExtentReports;
using NUnit.Framework;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Diagnostics;

namespace Automation.Scenarios_Test
{
    [TestFixture]
    
    [Parallelizable(scope: ParallelScope.Fixtures)]

    public class BaseTest
    {
        [ThreadStatic]
        protected static IWebDriver Driver;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private ScreenshotTaker TakePick;

        [OneTimeSetUp]
        public static void ExecuteForCreatingReportsNamespace()
        {
            Reporter.StartReporter();
        }

        [SetUp]
        public void Initialize()
        {

           // Trace.WriteLine(TestContext.Test.Name);
           
            //checar testContext ya que llega como null y si lo dejo como esta toma al primer test como el unico para el reporte
            Log.Debug("Pruebas iniciadas");
            //TestContext = Reporter.MyTestContext;
            Reporter.AddTestCaseMetadataToHtmlReport(TestContext.CurrentContext);
            Driver = new EdgeDriver();
            Driver.Manage().Window.Maximize();
            TakePick = new ScreenshotTaker(Driver, TestContext.CurrentContext);
        }

        [TearDown]
        public void AfterTest()
        {

            Log.Debug(GetType().FullName + "Empezo un metodo");
            try
            {
                TakeScreenshotForTestFailure();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Source);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException);
                Log.Error(ex.Message);

            }
            finally
            {
                TearDown();
                Log.Debug(GetType().FullName);
            }
        }
        public void TearDown()
        {
            if (Driver == null)
                return;

            Driver.Quit();
            Driver = null;
        }
        private void TakeScreenshotForTestFailure()
        {
            if (TakePick != null)
            {
                TakePick.CreateScreenshotIfTestFailed();
                Reporter.ReportTestOutcome(TakePick.ScreenshotFinelPath, TakePick.ScreenshotFinelPath);
            }
            else
            {
                Reporter.ReportTestOutcome("","");
            }
        }
    }
}
