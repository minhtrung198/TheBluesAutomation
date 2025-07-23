using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TheBluesAutomation.Pages
{
    public class ProductPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // Locators
        private readonly By productTitle = By.CssSelector(".rey-productTitle-wrapper h1");
        private readonly By addToCartButton = By.CssSelector(".single_add_to_cart_button");
        private readonly By quantityInput = By.CssSelector(".quantity");
        private readonly By priceElement = By.CssSelector(".price");
        private readonly By successMessage = By.CssSelector(".rey-modalContent");
        private readonly By productImage = By.CssSelector(".product-image");
        private readonly By popupContainer = By.CssSelector(".rey-acPopup");
        private readonly By viewCartButton = By.CssSelector(".rey-acPopup-buttons a.rey-acPopup-buttons-cart");

        // Locator cho size và reset
        private readonly By sizeOptions = By.CssSelector("ul.variable-items-wrapper.button-variable-wrapper li.variable-item");
        private readonly By resetVariations = By.CssSelector(".reset_variations");

        public ProductPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20)); // Tăng timeout
        }

        public string GetProductTitle()
        {
            return wait.Until(driver => driver.FindElement(productTitle)).Text;
        }

        public void SetQuantity(int quantity)
        {
            var quantityInput = wait.Until(d => d.FindElement(By.CssSelector("input[name='quantity']")));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = arguments[1];", quantityInput, quantity.ToString());
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", quantityInput);
        }

        public void ClickAddToCart()
        {
            try
            {
                var button = wait.Until(d => d.FindElement(addToCartButton));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                Thread.Sleep(200);

                button.Click();
                Console.WriteLine("✅ Clicked Add to Cart button");
            }
            catch (UnhandledAlertException ex)
            {
                Console.WriteLine("⚠️ Alert bật lên khi click Add to Cart: " + ex.Message);
                try
                {
                    IAlert alert = driver.SwitchTo().Alert();
                    Console.WriteLine("⚠️ Nội dung Alert: " + alert.Text);
                    alert.Accept();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("❌ Không thể đóng Alert: " + ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi click Add to Cart: " + ex.Message);
            }
        }

        public string GetPrice()
        {
            return wait.Until(driver => driver.FindElement(priceElement)).Text;
        }

        public bool IsSuccessMessageDisplayed()
        {
            try
            {
                var element = wait.Until(driver => driver.FindElement(successMessage));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetSuccessMessage()
        {
            return wait.Until(driver => driver.FindElement(successMessage)).Text;
        }

        public void SelectAllAvailableVariations()
        {
            // Reset variations trước để xóa default
            try
            {
                var resetLink = wait.Until(d => d.FindElement(resetVariations));
                if (resetLink.Displayed)
                {
                    resetLink.Click();
                    Console.WriteLine("✅ Đã reset variations");
                    Thread.Sleep(500);
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("⚠️ Không tìm thấy reset link, skip");
            }

            // Force select size đầu tiên
            try
            {
                wait.Until(d => d.FindElements(sizeOptions).Count > 0);
                var options = driver.FindElements(sizeOptions);

                if (options.Count > 0)
                {
                    var firstOption = options[1]; // Chọn L (index 0 là L? Từ HTML: L first, but default M is second; select index 0 for L
                    if (firstOption.Displayed && firstOption.Enabled)
                    {
                        firstOption.Click();
                        string dataTitle = firstOption.GetAttribute("data-title");
                        Console.WriteLine("✅ Force selected: " + dataTitle + " trong size");
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi select size: " + ex.Message);
            }
        }

        public void ClickViewCartInPopup()
        {
            try
            {
                var viewCartBtn = wait.Until(d => d.FindElement(viewCartButton));
                viewCartBtn.Click();
                Console.WriteLine("✅ Đã click 'Xem giỏ hàng'");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Không tìm thấy nút View Cart: " + ex.Message);
            }
        }

        public bool IsSuccessPopupDisplayed()
        {
            try
            {
                HandleUnexpectedAlertIfAny();

                var popup = wait.Until(d => d.FindElement(popupContainer));
                if (!popup.Displayed) return false;

                var viewCartBtn = wait.Until(d => d.FindElement(viewCartButton));
                if (viewCartBtn.Displayed)
                {
                    Console.WriteLine("✅ Popup hiện với 'View Cart'");
                    return true;
                }
                else
                {
                    Console.WriteLine("⚠️ 'View Cart' không hiển thị.");
                    return false;
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ Không tìm popup/'View Cart'.");
                return false;
            }
        }

        public void HandleUnexpectedAlertIfAny()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                Console.WriteLine("⚠️ Alert phát hiện: " + alert.Text);
                alert.Accept();
            }
            catch (NoAlertPresentException)
            {
                // Không có alert là OK
            }
        }

        public void GoToCartAfterAdd()
        {
            try
            {
                var viewCartBtn = wait.Until(d => d.FindElement(viewCartButton));
                if (viewCartBtn.Displayed)
                {
                    viewCartBtn.Click();
                    Console.WriteLine("✅ Đã click vào View Cart");
                }
                else
                {
                    throw new Exception("❌ Không tìm thấy nút View Cart trong thời gian chờ.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi click View Cart: " + ex.Message);
            }
        }

        public bool IsProductPageLoaded()
        {
            try
            {
                return wait.Until(driver => driver.FindElement(productTitle)).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}