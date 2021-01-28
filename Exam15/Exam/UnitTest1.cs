using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

using Bogus;

namespace Exam
{
    public class Tests
    {
        MyBrowser browser;
        [SetUp]
        public void Setup()
        {
        }

        class MyBrowser
        {
            public IWebDriver driver { get; private set; }
            public MyBrowser()
            {
                driver = new ChromeDriver("D:\\ITSteps\\2020\\Automation Testing\\Exam15\\Exam\\chromedriver");
                driver.Manage().Window.Maximize();
            }
            public void close()
            {
                driver.Close();
            }
            public void goTo(string url)
            {
                driver.Url = url;
            }

            public string getCurrentUrl()
            {
                return driver.Url;
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            browser = new MyBrowser();
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            browser.close();
        }

        [Test]
        public void Login()
        {
            var faker = new Faker();

            browser.goTo("https://ctflearn.com");

            var d = browser.driver;
            WebDriverWait wait = new WebDriverWait(d, new TimeSpan(0, 0, 16));

            string usernameValue = $"{faker.Name.FirstName()}{faker.Finance.Account(5)}";
            string emailValue = faker.Internet.Email();
            string passValue = faker.Internet.Password();

            wait.Until(nd => nd.FindElement(By.LinkText("Join Now")));
            IWebElement joinNow = d.FindElement(By.LinkText("Join Now"));
            joinNow.Click();

            wait.Until(nd => nd.FindElements(By.Id("username")));
            IWebElement usernameInput = d.FindElement(By.Id("username"));
            usernameInput.SendKeys(usernameValue);

            wait.Until(nd => nd.FindElements(By.Id("email")));
            IWebElement emailInput = d.FindElement(By.Id("email"));
            emailInput.SendKeys(emailValue);

            wait.Until(nd => nd.FindElements(By.Id("password")));
            IWebElement passwordInput = d.FindElement(By.Id("password"));
            passwordInput.SendKeys(passValue);

            wait.Until(nd => nd.FindElements(By.Id("confirm")));
            IWebElement confirmInput = d.FindElement(By.Id("confirm"));
            confirmInput.SendKeys(passValue);
            
            IWebElement button = d.FindElement(By.XPath("/html/body/div/div/div/div/div[1]/div[2]/form/button"));
            button.Click();
            wait.Until(nd => nd.Navigate());

            Assert.AreEqual(browser.getCurrentUrl(), "https://ctflearn.com/dashboard");

            browser.goTo("https://ctflearn.com/user/logout");
            wait.Until(nd => nd.Navigate());

            browser.goTo("https://ctflearn.com/user/login");
            wait.Until(nd => nd.Navigate());

            wait.Until(nd => nd.FindElement(By.Id("identifier")));
            IWebElement identifier = d.FindElement(By.Id("identifier"));
            identifier.SendKeys(emailValue);

            wait.Until(nd => nd.FindElement(By.Id("password")));
            IWebElement passwordLogin = d.FindElement(By.Id("password"));
            passwordLogin.SendKeys(passValue);

            wait.Until(nd => nd.FindElement(By.XPath("/html/body/div/div/div/div/div[1]/div[2]/form/button")));
            IWebElement buttonLogin = d.FindElement(By.XPath("/html/body/div/div/div/div/div[1]/div[2]/form/button"));
            buttonLogin.Click();
            wait.Until(nd => nd.Navigate());

            Assert.AreEqual(browser.getCurrentUrl(), "https://ctflearn.com/dashboard");
        }
    }
}