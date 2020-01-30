using System;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Akt.WebDriver.Browsers
{
    public class Chrome : IDisposable
    {
        private IWebDriver _driver;
        private const string CHROME = "chrome";

        public Chrome()
        {
            _driver = new ChromeDriver();
        }

        public void NavigateTo(string url, bool closeOpenBrowsers)
        {
            if (closeOpenBrowsers)
            {
                CloseAllChromeBrowsers();
            }

            _driver.Navigate().GoToUrl(url);
        }

        public IWebElement FindFirstElementByTagNameAndExactText(string tagName, string exactText)
        {
            IWebElement result = null;
            var allElementsByClassName = _driver.FindElements(By.TagName(tagName)).ToList();
            if (allElementsByClassName.Count != 0)
            {
                var filteredElements = allElementsByClassName.Where(b => b.Text.Equals(exactText)).ToList();
                if (filteredElements.Count != 0)
                {
                    result = filteredElements[0];
                }
            }

            return result;
        }

        public IWebElement FindFirstElementByTagNameAndContainsText(string tagName, string containsText)
        {
            IWebElement result = null;
            var allElementsByClassName = _driver.FindElements(By.TagName(tagName)).ToList();
            if (allElementsByClassName.Count != 0)
            {
                var filteredElements = allElementsByClassName.Where(b => b.Text.Contains(containsText)).ToList();
                if (filteredElements.Count != 0)
                {
                    result = filteredElements[0];
                }
            }

            return result;
        }

        public IWebElement FindFirstElementByClassNameAndExactText(string className, string exactText)
        {
            IWebElement result = null;
            var allElementsByClassName = _driver.FindElements(By.ClassName(className)).ToList();
            if (allElementsByClassName.Count != 0)
            {
                var filteredElements = allElementsByClassName.Where(b => b.Text.Equals(exactText)).ToList();
                if (filteredElements.Count != 0)
                {
                    result = filteredElements[0];
                }
            }

            return result;
        }

        public IWebElement FindFirstElementByClassNameAndContainsText(string className, string containsText)
        {
            IWebElement result = null;
            var allElementsByClassName = _driver.FindElements(By.ClassName(className)).ToList();
            if (allElementsByClassName.Count != 0)
            {
                var filteredElements = allElementsByClassName.Where(b => b.Text.Equals(containsText)).ToList();
                if (filteredElements.Count != 0)
                {
                    result = filteredElements[0];
                }
            }

            return result;
        }

        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            IWebElement result;
            try
            {
                result = _driver.FindElement(By.CssSelector(cssSelector));
            }
            catch (NoSuchElementException)
            {
                result = null;
            }

            return result;
        }

        public IWebElement FindElementByXPath(string xPath)
        {
            IWebElement result;
            try
            {
                result = _driver.FindElement(By.XPath(xPath));
            }
            catch (NoSuchElementException)
            {
                result = null;
            }

            return result;
        }

        public void SetSearchTimeout(int timeoutInMilliseconds)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(timeoutInMilliseconds);
        }

        public void SetSearchTimeoutToDefault()
        {
            SetSearchTimeout(0);
        }

        public static void CloseAllChromeBrowsers()
        {
            var chromeProcesses = Process.GetProcessesByName(CHROME);
            var numChrome = chromeProcesses.Length;
            foreach (var process in chromeProcesses)
            {
                process.Kill(true);
                process.WaitForExit();
                var hasExited = process.HasExited;
            }
        }

        public void Close()
        {
            _driver.Close();
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
