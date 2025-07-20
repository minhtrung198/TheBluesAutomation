using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using TheBluesAutomation.Utils;
using TheBluesAutomation.Pages;

namespace TheBluesAutomation.Tests
{
    [TestClass]
    public class LoginTests
    {
        private IWebDriver? driver;
        private ExtentReports? extent;
        private ExtentTest? test;
        private LoginPage? loginPage;

        [TestInitialize]
        public void Setup()
        {
            extent = ExtentManager.GetInstance();
            test = extent.CreateTest("LoginTests");

            var options = new ChromeOptions();
            options.AddArgument("--user-agent=Mozilla/5.0 (Linux; Android 13; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Mobile Safari/537.36");
            options.AddArgument("--window-size=360,780");
            driver = new ChromeDriver("C:\\chromedriver\\chromedriver.exe", options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            loginPage = new LoginPage(driver);
        }

        [TestMethod]
        public void TestSuccessfulLogin()
        {
            try
            {
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to theblues.com.vn");

                loginPage!.Login("pnmtrung106@gmail.com", "rG1!b(oKxNR#");
                test!.Log(Status.Info, "Performed login action");

                test!.Log(Status.Info, $"Current URL after login: {driver.Url}");
                string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, "AfterLogin");
                test!.AddScreenCaptureFromPath(screenshotPath);

                var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(30));

                var welcomeMessage = wait.Until(d =>
                {
                    var element = d.FindElement(By.CssSelector(".woocommerce-MyAccount-navigation"));
                    return element.Displayed ? element : throw new NoSuchElementException();
                });

                Assert.IsTrue(welcomeMessage.Text.Contains("XIN CHÀO", StringComparison.OrdinalIgnoreCase), "Thông báo chào mừng không đúng!");
                test!.Log(Status.Pass, "Login successful");
            }
            catch (Exception ex)
            {
                string failScreenshot = ScreenshotHelper.CaptureScreenshot(driver!, "LoginFail");
                test!.AddScreenCaptureFromPath(failScreenshot);
                test!.Log(Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void TestFailedLogin()
        {
            try
            {
                driver!.Navigate().GoToUrl("https://theblues.com.vn/");
                test!.Log(Status.Info, "Navigated to theblues.com.vn");

                loginPage!.Login("test123@gmail.com", "123123");
                test!.Log(Status.Info, "Performed login action with invalid credentials");

                var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(30));
                var errorMessage = wait.Until(d => d.FindElement(By.ClassName("woocommerce-error")));

                Assert.IsTrue(errorMessage.Text.Contains("Tài khoản không đúng"), "Thông báo lỗi không hiển thị!");
                test!.Log(Status.Pass, "Error message displayed correctly");
            }
            catch (Exception ex)
            {
                test!.Log(Status.Fail, $"Test failed: {ex.Message}");
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