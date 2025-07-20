using OpenQA.Selenium;

namespace TheBluesAutomation.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IWebElement LoginButton => driver.FindElement(By.CssSelector("button.js-rey-headerAccount"));
        private IWebElement EmailField => driver.FindElement(By.Id("username"));
        private IWebElement PasswordField => driver.FindElement(By.Id("password"));
        private IWebElement SubmitButton => driver.FindElement(By.CssSelector("button.btn-line-active.submit-btn"));

        public void Login(string email, string password)
        {
            LoginButton.Click();
            EmailField.SendKeys(email);
            PasswordField.SendKeys(password);
            SubmitButton.Click();
        }
    }
}
