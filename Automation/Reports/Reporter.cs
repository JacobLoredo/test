using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NLog;
using NUnit.Framework;
using System.IO;
using NUnit.Framework.Interfaces;
using System.Linq;
using System;
using AventStack.ExtentReports.MarkupUtils;

namespace Automation.Reports
{

    public static class Reporter
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static ExtentReports ReportManager { get; set; }
        private static string ApliccationDebugingFolder => "Reports\\Modules";
        private static string HtmlReportFullPath { get; set; }
        public static string LatestResultsReporterFolder { get; set; }
        public static TestContext MyTestContext { get; set; }
        private static ExtentTest CurrentTestCase { get; set; }

        public static void StartReporter()
        {
            Log.Trace("Starting a one time setup for the entire" + ".CreatingReports namespace." + "Going to initialize the reporter next...");
            CreateReportDirectory();
            var htmlReporter = new ExtentHtmlReporter(HtmlReportFullPath);
            ReportManager = new ExtentReports();
            ReportManager.AttachReporter(htmlReporter);
        }
        private static void CreateReportDirectory()
        {
            var actualDirectory = Directory.GetCurrentDirectory().Replace("bin\\Debug\\net5.0", ApliccationDebugingFolder);
            var filePath = Path.GetFullPath(actualDirectory);
            LatestResultsReporterFolder =filePath;
            Directory.CreateDirectory(LatestResultsReporterFolder);
            HtmlReportFullPath = $"{LatestResultsReporterFolder}\\TestResults.html";
            Log.Trace("Full path of HTML report=> " + HtmlReportFullPath);
        }
        public static void AddTestCaseMetadataToHtmlReport(TestContext testContext)
        {
            
            MyTestContext = testContext;
           // var description = MyTestContext.Test.Properties["Description"].First().ToString();
            CurrentTestCase = ReportManager.CreateTest(MyTestContext.Test.Name);
            AddAuthors(MyTestContext);
            AddCategories(MyTestContext);
        }
        public static void AddAuthors(TestContext testContext)
        {
            var Authores = testContext.Test.Properties["Author"].ToList();
            for (int i = 0; i < Authores.Count(); i++)
            {
                
                CurrentTestCase.AssignAuthor(Authores[i].ToString());
            }

        }
        public static void AddCategories(TestContext testContext) {
           var categories= testContext.Test.Properties["Category"].ToList();
            for (int i = 0; i < categories.Count(); i++) { 
                CurrentTestCase.AssignCategory(categories[i].ToString());
            }
        }
        public static void LogPassingTestStepForBugLogger(string message)
        {
            Log.Info(message);
            CurrentTestCase.Log(Status.Pass, message);
        }
        
        public static void NewAssignCategory() { 
        
        }
        public static void ReportTestOutcome(string screenshotPath,string newPath)
        {
            var status = MyTestContext.Result.Outcome.Status;
            switch (status)
            {
                case TestStatus.Inconclusive:
                    CurrentTestCase.AddScreenCaptureFromPath(screenshotPath);
                    CurrentTestCase.Warning("Test sin concluir");
                    break;
                case TestStatus.Skipped:
                    CurrentTestCase.Skip("Test omitido");
                    break;
                case TestStatus.Passed:
                    CurrentTestCase.Pass("El test se completo con exito");
                    break;
                case TestStatus.Failed:
                    Log.Error($"Test Failed-> {MyTestContext.Test.MethodName}");
                    CurrentTestCase.AddScreenCaptureFromPath(screenshotPath);
                    CurrentTestCase.Fail("El caso de prueba a fallado");
                    break;
                default:
                    break;
            }
            
            ReportManager.Flush();
            var dirReporte=HtmlReportFullPath.Replace("TestResults.html", "index.html");
            var splitNewPath = newPath.Split("\\");
            var v = TestStatus.Failed== status?newPath.Replace(splitNewPath.Last(),""): newPath+"\\";
            if (File.Exists(v + "Reporte.html"))
                File.Delete(v + "Reporte.html");
            File.Move(dirReporte, v+"Reporte.html");

        }

        public static void LogTestStepForBugLogger(Status status,string message) {
            Log.Info(message);
            CurrentTestCase.Log(status,message);
        }
        public static void LogTestStepForBugLoggerJSON(Status stauts, string messaageJSON) {
            CurrentTestCase.Log(stauts, MarkupHelper.CreateCodeBlock(messaageJSON, CodeLanguage.Json));
        }


    }
}
