using System;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace AndroidTest
{
    class LoginPage
    {
        public AppiumDriver<AndroidElement> driver;
        public LoginPage(AppiumDriver<AndroidElement> driver) => this.driver = driver;
        public AndroidElement UsernameInput { get; set; }
        public AndroidElement PasswordInput { get; set; }
        public AndroidElement Button { get; set; }

    }

    [TestFixture]
    public class Tests
    {
        private static Uri testServerAddress = new Uri("http://localhost:4723/wd/hub");
        private static TimeSpan INIT_TIMEOUT_SEC = TimeSpan.FromSeconds(180);


        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions caps;
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            caps = new AppiumOptions();
            caps.AddAdditionalCapability("browserstack.user", Environment.GetEnvironmentVariable("stack.user"));
            caps.AddAdditionalCapability("browserstack.key", Environment.GetEnvironmentVariable("stack.key"));
            caps.AddAdditionalCapability("app", "bs://4efa1cf1d71c4a43d09d9054356b897d5109b98e");
            caps.AddAdditionalCapability("device", "Google Pixel 3");ut
            caps.AddAdditionalCapability("os_version", "10.0");
            caps.PlatformName = "Android";
            caps.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "com.example.hnenn");
            caps.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, "com.example.hnenn.ui.login.LoginActivity");
            caps.AddAdditionalCapability(MobileCapabilityType.NoReset, true);
            caps.AddAdditionalCapability("project", "First CSharp project");
            caps.AddAdditionalCapability("build", "CSharp Android");
            caps.AddAdditionalCapability("name", "first_test");
            AndroidDriver<AndroidElement> driver = new AndroidDriver<AndroidElement>(
                new Uri("http://hub-cloud.browserstack.com/wd/hub"), caps);
            loginPage = new LoginPage(driver)
            {
                UsernameInput = driver.FindElementById("com.example.practice8:id/username"),
                PasswordInput = driver.FindElementById("com.example.practice8:id/password"),
                Button = driver.FindElementById("com.example.practice8:id/login")
            };
        }

        [TestCase("NONE", "zaqw", TestName = "non valid")]
        [TestCase("", "zaqwer", TestName = "non valid name ")]
        [TestCase("g", "123456", TestName = "valid data")]
        public void MyTest(string username, string password)
        {

            loginPage.UsernameInput.SendKeys(username);
            loginPage.PasswordInput.SendKeys(password);

            Assert.True(loginPage.Button.Enabled);
        }

    }
}
