using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Akt.Selenium.Browsers;
using OpenQA.Selenium;

namespace EcsDigitalQaTechTest
{
    public class QaTechTestPage
    {
        private static Chrome chromeBrowser;
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
        const string BUTTON = "button";
        const string DIV = "div";
        const string SUBMIT_ANSWERS = "SUBMIT ANSWERS";
        const string NULL = "null";
        private const int INCREASED_SEARCH_TIMEOUT = 30000;

        public QaTechTestPage()
        {
            chromeBrowser = new Chrome();
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

        public void ClickRenderChallengeButton()
        {
            GetRenderChallengeButton().Click();
        }

        public bool? IsArrayChallengeTableDisplayed()
        {
            chromeBrowser.SetSearchTimeout(INCREASED_SEARCH_TIMEOUT);
            var arrayChallengeTable = chromeBrowser.FindElementByXPath(ARRAY_TABLE_XPATH);
            chromeBrowser.SetSearchTimeoutToDefault();
            return null != arrayChallengeTable;
        }

        public List<string> CalculateCorrectAnswers()
        {
            var arraysTable = chromeBrowser.FindElementByXPath(ARRAY_TABLE_XPATH);
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
                    var cell = chromeBrowser.FindElementByCssSelector(cellSelector);
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

            return results;
        }

        public void SetFirstSubmitTextBox(string text)
        {
            var resultIndex = 0;
            SetSubmitTextBoxByIndex(resultIndex, text);
        }

        public void SetSecondSubmitTextBox(string text)
        {
            var resultIndex = 1;
            SetSubmitTextBoxByIndex(resultIndex, text);
        }

        public void SetThirdSubmitTextBox(string text)
        {
            var resultIndex = 2;
            SetSubmitTextBoxByIndex(resultIndex, text);
        }

        public void SetFourthSubmitTextBox(string text)
        {
            var resultIndex = 3;
            SetSubmitTextBoxByIndex(resultIndex, text);
        }

        private static void SetSubmitTextBoxByIndex(int resultIndex, string answer)
        {
            var textBoxSelector = SUBMIT_RESULT_TEXTBOX_PREFIX + (resultIndex + 1) + "\"]";
            var submitTextBox = chromeBrowser.FindElementByCssSelector(textBoxSelector);
            submitTextBox.SendKeys(answer);
        }

        public void ClickSubmitButton()
        {
            var submitButton = chromeBrowser.FindFirstElementByTagNameAndContainsText(BUTTON, SUBMIT_ANSWERS);
            submitButton.Click();
        }

        public bool IsMessagePresent(string messageText)
        {
            chromeBrowser.SetSearchTimeout(INCREASED_SEARCH_TIMEOUT);
            Thread.Sleep(1000); //Should have used WebDriverWait
            var message = chromeBrowser.FindFirstElementByTagNameAndContainsText(DIV, messageText);
            chromeBrowser.SetSearchTimeoutToDefault();
            return null != message;
        }
    }
}
