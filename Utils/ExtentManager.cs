using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace TheBluesAutomation.Utils
{
    public static class ExtentManager
    {
        private static ExtentReports? extent;
        private static ExtentSparkReporter? sparkReporter;

        public static ExtentReports GetInstance()
        {
            if (extent == null)
            {
                string reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                if (!Directory.Exists(reportDir))
                {
                    Directory.CreateDirectory(reportDir);
                }
                string reportPath = Path.Combine(reportDir, $"ExtentReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");

                sparkReporter = new ExtentSparkReporter(reportPath);
                extent = new ExtentReports();
                extent.AttachReporter(sparkReporter);
            }
            return extent;
        }
    }
}
