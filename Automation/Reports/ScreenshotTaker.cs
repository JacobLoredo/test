using NUnit.Framework;
using NLog;
using OpenQA.Selenium;
using System;
using NUnit.Framework.Interfaces;
using System.Linq;
using System.IO;

namespace Automation.Reports
{
    public class ScreenshotTaker
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IWebDriver _driver;
        private readonly TestContext _testContext;
        public string ScreenshotFinelPath { get; set; }
        private string ScreenshotFileName { get; set; }
        private string DirectoryName { get; set; }
        public ScreenshotTaker(IWebDriver driver, TestContext testContext) {
            if (driver == null)
                return;
            _driver = driver;
            _testContext = testContext;
            var arrayTex=_testContext.Test.ClassName.Split(".");
            DirectoryName = _testContext.Test.ClassName.Replace("Automation.Scenarios_Test.Modules.", "").Replace("."+ arrayTex.Last(),"").Replace(".","\\");
            ScreenshotFileName = _testContext.Test.Name;
        }
        public void CreateScreenshotIfTestFailed() {
            var status = _testContext.Result.Outcome.Status;
            if (status == TestStatus.Failed || status == TestStatus.Inconclusive)
            {
                TakeScreenShotForFailure();
            }
            else {
                ScreenshotFinelPath = $"{Path.Combine(Reporter.LatestResultsReporterFolder, DirectoryName, DateTime.Now.ToString("dd-MM-yyyy"))}";
                ScreenshotFinelPath = ScreenshotFinelPath.Replace('/', ' ').Replace('"', ' ');
                Directory.CreateDirectory($"{Path.Combine(Reporter.LatestResultsReporterFolder, DirectoryName, DateTime.Now.ToString("dd-MM-yyyy"))}");
            }
        }
        public string TakeScreenshot(string screeenshotFileName) {
            var ScrenS = GetScreenshot();
            var successfullySavede = TryToSaveScreenshot(screeenshotFileName, ScrenS);
            return successfullySavede ? ScreenshotFinelPath : "";
        }
        public bool TakeScreenShotForFailure() {
            ScreenshotFileName = $"Fail_{ScreenshotFileName}";
            var ScrenS = GetScreenshot();
            var successfullySavede = TryToSaveScreenshot(ScreenshotFileName, ScrenS);
            if (successfullySavede) {
                Log.Error($"Screenshot Error=>{ScreenshotFinelPath}");
            }
            return successfullySavede;
        }
        public Screenshot GetScreenshot() {
            return ((ITakesScreenshot)_driver).GetScreenshot();
        }
        private bool TryToSaveScreenshot(string screenhotFileName, Screenshot ss) {
            try
            {
                SaveScreenshot(screenhotFileName, ss);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.InnerException);
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                return false;

            }
        }
        private void SaveScreenshot(string screenhotName,Screenshot ss) {
            if (ss == null)
                return;
            Directory.CreateDirectory($"{Path.Combine(Reporter.LatestResultsReporterFolder, DirectoryName, DateTime.Now.ToString("dd-MM-yyyy"))}");
            ScreenshotFinelPath = $"{Path.Combine(Reporter.LatestResultsReporterFolder, DirectoryName,DateTime.Now.ToString("dd-MM-yyyy"))}\\{screenhotName}.png";
            ScreenshotFinelPath = ScreenshotFinelPath.Replace('/', ' ').Replace('"',' ');
            ss.SaveAsFile(ScreenshotFinelPath,ScreenshotImageFormat.Png);
        }
    }
}
