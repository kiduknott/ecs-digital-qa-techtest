using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace EcsDigitalQaTechTest
{
    [Binding]
    public class AutomatedUITestsSteps
    {
        private string techTestContainerName = "ecsd-tech-test:latest";
        private QaTechTestPage qaTechTestPage = new QaTechTestPage();

        [Given(@"custom-tech-test is running in Docker")]
        public async void GivenCustom_Tech_TestIsRunningInDocker()
        {
            var dockerProxy = new DockerProxy();
            await dockerProxy.StartContainer(techTestContainerName);
        }

        [Given(@"the custom-tech-test url is launched")]
        public void WhenTheCustom_Tech_TestUrlIsLaunched()
        {
            qaTechTestPage.Launch();
        }

        [When(@"the RenderChallenge button is clicked")]
        public void WhenTheRenderChallengeButtonIsClicked()
        {
            qaTechTestPage.ClickRenderChallengeButton();
        }

        [Then(@"the custom-tech-test web page is displayed with the title (.*)")]
        public void ThenTheCustom_Tech_TestWebPageIsDisplayed(string titleText)
        {
            var isTitleDisplayed = qaTechTestPage.IsTitleDisplayed(titleText);
            Assert.True(isTitleDisplayed, "The custom tech web page was not displayed with text: {0}", titleText);
        }

        [Then(@"the RenderChallenge button is displayed")]
        public void ThenTheRenderChallengeButtonIsDisplayed()
        {
            var isRenderChallengeButtonDisplayed = qaTechTestPage.IsRenderChallengeButtonDisplayed();
            Assert.True(isRenderChallengeButtonDisplayed, "The RenderChallenge button was not displayed");
        }

        [When(@"the correct answers are submitted by (.*)")]
        public void WhenTheCorrectAnswersAreSubmittedBy(string submitter)
        {
            var correctAnswers = qaTechTestPage.CalculateCorrectAnswers();
            qaTechTestPage.SetFirstSubmitTextBox(correctAnswers[0]);
            qaTechTestPage.SetSecondSubmitTextBox(correctAnswers[1]);
            qaTechTestPage.SetThirdSubmitTextBox(correctAnswers[2]);
            qaTechTestPage.SetFourthSubmitTextBox(submitter);
            qaTechTestPage.ClickSubmitButton();
        }


        [Then(@"the ArrayChallenge table is displayed")]
        public void ThenTheArrayChallengeTableIsDisplayed()
        {
            var isArrayChallengeTableDisplayed = qaTechTestPage.IsArrayChallengeTableDisplayed();
            Assert.True(isArrayChallengeTableDisplayed, "The ArrayChallenge table was not displayed");
        }

        [Then(@"the success text (.*) is displayed")]
        public void ThenTheSuccessTextIsDisplayed(string successText)
        {
            var isSuccessMessageDisplayed = qaTechTestPage.IsMessagePresent(successText);
            Assert.True(isSuccessMessageDisplayed, "The Success Message was not displayed");
        }


        [AfterTestRun]
        public static void AfterTestRun()
        {
            ChromeBrowser.CloseAllChromeBrowsers();
        }
    }
}
