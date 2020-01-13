using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace EcsDigitalQaTechTest
{
    [Binding]
    public class AutomatedUITestsSteps
    {
        private string applicationContainerName = "ecsd-tech-test:latest";
        private string customTechTestUrl = "http://localhost:3000/";
        private ChromeBrowser chromeBrowser = new ChromeBrowser();
        private bool closeOpenBrowsers = false;
        private const string TITLE = "title";

        [Given(@"custom-tech-test is running in Docker")]
        public async void GivenCustom_Tech_TestIsRunningInDocker()
        {
            var dockerProxy = new DockerProxy();
            var digitalTestContainerListResponses = dockerProxy.GetContainersByImageName(applicationContainerName);
            await dockerProxy.StartContainer(digitalTestContainerListResponses[0]);
        }
        
        [When(@"the custom-tech-test url is launched")]
        public void WhenTheCustom_Tech_TestUrlIsLaunched()
        {
            chromeBrowser.NavigateTo(customTechTestUrl, closeOpenBrowsers);
        }
        
        [Then(@"the custom-tech-test web page is displayed with the title (.*)")]
        public void ThenTheCustom_Tech_TestWebPageIsDisplayed(string titleText)
        {
            var title = chromeBrowser.FindFirstElementByClassNameAndExactText(TITLE, titleText);
            Assert.IsNotNull(title, "The custom tech web page was not displayed with text: {0}", titleText);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            chromeBrowser.Dispose();
        }
    }
}
