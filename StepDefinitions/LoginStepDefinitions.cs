using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using SpecSimple.Drivers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TechTalk.SpecFlow;

namespace SpecSimple.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        IWebDriver driver;
        private static ExtentReports extent;
        private static ExtentTest feature;
        private static ExtentTest scenario;
        private static ScenarioContext _scenarioContext;
        private static SeleniumDriver seleniumDriver;

        private static string reportPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Reports");
        private static string reportFile = Path.Combine(reportPath, "TestReport"+ DateTime.Now.ToString("yyyy-mm-dd_HH-mm-ss") +".html");

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            TestContext.Progress.WriteLine("Before Scenario");
            scenario = feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            _scenarioContext = scenarioContext;
           
            _scenarioContext.Set(new SeleniumDriver(_scenarioContext), "SeleniumDriver");
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            TestContext.Progress.WriteLine("Before Feature");
            feature = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [Given(@"I have navigated to the login page")]
        public void GivenIHaveNavigatedToTheLoginPage()
        {
            //throw new PendingStepException();
            TestContext.Progress.WriteLine("GivenIHaveNavigatedToTheLoginPage");

            driver = _scenarioContext.Get<SeleniumDriver>("SeleniumDriver").Setup();
            driver.Url = "https://www.saucedemo.com/";
        }

        [When(@"I enter correct ""([^""]*)"" and ""([^""]*)""")]
        public void WhenIEnterCorrectAnd(string username, string password)
        {
            //throw new PendingStepException();
            TestContext.Progress.WriteLine("username is - " + username + " and password is - " + password);

            driver.FindElement(By.Id("user-name")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.Id("login-button")).Submit();

        }


        [Then(@"I should be inventory page")]
        public void ThenIShouldBeInventoryPage()
        {
            //throw new PendingStepException();
            TestContext.Progress.WriteLine("I should be able to see inventory page! ");
            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/inventory.html"), "Login unsuccessful, inventory page not displayed!");
        }


        [AfterScenario]
        public static void AfterScenario(ScenarioContext scenarioContext)
        {
            TestContext.Progress.WriteLine("After Scenario - Quit Selenium WebDriver");
            scenarioContext.Get<IWebDriver>("IWebDriver").Quit();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            TestContext.Progress.WriteLine("After Feature");
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            if(!Directory.Exists(reportFile))
            {
                Directory.CreateDirectory(reportPath);
            }
            var htmlReporter = new ExtentSparkReporter(reportFile);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void FlushReport()
        {
            extent.Flush();
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            var stepInfo = ScenarioStepContext.Current.StepInfo.Text;
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepInfo);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepInfo);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepInfo);
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepInfo);

            }
            else
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepInfo).Fail(scenarioContext.TestError.Message);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepInfo).Fail(scenarioContext.TestError.Message);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepInfo).Fail(scenarioContext.TestError.Message);
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepInfo).Fail(scenarioContext.TestError.Message);

            }

        }
    }
}
