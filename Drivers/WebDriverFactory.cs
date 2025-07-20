using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace TheBluesAutomation.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            return new ChromeDriver(options);
        }
    }
}
