
using NUnit.Framework;

namespace Automation.Reports
{
   
    public static class NamespaceSetup

    { 
        public static void ExecuteForCreatingReportsNamespace(TestContext testContext) {
            Reporter.StartReporter();
        }
    }
}
