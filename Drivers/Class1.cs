using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecSimple.Drivers
{
    public class SeleniumDriver
    {
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        public SeleniumDriver(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public IWebDriver Setup()
        {
            var driver = new ChromeDriver();
            //driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), firefoxOption.ToCapabilities());
            _scenarioContext.Set(driver, "IWebDriver");
            //driver.Manage().Window.Maximize();
            return driver;
        }

    }
}
