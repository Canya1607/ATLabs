using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using System;
using System.Drawing;
using System.IO;

namespace AndroidTest
{
    public class SignInPageTests
    {
        Uri serverAddress = new Uri("http://localhost:4723/wd/hub");
        TimeSpan timeout = TimeSpan.FromSeconds(180);

        AppiumOptions driverOptions = new AppiumOptions();
        AppiumDriver<AndroidElement> driver;

        AndroidElement emailInput;
        AndroidElement passInput;
        AndroidElement signButton;

        [SetUp]
        public void Setup()
        {
            driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.App, @"D:\ITSteps\2020\Automation Testing\Lab8\AndroidTest\app-debug.apk");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Redmi");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.Udid, "49a9f241");

            driver = new AndroidDriver<AndroidElement>(serverAddress, driverOptions, timeout);
            emailInput = driver.FindElementById("com.example.myapplication:id/username");
            passInput = driver.FindElementById("com.example.myapplication:id/password");
            signButton = driver.FindElementById("com.example.myapplication:id/login");
        }

        [Test, Order(1)]
        public void TestCorrectEmailWrongPass()
        {
            emailInput.SendKeys("test@gmail.com");
            passInput.SendKeys("1234");
            signButton.Click();
            var state = driver.GetAppState("com.example.myapplication");
            Assert.IsTrue(state == AppState.RunningInForeground);
        }

        [Test, Order(2)]
        public void TestWrongEmailCorrectPass()
        {
            emailInput.SendKeys("");
            passInput.SendKeys("123456");
            signButton.Click();
            var state = driver.GetAppState("com.example.myapplication");
            Assert.IsTrue(state == AppState.RunningInForeground);
        }

        [Test, Order(3)]
        public void TestEverythingWrong()
        {
            emailInput.SendKeys("");
            passInput.SendKeys("");
            signButton.Click();
            var state = driver.GetAppState("com.example.myapplication");
            Assert.IsTrue(state == AppState.RunningInForeground);
        }
        [Test, Order(4)]
        public void TestEverythingCorrect()
        {
            emailInput.SendKeys("test@gmail.com");
            passInput.SendKeys("123456");
            signButton.Click();
            var state = driver.GetAppState("com.example.myapplication");
            Assert.IsTrue(state == AppState.RunningInBackground);
        }
    }
}