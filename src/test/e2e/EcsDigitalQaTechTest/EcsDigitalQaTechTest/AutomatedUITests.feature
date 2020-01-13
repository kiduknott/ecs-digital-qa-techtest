Feature: AutomatedUITests
In order to have a correctly functioning application
AS A product team
We want the application to conform to the business rules

Scenario Outline: The custom-tech-test web page is displayed when the url is launched
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
Then the custom-tech-test web page is displayed with the title <TitleText>
Examples: 
| TitleText                                            |
| Welcome to the ECSDigital Engineer in Test tech test |

Scenario: The Render Challenge button is displayed when the url is launched
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
Then the RenderChallenge button is displayed

Scenario Outline: Clicking the RenderChallenge button displays the ArrayChallenge table
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
When the RenderChallenge button is clicked
Then the ArrayChallenge table is displayed