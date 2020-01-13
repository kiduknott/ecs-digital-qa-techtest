using System;
using TechTalk.SpecFlow;

namespace EcsDigitalQaTechTest
{
    [Binding]
    public class AutomatedUITestsSteps
    {
        [Given(@"custom-tech-test is running in Docker")]
        public void GivenCustom_Tech_TestIsRunningInDocker()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the custom-tech-test url is launched")]
        public void WhenTheCustom_Tech_TestUrlIsLaunched()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the custom-tech-test web page is displayed")]
        public void ThenTheCustom_Tech_TestWebPageIsDisplayed()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
