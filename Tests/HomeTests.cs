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
            extent = ExtentManager.GetInstance();
            test = extent.CreateTest("HomeTests");

            var options = new ChromeOptions();
            options.AddArgument("--headless");  // Headless mode bắt buộc cho CI
            options.AddArgument("--no-sandbox");  // Tránh lỗi sandbox trên CI
            options.AddArgument("--disable-dev-shm-usage");  // Tăng bộ nhớ
            options.AddArgument("--disable-gpu");  // Tắt GPU
            options.AddArgument("--remote-debugging-port=9222");

            // Khởi tạo ChromeDriver (đảm bảo path đúng với máy local)
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [TestMethod]
        public void NavigateToBluesWebsite()
        {
            try
            {
                // Điều hướng đến trang đích
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/ successfully");

                // Dừng tại đây, không thực hiện thêm hành động
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
                // Điều hướng đến trang chủ
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/ successfully");

                // Chờ các category load
                var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(30));
                var firstCategoryLink = wait.Until(d => d.FindElement(By.CssSelector(".elementor-column:nth-child(1) .elementor-widget-container a[href='/product-category/thoi-trang-nam/']")));

                // Nhấp vào category đầu tiên (THỜI TRANG NAM)
                firstCategoryLink.Click();
                test!.Log(Status.Info, "Clicked on first category: THỜI TRANG NAM");

                // Chờ trang đích load
                wait.Until(d => d.Url.Contains("/product-category/thoi-trang-nam/"));
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/product-category/thoi-trang-nam/");

                // Dừng tại đây, không thực hiện thêm hành động
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
                // Điều hướng đến trang chủ
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/ successfully");

                // Chờ các category load
                var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(30));
                var firstCategoryLink = wait.Until(d => d.FindElement(By.CssSelector(".elementor-column:nth-child(2) .elementor-widget-container a[href='/product-category/thoi-trang-nu/']")));

                // Nhấp vào category đầu tiên (THỜI TRANG NAM)
                firstCategoryLink.Click();
                test!.Log(Status.Info, "Clicked on first category: THỜI TRANG NU");

                // Chờ trang đích load
                wait.Until(d => d.Url.Contains("/product-category/thoi-trang-nu/"));
                test!.Log(Status.Info, "Navigated to https://theblues.com.vn/product-category/thoi-trang-nu/");

                // Dừng tại đây, không thực hiện thêm hành động
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