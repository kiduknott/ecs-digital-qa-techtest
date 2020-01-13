using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace EcsDigitalQaTechTest
{
    public class QaTechTestPage
    {
        private static ChromeBrowser chromeBrowser;
        private const string customTechTestUrl = "http://localhost:3000/";
        private bool closeOpenBrowsers = false;
        private const string TITLE = "title";
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

        public QaTechTestPage()
        {
            chromeBrowser = new ChromeBrowser();
        }

        public void Launch()
        {
            chromeBrowser.NavigateTo(customTechTestUrl, closeOpenBrowsers);
        }

        public bool IsTitleDisplayed(string titleText)
        {
            var title = chromeBrowser.FindFirstElementByClassNameAndExactText(TITLE, titleText);
            return null != title;
        }

        public IWebElement GetRenderChallengeButton()
        {
            return chromeBrowser.FindElementByCssSelector(RENDER_CHALLENGE_BUTTON_SELECTOR);
        }

        public bool IsRenderChallengeButtonDisplayed()
        {
            return null != GetRenderChallengeButton();
        }

        public void Close()
        {
            chromeBrowser.Close();
        }

        public void Dispose()
        {
            chromeBrowser.Dispose();
        }

        
    }
}
