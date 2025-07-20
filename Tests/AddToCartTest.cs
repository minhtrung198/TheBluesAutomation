using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TheBluesAutomation.Drivers;
using TheBluesAutomation.Pages;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Chrome;
using TheBluesAutomation.Utils;

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
        //private ExtentReports? extent;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            extent = new ExtentReports();
            Directory.CreateDirectory("Reports");
            var reporter = new ExtentHtmlReporter("Reports/AddToCartReport.html");
            extent.AttachReporter(reporter);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            extent = ExtentManager.GetInstance();
            test = extent.CreateTest("AddToCartTests");

            var options = new ChromeOptions();
            options.AddArgument("--user-agent=Mozilla/5.0 (Linux; Android 13; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Mobile Safari/537.36");
            options.AddArgument("--window-size=360,780");
            driver = new ChromeDriver("C:\\chromedriver\\chromedriver.exe", options);
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
                productPage.SetQuantity(1);

                string productTitle = productPage.GetProductTitle();
                string productPrice = productPage.GetPrice();

                productPage.ClickAddToCart();
                productPage.HandleUnexpectedAlertIfAny();

                Assert.IsTrue(productPage.IsSuccessPopupDisplayed(), "Popup thông báo thành công phải hiển thị");
                productPage.GoToCartAfterAdd();
                productPage.ClickViewCartInPopup();

                Assert.AreEqual(1, cartPage!.GetCartItemCount(), "Cart should contain 1 item");
                Assert.AreEqual(productTitle, cartPage.GetFirstItemTitle(), "Product title should match");
                Assert.AreEqual(productPrice, cartPage.GetFirstItemPrice(), "Product price should match");

                test.Log(Status.Pass, "Test passed successfully");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        //[TestMethod]
        //public void AddToCart_EmptyQuantity_ShouldShowError()
        //{
        //    test = extent.CreateTest("Add to Cart with Empty Quantity");

        //    try
        //    {
        //        // Arrange
        //        driver.Navigate().GoToUrl("https://theblues.com.vn/san-pham/ao-khoac-du-nam-2-lop-qn2-kj1l-23-031r/");

        //        // Act
        //        productPage.SetQuantity(0);
        //        productPage.ClickAddToCart();


        //        test.Log(Status.Pass, "Empty quantity validation test passed");
        //    }
        //    catch (Exception ex)
        //    {
        //        test.Log(Status.Fail, $"Test failed: {ex.Message}");
        //        throw;
        //    }
        //}

        [TestCleanup]
        public void Cleanup()
        {
            driver?.Quit();
            extent?.Flush();
        }
    }
}