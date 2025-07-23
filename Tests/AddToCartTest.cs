using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TheBluesAutomation.Drivers;
using TheBluesAutomation.Pages;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Chrome;
using TheBluesAutomation.Utils;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace TheBluesAutomation.Tests
{
    [TestClass]
    public class AddToCartTest
    {
        private IWebDriver? driver;
        private ProductPage? productPage;
        private CartPage? cartPage;
        private LoginPage? loginPage;
        private static ExtentReports? extent;
        private ExtentTest? test;

        [TestInitialize]
        public void TestInitialize()
        {
            extent = ExtentManager.GetInstance();
            test = extent.CreateTest("AddToCartTests");

            var options = new ChromeOptions();
            // Disable mobile để test desktop (comment in nếu cần mobile sau khi pass)
            // options.AddArgument("--user-agent=Mozilla/5.0 (Linux; Android 13; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Mobile Safari/537.36");
            // options.AddArgument("--window-size=360,780");
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--start-maximized");
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            loginPage = new LoginPage(driver);
            productPage = new ProductPage(driver);
            cartPage = new CartPage(driver);
        }

        [TestMethod]
        public void AddSingleItemToCartSuccessfully()
        {
            test = extent!.CreateTest("Add Single Item to Cart");

            try
            {
                driver!.Navigate().GoToUrl("https://theblues.com.vn/san-pham/ao-khoac-du-nam-2-lop-qn2-kj1l-23-031r/");
                test.Log(Status.Info, "Navigated to product page");

                productPage!.SelectAllAvailableVariations();
                System.Threading.Thread.Sleep(1000);
                productPage.SetQuantity(1);

                string productTitle = productPage.GetProductTitle();
                string productPrice = productPage.GetPrice();

                productPage.ClickAddToCart();
                System.Threading.Thread.Sleep(2000);
                productPage.HandleUnexpectedAlertIfAny(); // Handle alert ngay sau add
                productPage.ClickViewCartInPopup();
                System.Threading.Thread.Sleep(2000); // Wait cart load

                // Switch to main window nếu cần
                driver.SwitchTo().Window(driver.WindowHandles.Last());

                Assert.IsTrue(productPage.IsSuccessPopupDisplayed(), "Popup thông báo thành công phải hiển thị");
                productPage.GoToCartAfterAdd();

                Assert.AreEqual(1, cartPage!.GetCartItemCount(), "Cart should contain 1 item");
                Assert.AreEqual(productTitle, cartPage.GetFirstItemTitle(), "Product title should match");
                Assert.AreEqual(productPrice, cartPage.GetFirstItemPrice(), "Product price should match");

                test.Log(Status.Pass, "Test stopped after viewing the cart successfully");
                test.Log(Status.Pass, "Test passed successfully");
            }
            catch (Exception ex)
            {
                string screenshotPath = string.Empty;
                try
                {
                    if (driver != null)
                    {
                        screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, "AddToCartFail");
                    }
                }
                catch { /* Ignore if screenshot fail */ }
                test.Log(Status.Fail, $"Test failed: {ex.Message} - Screenshot: {screenshotPath}");
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