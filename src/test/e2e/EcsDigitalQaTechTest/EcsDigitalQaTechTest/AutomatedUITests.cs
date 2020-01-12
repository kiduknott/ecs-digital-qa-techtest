using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using OpenQA.Selenium.Interactions;

namespace EcsDigitalQaTechTest
{
    public class AutomatedUITests : IDisposable
    {
        private IWebDriver _driver;
        private DockerClient dockerClient;
        const string ARRAY_TABLE_XPATH = "//*[@id=\"challenge\"]/div/div/div[1]/div/div[2]/table";
        const string TBODY = "tbody";
        const string TR = "tr";
        const string TD = "td";
        const string RENDER_CHALLENGE_BUTTON_SELECTOR = "button[data-test-id=\"render-challenge\"]";
        const string ARRAY_TABLE_CELL_SELECTOR_PREFIX = "td[data-test-id=\"array-item-";
        const string SUBMIT_RESULT_TEXTBOX_PREFIX = "input[data-test-id=\"submit-";
        const string AUTHOR = "Amatey Teye";
        const string BUTTON = "button";
        const string errorMessageText = "It looks like your answer wasn't quite right";
        const string successMessageText = "Congratulations you have succeeded.Please submit your challenge";
        const string DIV = "div";
        const string SUBMIT_ANSWERS = "SUBMIT ANSWERS";
        const string NULL = "null";
        const string CLOSE = "CLOSE";
        const string CHROME = "chrome";

        [SetUp]
        public async Task Setup()
        {
            //Start and stop application: https://github.com/microsoft/Docker.DotNet
            dockerClient = CreateDockerClient();
            var digitalTestContainerListResponses = await GetContainersByImageName(dockerClient, "ecsd-tech-test:latest");
            await StartContainer(dockerClient, digitalTestContainerListResponses[0]);
            //CloseAllChromeBrowsers();
            _driver = new ChromeDriver();
        }

        private DockerClient CreateDockerClient()
        {
            //no idea how to connect - used the example and it worked
            return new DockerClientConfiguration(
                    new Uri("npipe://./pipe/docker_engine"))
                .CreateClient();
        }

        [TearDown]
        public async Task TearDown()
        {
            var digitalTestContainerListResponses = await GetContainersByImageName(dockerClient, "ecsd-tech-test:latest");
            foreach (var response in digitalTestContainerListResponses)
            {
                await StopContainer(dockerClient, response);
            }
        }

