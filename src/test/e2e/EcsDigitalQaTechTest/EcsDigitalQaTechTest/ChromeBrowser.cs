using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EcsDigitalQaTechTest
{
    public class ChromeBrowser : IDisposable
    {
        private IWebDriver _driver;
        private const string CHROME = "chrome";

        public ChromeBrowser()
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
                var filteredElements = allElementsByClassName.Where(b => b.Text.Equals(containsText)).ToList();
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
