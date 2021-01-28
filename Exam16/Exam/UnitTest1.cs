using System;
using System.Threading;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Exam
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        // private ChromeOptions chromeOptions;

        [SetUp]
        public void Setup()
        {
            // For Sauce Labs
            // chromeOptions = new ChromeOptions();
            // driver = new RemoteWebDriver(new Uri(url), chromeOptions);

            // For localhost 
            driver = new ChromeDriver("D:\\ITSteps\\2020\\Automation Testing\\Exam16\\Exam\\chromedriver");
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }
        [Test]
        [Repeat(3)]
        public void Test()
        {
            driver.Navigate().GoToUrl("https://ctflearn.com/user/login");
            
            IWebElement username = driver.FindElement(By.CssSelector("#identifier"));
            username.SendKeys("alex.user.accs@gmail.com");

            IWebElement pasword = driver.FindElement(By.CssSelector("#password"));
            pasword.SendKeys("_Sanya765");

            IWebElement login_submit = driver.FindElement(By.CssSelector("body > div.container-fluid > div > div > div > div.card.bg-secondary > div.card-body > form > button"));
            login_submit.Click();

            driver.Navigate().GoToUrl("https://ctflearn.com/challenge/1/browse");

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            IWebElement searchField = driver.FindElement(By.CssSelector("#search"));
            searchField.SendKeys("Sco");
            Thread.Sleep(3000); // it`s not good but working at all
            searchField.SendKeys("pe");
            Thread.Sleep(1500);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(1.5);

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            wait.Until(d => d.FindElement(By.CssSelector("#challenge-list > div > div")));
            IWebElement challenge = driver.FindElement(By.CssSelector("#challenge-list > div > div"));
            challenge.Click();

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            IWebElement imageButton = driver.FindElement(By.CssSelector("#fileName"));
            imageButton.Click();

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            wait.Until(d => d.FindElement(By.CssSelector("#inlineFormInputGroup")));
            IWebElement field = driver.FindElement(By.CssSelector("#inlineFormInputGroup"));
            field.SendKeys("123");
            IWebElement submit = driver.FindElement(By.CssSelector(
                "body > div.container-fluid > div > div:nth-child(1) > div.col-lg-7.col-md-12.mb-4 > div > div > div.card-body.d-flex.flex-column.align-content-end > div.row.d-flex.flex-fill.align-items-end > div > div > div.col-3 > button"));
            submit.Click();

            wait.Until(d => d.FindElement(By.XPath("//strong[@class='mr-auto']")));
            IWebElement popup = driver.FindElement(By.XPath("//strong[@class='mr-auto']"));
            Assert.AreEqual(popup.GetAttribute("innerHTML"), "Incorrect flag. Hack harder!");

            driver.Close();
        }
    }
}