using OpenQA.Selenium;
using System.IO;

namespace TheBluesAutomation.Utils
{
    public static class ScreenshotHelper
    {
        public static string CaptureScreenshot(IWebDriver driver, string screenshotName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", $"{screenshotName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            screenshot.SaveAsFile(filePath);
            return filePath;
        }
    }
}