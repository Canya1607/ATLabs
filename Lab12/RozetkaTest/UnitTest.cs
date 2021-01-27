using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using System;
using System.Linq;

namespace RozetkaTest
{
    public class UnitTest
    {
        private int reps = 1;
        MyBrowser browser;

        class MyBrowser
        {
            private ChromeOptions chromeOptions;
            private string url =
                "https://Alexcon4ik:82c260c12c9c437995429d8a439838e0@ondemand.eu-central-1.saucelabs.com:443/wd/hub";

            public IWebDriver driver { get; private set; }
            public MyBrowser()
            {
                chromeOptions = new ChromeOptions();
                driver = new RemoteWebDriver(new Uri(url), chromeOptions);
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

        [Test, Repeat(3)]
        public void PriceSearchProductPageCompare()
        {
            browser.goTo("https://rozetka.com.ua/");

            var d = browser.driver;
            WebDriverWait wait = new WebDriverWait(d, new TimeSpan(0, 1, 0));

            IWebElement searchInput = d.FindElement(By.ClassName("search-form__input"));
            searchInput.SendKeys("dell latitude 5290");
            wait.Until(nd => nd.FindElements(By.ClassName("suggest-goods__price")).Count > 0);

            IWebElement firstElementOfSearch = d.FindElements(By.ClassName("suggest-goods__price"))[0];

            Func<string, string> parsePrice = (string p) => string.Join("", p.Where(char.IsDigit).ToArray());
            double searchPrice = double.Parse(parsePrice(firstElementOfSearch.Text));

            firstElementOfSearch.Click();
            wait.Until(nd => nd.FindElement(By.ClassName("product-prices__big")));

            IWebElement primaryElement = d.FindElement(By.ClassName("product-prices__big"));
            double primaryPrice = double.Parse(parsePrice(primaryElement.Text));

            Assert.AreEqual(primaryPrice, searchPrice);

            d.Navigate().Back();
            wait.Until(nd => nd.FindElement(By.ClassName("search-form__input")));

            searchInput = d.FindElement(By.ClassName("search-form__input"));
            searchInput.SendKeys("dell latitude 5290");
            wait.Until(nd => nd.FindElements(By.ClassName("suggest-goods__price")).Count > 0);
            IWebElement newFirstElementOfSearch = d.FindElements(By.ClassName("suggest-goods__price"))[0];
            double newSearchPrice = double.Parse(parsePrice(newFirstElementOfSearch.Text));

            Console.WriteLine($"This is repeat #{reps++}");
            Console.WriteLine($"First search price: {searchPrice}");
            Console.WriteLine($"product page price: {primaryPrice}");
            Console.WriteLine($"Second search price: {newSearchPrice}\n");
            Assert.AreEqual(newSearchPrice, searchPrice, primaryPrice);
        }
    }
}