using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;
using SikuliSharp;
using WindowsInput;

namespace SikulixTest
{
    public class Tests
    {
        InputSimulator keyboard = new InputSimulator();
        ISikuliSession session;
        Process notepad;

        private IPattern ResolveScreenshot(string screenshotName, double similiraty = 0.7) => Patterns.FromFile($"D:\\ITSteps\\2020\\Automation Testing\\{screenshotName}", (float)similiraty);

        [OneTimeSetUp]
        public void Setup()
        {
            session = Sikuli.CreateSession();
            notepad = Process.Start("notepad.exe");
        }

        [Test, TestCase("D:\\ITSteps\\2020\\Automation Testing", "test.txt"), Order(1)]
        public void OpenFileTest(string folderPath, string fileName)
        {
            var fileButton = ResolveScreenshot("fB.png");
            session.Click(fileButton);

            var openButton = ResolveScreenshot("oB.png");
            session.Click(openButton);

            var folderInput = ResolveScreenshot("fI.png");
            var fileInput = ResolveScreenshot("fIT.png");
            var dialogOpenButton = ResolveScreenshot("dOB.png");

            session.Click(folderInput);
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(folderPath);
            session.Click(fileInput, new Point(100, 0));
            keyboard.Keyboard.TextEntry(fileName);
            session.Click(dialogOpenButton);

            var expectResult = ResolveScreenshot("eH1.png");
            Assert.IsTrue(session.Exists(expectResult), "File not opened");
        }
        [Test, TestCase("123"), Order(2)]
        public void InputTest(string input)
        {
            var header = ResolveScreenshot("h.png");
            session.DoubleClick(header, new Point(0, 150));
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(input);
            var expectResult = ResolveScreenshot("eR.png");
            Assert.IsTrue(session.Exists(expectResult), "File not edited");
        }

        [Test, TestCase("D:\\ITSteps\\2020\\Automation Testing", "test1.txt"), Order(3)]
        public void SaveFileTest(string folderPath, string newName, bool overrideFile = true)
        {
            var fileButton = ResolveScreenshot("fB.png");
            session.Click(fileButton);

            var saveButton = ResolveScreenshot("sB.png");
            session.Click(saveButton);

            var folderInput = ResolveScreenshot("fII.png");
            var fileInput = ResolveScreenshot("fITI.png");

            session.Click(folderInput);
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(folderPath);
            session.Click(fileInput, new Point(100, 0));
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(newName);

            var dialogSaveButton = ResolveScreenshot("dSB.png");
            session.Click(dialogSaveButton);

            var warnSign = ResolveScreenshot("wS.png");
            if (session.Exists(warnSign))
            {
                var yesButton = ResolveScreenshot("yB.png");
                if (overrideFile)
                    session.Click(yesButton);
                else
                    throw new Exception("!override");
            }

            var expectResult = ResolveScreenshot("eH2.png");
            Assert.IsTrue(session.Exists(expectResult), "File not saved");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            session.Dispose();
            notepad.Kill();
        }
    }
}