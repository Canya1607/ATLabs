using NUnit.Framework;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using FlaUI.Core;

namespace NUnitTestLab6
{
    public class Tests
    {
        protected TimeSpan timeOut = TimeSpan.FromSeconds(20);
        public void ClickOnMenuButton(string buttnName, Window currentWindow)
        {
            var currentButton = currentWindow.FindAllDescendants().Single(x => x.Name == buttnName).AsMenuItem();
            currentButton.WaitUntilClickable();
            currentButton.Click();
        }
        public void OpenFileByModaWindow(Window currentWindow)
        {
            Retry.WhileTrue(() => currentWindow.ModalWindows.Length == 1);
            var openFile = currentWindow.ModalWindows[0].AsWindow();
            openFile.Name.Should().Be("Open");
            var edit = openFile.FindFirstChild("1148");
            edit.WaitUntilClickable();
            edit.Click();
            Retry.WhileTrue(() => edit.FrameworkAutomationElement.HasKeyboardFocus);
            Keyboard.Type(@"D:\Document.txt");
            var fileOpen = openFile.FindFirstChild("1");
            Retry.DefaultTimeout = timeOut;
            fileOpen.WaitUntilClickable(timeOut);
            fileOpen.Click();
        }
        public void EditCurrentText(Window currentWindow,string currentText, string newText)
        {
            var editText = currentWindow.FindAllDescendants().Single(x => x.Name == "Text Editor").AsTextBox();
            editText.Text.Should().Be(currentText);
            editText.Enter(newText);
        }
        public void SaveFileByModaWindow(Window currentWindow, string placeToSave,string newFileName)
        {
            Retry.WhileTrue(() => currentWindow.ModalWindows.Length == 1);
            var saveFile = currentWindow.ModalWindows[0].AsWindow();

            Keyboard.Type(placeToSave+newFileName);
            var fileName = saveFile.FindAllDescendants(x => x.ByClassName("AppControlHost")).First(x => x.Name == "File name:");
            fileName.AsTextBox().Text.Should().Be(placeToSave+newFileName);
            saveFile.FindFirstChild("1").AsButton().Invoke();
        }

        [Test]
        public void Test1()
        {
            var app = Application.Launch("notepad.exe");
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                window.Title.Should().Be("Untitled - Notepad");

                ClickOnMenuButton("File", window);
                ClickOnMenuButton("Open...", window);

                OpenFileByModaWindow(window);

                Retry.WhileTrue(() => window.Title.Equals("Document.txt - Notepad"));

                EditCurrentText(window, "Automation", "New Text");
                ClickOnMenuButton("File", window);
                ClickOnMenuButton("Save As...", window);

                var dynamicPart = DateTime.Now.ToShortDateString().Replace("/", string.Empty);
                var name = $"Document{dynamicPart}.txt";
                var path = "D:\\";

                SaveFileByModaWindow(window, path, name);

                app.Close();

                Retry.WhileFalse(() => File.Exists(path+name));
                File.ReadAllText(path+name).Should().Be("New Text");
                File.Delete(path+name);
            }
        }
    }
}