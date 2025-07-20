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

        public ProductPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public string GetProductTitle()
        {
            return wait.Until(driver => driver.FindElement(productTitle)).Text;
        }

        public void SetQuantity(int quantity)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var quantityInput = wait.Until(d => d.FindElement(By.CssSelector("input[name='quantity']")));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = arguments[1];", quantityInput, quantity.ToString());

            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", quantityInput);
        }

        public void ClickAddToCart()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var button = wait.Until(d => d.FindElement(By.CssSelector("button.single_add_to_cart_button")));

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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                // Đợi đến khi các button size hiển thị
                wait.Until(d => d.FindElement(By.CssSelector("ul.variable-items-wrapper li.variable-item")));

                var sizeOptions = driver.FindElements(By.CssSelector("ul.variable-items-wrapper li.variable-item"));

                if (sizeOptions.Count > 0)
                {
                    // Click vào lựa chọn size đầu tiên chưa được chọn
                    foreach (var option in sizeOptions)
                    {
                        string ariaChecked = option.GetAttribute("aria-checked");
                        if (ariaChecked != "true")
                        {
                            option.Click();
                            Console.WriteLine("✅ Đã chọn size: " + option.GetAttribute("data-title"));
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("❌ Không tìm thấy nút chọn size");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi chọn size: " + ex.Message);
            }
        }

        public void ClickViewCartInPopup()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var viewCartBtn = wait.Until(d => d.FindElement(By.CssSelector("a.button.wc-forward")));
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
                HandleUnexpectedAlertIfAny(); // Xử lý alert nếu có

                WebDriverWait wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(10));
                var viewCartButton = wait.Until(d =>
                {
                    try
                    {
                        var element = d.FindElement(By.CssSelector(".rey-acPopup-buttons a.rey-acPopup-buttons-cart"));
                        return element.Displayed ? element : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });

                if (viewCartButton != null && viewCartButton.Displayed)
                {
                    Console.WriteLine("✅ Popup hiện ra với nút 'View Cart'");
                    return true;
                }
                else
                {
                    Console.WriteLine("⚠️ Nút 'View Cart' không hiển thị.");
                    return false;
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ Không tìm thấy nút 'View Cart' trong thời gian chờ.");
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
                // Không có alert nào là OK
            }
        }

        public void GoToCartAfterAdd()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement viewCartBtn = wait.Until(driver =>
                {
                    var btn = driver.FindElement(By.CssSelector(".rey-acPopup-buttons a.rey-acPopup-buttons-cart"));
                    return (btn.Displayed && btn.Enabled) ? btn : null;
                });

                viewCartBtn.Click();
                Console.WriteLine("✅ Đã click vào View Cart");
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