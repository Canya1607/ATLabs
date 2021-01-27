using NUnit.Framework;
using Google.Apis.Drive.v2.Data;
using System.Collections;
using System.Collections.Generic;
using Google.Apis.Drive.v3.Data;
using GoogleDriveTest;

namespace GoogleDriveAPIV2
{
    using GoogleDrive.GeneratorV2;
    using File = Google.Apis.Drive.v2.Data.File;
    public class V2Test
    {
        const string appname = "GAPITestsV2";
        GoogleDriveV2 gdV2;
        [OneTimeSetUp]
        public void Setup()
        {
            gdV2 = new GoogleDriveV2(appname);
        }

        [Test]
        [TestCase("D:\\ITSteps\\2020\\Automation Testing\\Lab2Try\\GoogleDriveTest\\credentials.json")]
        public void GoogleDriveV2ContructorTest(string pathToCredentials)
        {
            GoogleDriveV2 gdV2Test = new GoogleDriveV2(appname);
            Assert.NotNull(gdV2Test.ds);
            Assert.NotNull(gdV2Test.credentials);
        }

        [Test]
        public void FilesGetTest()
        {
            List<File> files = gdV2.retrieveAllFiles();
            Assert.IsTrue(files.Count > 0);
        }

        static IEnumerable SingleFileTestData
        {
            get
            {
                yield return new TestCaseData("/file1.txt");
                yield return new TestCaseData("/New Folder/file2.pdf");
                yield return new TestCaseData("/error.txt").Ignore("This file does not exist");
            }
        }
        [Test]
        [TestCaseSource(nameof(SingleFileTestData))]
        public void SingleFileGetTest(string filePath)
        {
            File file = gdV2.retrieveFile(filePath);
            Assert.IsNotNull(file);
        }


        static IEnumerable UploadFileTestData
        {
            get
            {
                yield return new TestCaseData(Generator.getFile());
            }
        }
        [Test]
        [TestCaseSource(nameof(UploadFileTestData))]
        public void UploadFileTest(File file)
        {
            bool uploaded = gdV2.uploadFile(file);
            Assert.IsTrue(uploaded);
        }
    }
}

namespace GoogleDriveAPIV3
{
    using GoogleDrive.GeneratorV3;
    using File = Google.Apis.Drive.v3.Data.File;
    public class V3Test
    {
        const string defaultCredentialPath = "D:\\ITSteps\\2020\\Automation Testing\\Lab2Try\\GoogleDriveTest\\credentials.json";
        const string appname = "GAPITestV3";
        GoogleDriveV3 gdV3;

        [OneTimeSetUp]
        public void Setup()
        {
            gdV3 = new GoogleDriveV3(appname);
        }

        [Test]
        [TestCase(defaultCredentialPath)]
        public void GoogleDriveV2ContructorTest(string pathToCredentials)
        {
            GoogleDriveV3 gdV3 = new GoogleDriveV3(appname);
            Assert.NotNull(gdV3.ds);
            Assert.NotNull(gdV3.credentials);
        }

        [Test]
        public void FilesGetTest()
        {
            List<File> files = gdV3.retrieveAllFiles();
            Assert.IsTrue(files.Count > 0);
        }

        static IEnumerable SingleFileTestData
        {
            get
            {
                yield return new TestCaseData("/file1.txt");
                yield return new TestCaseData("/New Folder/file2.pdf");
                yield return new TestCaseData("/error.txt").Ignore("This file does not exist");
            }
        }
        [Test]
        [TestCaseSource(nameof(SingleFileTestData))]
        public void SingleFileGetTest(string filePath)
        {
            File file = gdV3.retrieveFile(filePath);
            Assert.IsNotNull(file);
        }

        static IEnumerable UploadFileTestData
        {
            get
            {
                yield return new TestCaseData(Generator.getFile());
            }
        }
        [Test]
        [TestCaseSource(nameof(UploadFileTestData))]
        public void UploadFileTest(File file)
        {
            bool uploaded = gdV3.uploadFile(file);
            Assert.IsTrue(uploaded);
        }
    }
}