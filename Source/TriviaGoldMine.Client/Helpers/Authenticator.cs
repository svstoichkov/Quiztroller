namespace Quiztroller.Helpers
{
    using System.Threading.Tasks;

    using OpenQA.Selenium;
    using OpenQA.Selenium.PhantomJS;

    public static class Authenticator
    {
        public static Task<string> Authenticate(string email, string password)
        {
            return Task.Run(() =>
            {
                var driverService = PhantomJSDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;

                var browser = new PhantomJSDriver(driverService);
                browser.Navigate().GoToUrl("http://www.triviagoldmine2.com/wp-login.php");

                browser.FindElement(By.Id("user_login")).SendKeys(email);
                browser.FindElement(By.Id("user_pass")).SendKeys(password);
                browser.FindElement(By.Id("wp-submit")).Click();

                browser.Navigate().GoToUrl($"http://www.triviagoldmine2.com/?username={email}&password={password}");
                var source = browser.PageSource;
                var response = source.Substring(25, source.Length - 14 - 25);

                return response;
            });
        }
    }
}
