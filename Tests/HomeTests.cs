using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using TheBluesAutomation.Utils;
using TheBluesAutomation.Pages;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TheBluesAutomation.Tests
{
    [TestClass]
    public class HomeTests
    {
        private IWebDriver? driver;
        private static ExtentReports? extent;
        private ExtentTest? test;

        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                extent = ExtentManager.GetInstance();
                test = extent.CreateTest("HomeTests");

                var options = new ChromeOptions();
                options.AddArgument("--headless");  // Headless mode cho CI
                options.AddArgument("--no-sandbox");  // Tránh lỗi sandbox
                options.AddArgument("--disable-dev-shm-usage");  // Tăng hiệu suất
                options.AddArgument("--disable-gpu");  // Tắt GPU
                options.AddArgument("--remote-debugging-port=9222");  // Debug nếu cần
                options.AddArgument("--disable-extensions");  // Tắt extensions gây crash
                options.AddArgument("--window-size=1920,1080");  // Đặt kích thước window

                // Kiểm tra môi trường CI
                bool isCI = Environment.GetEnvironmentVariable("CI") == "true";
                if (!isCI)
                {
                    // Local: dùng WebDriverManager để tự tải driver
                    new DriverManager().SetUpDriver(new ChromeConfig());
                }

                driver = new ChromeDriver(options);  // Không cần path, dùng PATH từ WebDriverManager
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            }
            catch (Exception ex)
            {
                test!.Log(Status.Fail, $"Failed to initialize driver: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void NavigateToBluesWebsite()
        {
            try
            {
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/ successfully");

                test!.Log(Status.Pass, "Test stopped at https://theblues.com.vn/");
            }
            catch (Exception ex)
            {
                string screenshotPath = string.Empty;
                try
                {
                    if (driver != null)
                    {
                        screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, "NavigateFail");
                    }
                }
                catch { /* Ignore if screenshot fails */ }
                test!.Log(Status.Fail, $"Test failed: {ex.Message} - Screenshot: {screenshotPath}");
                throw;
            }
        }

        [TestMethod]
        public void NavigateToMenCategory()
        {
            try
            {
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/ successfully");

                var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(30));
                var firstCategoryLink = wait.Until(d => d.FindElement(By.CssSelector(".elementor-column:nth-child(1) .elementor-widget-container a[href='/product-category/thoi-trang-nam/']")));

                firstCategoryLink.Click();
                test!.Log(Status.Info, "Clicked on first category: THỜI TRANG NAM");

                wait.Until(d => d.Url.Contains("/product-category/thoi-trang-nam/"));
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/product-category/thoi-trang-nam/");

                test!.Log(Status.Pass, "Test stopped at https://theblues.com.vn/product-category/thoi-trang-nam/");
            }
            catch (Exception ex)
            {
                string screenshotPath = string.Empty;
                try
                {
                    if (driver != null)
                    {
                        screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, "NavigateCategoryFail");
                    }
                }
                catch { /* Ignore if screenshot fails */ }
                test!.Log(Status.Fail, $"Test failed: {ex.Message} - Screenshot: {screenshotPath}");
                throw;
            }
        }

        [TestMethod]
        public void NavigateToWomanCategory()
        {
            try
            {
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/ successfully");

                var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(30));
                var firstCategoryLink = wait.Until(d => d.FindElement(By.CssSelector(".elementor-column:nth-child(2) .elementor-widget-container a[href='/product-category/thoi-trang-nu/']")));

                firstCategoryLink.Click();
                test!.Log(Status.Info, "Clicked on first category: THỜI TRANG NỮ");

                wait.Until(d => d.Url.Contains("/product-category/thoi-trang-nu/"));
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/product-category/thoi-trang-nu/");

                test!.Log(Status.Pass, "Test stopped at https://theblues.com.vn/product-category/thoi-trang-nu/");
            }
            catch (Exception ex)
            {
                string screenshotPath = string.Empty;
                try
                {
                    if (driver != null)
                    {
                        screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, "NavigateCategoryFail");
                    }
                }
                catch { /* Ignore if screenshot fails */ }
                test!.Log(Status.Fail, $"Test failed: {ex.Message} - Screenshot: {screenshotPath}");
                throw;
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
             driver?.Quit();
            extent?.Flush();
        }
    }
}