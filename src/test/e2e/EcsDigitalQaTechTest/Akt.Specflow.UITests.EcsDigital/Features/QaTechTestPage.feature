Feature: ECS QA Tech Tests
In order to have a correctly functioning Custom Tech Test application
AS A product team
We want the application to conform to the stated business rules

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

Scenario: Clicking the RenderChallenge button displays the ArrayChallenge table
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
When the RenderChallenge button is clicked
Then the ArrayChallenge table is displayed

Scenario Outline: The success message is displayed when the correct answers are submitted
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
When the RenderChallenge button is clicked
And the correct answers are submitted by <submitter>
Then the success text <successMessage> is displayed
Examples: 
| successMessage                                                   | submitter   |
| Congratulations you have succeeded. Please submit your challenge | Amatey Teye |

Scenario Outline: The failure message is not displayed when the correct answers are submitted
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
When the RenderChallenge button is clicked
And the correct answers are submitted by <submitter>
Then the failure text <failureMessage> is not displayed
Examples: 
| failureMessage            | submitter   |
| It looks like your answer | Amatey Teye |

Scenario Outline: The failure message is displayed when an correct answer is submitted
Given custom-tech-test is running in Docker
And the custom-tech-test url is launched
When the RenderChallenge button is clicked
And the answers with incorrect position <incorrectPosition> is submitted by <submitter>
Then the failure text <failureMessage> is displayed
Examples: 
| failureMessage                               | incorrectPosition | submitter   |
| It looks like your answer wasn't quite right | 1                 | Amatey Teye |
| It looks like your answer wasn't quite right | 2                 | Amatey Teye |
| It looks like your answer wasn't quite right | 3                 | Amatey Teye |