        private static async Task<List<ContainerListResponse>> GetContainersByImageName(DockerClient dockerClient, string imageName)
        {
            var containers = await dockerClient.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    Limit = 10,
                });
            var digitalTestContainers =
                containers.Where(c => c.Image.Equals(imageName)).ToList();
            return digitalTestContainers;
        }

        private static async Task StartContainer(DockerClient dockerClient, ContainerListResponse containerListResponse)
        {
            await dockerClient.Containers.StartContainerAsync(containerListResponse.ID,
                new ContainerStartParameters());
        }

        private static async Task StopContainer(DockerClient dockerClient, ContainerListResponse containerListResponse)
        {
            await dockerClient.Containers.StopContainerAsync(containerListResponse.ID,
                new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 30
                },
                CancellationToken.None);
        }

        private void CloseAllChromeBrowsers()
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

        [Test]
        public void ECSDigitalWebSiteOpensWhenLaunchedViaUrl()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000/");

            var renderChallengeButton = _driver.FindElement(By.CssSelector(RENDER_CHALLENGE_BUTTON_SELECTOR));
            renderChallengeButton.Click();

            Thread.Sleep(3000);

            var arraysTable = _driver.FindElement(By.XPath(ARRAY_TABLE_XPATH));
            var body = arraysTable.FindElements(By.TagName(TBODY)).ToList()[0];

            var arraysTableRows = body.FindElements(By.TagName(TR)).ToList();
            var numRows = arraysTableRows.Count;

            var tableValues = new Dictionary<int, List<int>>();

            for (var rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                var row = arraysTableRows[rowIndex];
                var cells = row.FindElements(By.TagName(TD));
                var numCells = cells.Count;

                var rowValues = new List<int>();
                for (var cellIndex = 0; cellIndex < numCells; cellIndex++)
                {
                    var cellSelector = ARRAY_TABLE_CELL_SELECTOR_PREFIX + (rowIndex + 1) + "-" + cellIndex + "\"]";
                    var cell = _driver.FindElement(By.CssSelector(cellSelector));
                    rowValues.Add(int.Parse(cell.Text));
                }

                tableValues.Add(rowIndex, rowValues);
            }

            var results = new List<string>();
            foreach (var entry in tableValues)
            {
                var rowValues = entry.Value;

                var answer = NULL;
                var leftIndex = 0;
                var rightIndex = rowValues.Count - 1;

                var leftSum = rowValues[leftIndex];
                var rightSum = rowValues[rightIndex];
                do
                {
                    if (leftSum > rightSum)
                    {
                        rightIndex--;
                        rightSum += rowValues[rightIndex];
                    }
                    else if (leftSum < rightSum)
                    {
                        leftIndex++;
                        leftSum += rowValues[leftIndex];
                    }
                    else
                    {
                        answer = (leftIndex + 1).ToString();
                        break;
                        //answer = (leftIndex + 1).ToString();
                    }
                } while (leftIndex != rightIndex);

                results.Add(answer);
            }

            for (var resultIndex = 0; resultIndex < results.Count; resultIndex++)
            {
                var submitResultTextboxSelector = SUBMIT_RESULT_TEXTBOX_PREFIX + (resultIndex + 1) + "\"]";
                var submitTextBox = _driver.FindElement(By.CssSelector(submitResultTextboxSelector));
                submitTextBox.SendKeys(results[resultIndex]);
            }

            var nameTextboxSelector = SUBMIT_RESULT_TEXTBOX_PREFIX + (results.Count + 1) + "\"]";
            var nameTextBox = _driver.FindElement(By.CssSelector(nameTextboxSelector));
            nameTextBox.SendKeys(AUTHOR);

            var buttons = _driver.FindElements(By.TagName(BUTTON)).ToList();

            var submitButton = buttons.Where(b => b.Text.Equals(SUBMIT_ANSWERS)).ToList()[0];

            var actions = new Actions(_driver);
            actions.MoveToElement(submitButton);
            actions.Perform();

            submitButton.Click();

            Thread.Sleep(5000);

            var isErrorMessagePresent = IsElementPresent(_driver, DIV, errorMessageText);
            var isSuccessMessagePresent = IsElementPresent(_driver, DIV, successMessageText);
            var isCloseButtonPresent = IsElementPresent(_driver, BUTTON, CLOSE);
            if (isCloseButtonPresent)
            {
                var buttonsAfterSubmit = _driver.FindElements(By.TagName(BUTTON)).ToList();
                var closeButton = buttonsAfterSubmit.Where(b => b.Text.Equals(CLOSE)).ToList()[0];
                closeButton.Click();
            }

            Thread.Sleep(2000);
            //Assert.Pass();
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        public bool IsElementPresent(IWebDriver driver, By locator)
        {
            //Set the timeout to something low
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);

            try
            {
                driver.FindElement(locator);
                //If element is found set the timeout back and return true
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(20000);
                return true;
            }
            catch (NoSuchElementException)
            {
                //If element isn't found, set the timeout and return false
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(20000);
                return false;
            }
        }

        public bool IsElementPresent(IWebDriver driver, string tagName, string text)
        {
            //Set the timeout to something low
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);

            try
            {
                var elements = _driver.FindElements(By.TagName(tagName)).ToList();
                var element = elements.Where(b => b.Text.Contains(text)).ToList()[0];
                //If element is found set the timeout back and return true
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(20000);
                return true;
            }
            catch (NoSuchElementException)
            {
                //If element isn't found, set the timeout and return false
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(20000);
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                //If element isn't found, set the timeout and return false
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(20000);
                return false;
            }
        }
    }
}
