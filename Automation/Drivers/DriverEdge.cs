using OpenQA.Selenium;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using System;

namespace Automation.Drivers
{
    public static class DriverEdge
    {
        public static ExtentTest test;
        public static ExtentReports extent;
        public static IWebDriver Driver { get; set; }

        public static void WaitForElementUpTo(int seconds = 5)
        {

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }
    }
}

