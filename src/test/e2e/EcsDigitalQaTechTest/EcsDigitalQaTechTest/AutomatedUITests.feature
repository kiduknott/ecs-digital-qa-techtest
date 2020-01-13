Feature: AutomatedUITests
In order to have a correctly functioning application
AS A product team
We want the application to conform to the business rules

Scenario Outline: The custom-tech-test web page is displayed when the url is launched
Given custom-tech-test is running in Docker
When the custom-tech-test url is launched
Then the custom-tech-test web page is displayed