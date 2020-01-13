using System;
using System.Collections.Generic;
using System.Text;

namespace EcsDigitalQaTechTest
{
    public class QaTechTestPage
    {
        private static ChromeBrowser chromeBrowser;
        private const string customTechTestUrl = "http://localhost:3000/";
        private bool closeOpenBrowsers = false;
        private const string TITLE = "title";

        public QaTechTestPage()
        {
            chromeBrowser = new ChromeBrowser();
        }

        public void Launch()
        {
            chromeBrowser.NavigateTo(customTechTestUrl, closeOpenBrowsers);
        }

        public bool? IsTitleDisplayed(string titleText)
        {
            var title = chromeBrowser.FindFirstElementByClassNameAndExactText(TITLE, titleText);
            return null != title ? true : false;
        }

        public void Close()
        {
            chromeBrowser.Dispose();
        }
    }
}
