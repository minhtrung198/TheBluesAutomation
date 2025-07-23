using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TheBluesAutomation.Pages
{
    public class CartPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // Locators
        private readonly By cartItems = By.CssSelector(".product-quantity .quantity");
        private readonly By itemTitle = By.CssSelector(".woocommerce-mini-cart-item-title");
        private readonly By itemPrice = By.CssSelector(".product-price .woocommerce-Price-amount");
        private readonly By itemQuantity = By.CssSelector(".product-quantity .quantity .input-text");
        private readonly By totalPrice = By.CssSelector(".product-subtotal");
        private readonly By checkoutButton = By.CssSelector(".wc-proceed-to-checkout .checkout-button");
        private readonly By emptyCartMessage = By.CssSelector(".empty-cart");
        private readonly By removeButton = By.CssSelector(".remove-item");

        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public int GetCartItemCount()
        {
            try
            {
                return driver.FindElements(cartItems).Count;
            }
            catch
            {
                return 0;
            }
        }

        public string GetFirstItemTitle()
        {
            return wait.Until(driver => driver.FindElement(itemTitle)).Text;
        }

        public string GetFirstItemPrice()
        {
            return wait.Until(driver => driver.FindElement(itemPrice)).Text;
        }

        public int GetFirstItemQuantity()
        {
            var quantityText = wait.Until(driver => driver.FindElement(itemQuantity)).Text;
            return int.Parse(quantityText);
        }

        public string GetTotalPrice()
        {
            return wait.Until(driver => driver.FindElement(totalPrice)).Text;
        }

        public bool IsEmptyCartMessageDisplayed()
        {
            try
            {
                var element = wait.Until(driver => driver.FindElement(emptyCartMessage));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void ClickCheckoutButton()
        {
            try
            {
                var checkoutBtn = wait.Until(d => d.FindElement(checkoutButton));
                checkoutBtn.Click();
                Console.WriteLine("✅ Đã click 'Tiến hành thanh toán'");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Không tìm thấy nút Checkout: " + ex.Message);
            }
        }

        public void RemoveFirstItem()
        {
            var removeBtn = wait.Until(driver => driver.FindElement(removeButton));
            removeBtn.Click();
        }

        public bool IsCartPageLoaded()
        {
            try
            {
                return wait.Until(driver => driver.FindElement(cartItems)).Displayed ||
                       wait.Until(driver => driver.FindElement(emptyCartMessage)).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}