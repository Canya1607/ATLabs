using System;
using System.IO;
using System.Collections.Generic;

using NUnit.Framework;
using FluentAssertions;

namespace Practice1NUnit
{
    public class Data
    {
        public Data(double input, double output)
        {
            Input = input;
            Output = output;
        }
        public double Input { get; set; } = 0.0;
        public double Output { get; set; } = 0.0;

        public static List<Data> ReadFromCSV(string path)
        {
            List<Data> datas = new List<Data>();
            using (var csvReader = new StreamReader(@path))
            {
                while (!csvReader.EndOfStream)
                {
                    var line = csvReader.ReadLine();
                    var values = line.Split(';');
                    double val1 = Double.Parse(values[0]);
                    double val2 = Double.Parse(values[1]);
                    datas.Add(new Data(val1, val2));
                    //Console.WriteLine(val1);
                    //Console.WriteLine(val2);
                }
            }
            return datas;
        }

        public override string ToString()
        {
            return Input.ToString() + " " + Output.ToString();
        }
    }

    [TestFixture]
    public class Tests
    {
        [OneTimeSetUp]
        public void OneTimeInit()
        {
            TestContext.Progress.WriteLine("Hello World TestContextProgress");
        }

        public static List<Data> modelA = new List<Data>
        {
            new Data(-26.0, 2.0),
            new Data(-101.0, 1.0)
        };

        public static List<Data> modelB = Data.ReadFromCSV(@"D:\ITSteps\2020\Automation Testing\Lab1\Practice1\my.csv");

        public double MathFunction(double x)
        {
            double y = 10 / System.Math.Sqrt((-x - 1));
            return y;
        }

        [SetUp]
        public void Init()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            Console.WriteLine("Test Started - " + easternTime.ToShortDateString());
        }

        [TearDown]
        public void Cleanup()
        {
            Console.WriteLine("Cruel World");
        }

        [TestCase(-26.0, 2.0)]
        public void DataDriven(double inp, double outp)
        {
            Assert.AreEqual(MathFunction(inp), outp, "DataDriven failed with output " + outp.ToString());
        }

        [TestCase(-1.0, Double.PositiveInfinity)]
        public void DataDrivenWithDisionByZero(double inp, double outp)
        {
            Assert.AreEqual(MathFunction(inp), outp, "DataDriven failed with output " + outp.ToString());
        }


        [Test, TestCaseSource(nameof(modelB))]
        public void DataDrivenCSV(Data d)
        {
            Assert.AreEqual(MathFunction(d.Input), d.Output, "DataDrivenCSV failed with output " + d.Output.ToString());
        }

        [Test]
        public void DataDrivenRandom(
            [Values(-5.0, -17.0, -26.0, -101.0)] double inp,
            [Random(1, 6, 5)] double outp)
        {
            Assert.AreEqual(MathFunction(inp), outp, "DataDrivenRandom failed with output " + outp.ToString());
        }

        [Test, Retry(3), Timeout(1000)]
        public void TestWithRetryAndStop([Values("three", "two", "one")] string arg)
        {
            Assert.AreEqual("one", arg, "TestWithRetryAndStop failed on " + arg);
        }

        [OneTimeTearDown]
        ~Tests()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            TestContext.Progress.WriteLine("Test Ended - " + easternTime.ToShortDateString());
        }
    }
}