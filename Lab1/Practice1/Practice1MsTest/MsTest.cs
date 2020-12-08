using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Practice1MsTest
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

        public static List<object[]> GenerateRandom(double min, double max, int count)
        {
            List<object[]> datas = new List<object[]>();
            var rand = new Random();
            for (int i = 0; i < count; i++)
            {
                double r = rand.NextDouble() * (max - min) + min;
                double r1 = rand.NextDouble() * (max - min) + min;
                datas.Add(new object[] { r, r1 });
            }
            return datas;
        }

        public override string ToString()
        {
            return Input.ToString() + " " + Output.ToString();
        }
    }

    [TestClass]
    public class MsTest
    {
        [ClassInitialize]
        public void OneTimeInit()
        {
            Debug.WriteLine("Hello World TestContextProgress");
        }

        public static List<Data> modelB = Data.ReadFromCSV(@"D:\ITSteps\2020\Automation Testing\Lab1\Practice1\my.csv");
        public static IEnumerable<object[]> modelC => Data.GenerateRandom(1, 5, 5);

        public double MathFunction(double x)
        {
            double y = 10 / System.Math.Sqrt((-x - 1));
            return y;
        }

        [TestInitialize]
        public void Init()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            Console.WriteLine("Test Started - " + easternTime.ToShortDateString());
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("Cruel World");
        }

        [TestMethod]
        [DataRow(-26.0, 2.0)]
        public void DataDriven(double inp, double outp)
        {
            //Assert.DoesNotThrow(Throws.)
            Assert.AreEqual(MathFunction(inp), outp, "DataDriven failed with output " + outp.ToString());
        }

        [TestMethod]
        [DataRow(-1.0, Double.PositiveInfinity)]
        public void DataDrivenWithDisionByZero(double inp, double outp)
        {
            //Assert.DoesNotThrow(Throws.)
            Assert.AreEqual(MathFunction(inp), outp, "DataDriven failed with output " + outp.ToString());
        }

        [TestMethod]
        [DynamicData(nameof(modelB))]
        public void DataDrivenCSV(Data d)
        {
            Assert.AreEqual(MathFunction(d.Input), d.Output, "DataDrivenCSV failed with output " + d.Output.ToString());
        }

        [TestMethod]
        [DynamicData(nameof(modelC))]
        public void DataDrivenRandom(double inp, double outp)
        {
            Assert.AreEqual(MathFunction(inp), outp, "DataDrivenRandom failed with output " + outp.ToString());
        }

        [TestMethod, Timeout(1000)]
        public void TestWithRetryAndStop()
        {
            Assert.AreEqual("one", "three", "TestWithRetryAndStop failed on three");
        }

        [ClassCleanup]
        ~MsTest()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            Console.WriteLine("Test Ended - " + easternTime.ToShortDateString());
        }

        //Trace.TraceInformation("Test message.");
        //// You must close or flush the trace to empty the output buffer.
        //Trace.Flush();
        //Debug.WriteLine("Time {0}", DateTime.Now);
        //System.Threading.Thread.Sleep(30000);
        //Debug.WriteLine("Time {0}", DateTime.Now);
        //Assert.IsFalse("s".ToString().Contains("New text"));
        ///* call some method that writes "New text" to stdout */
        //Assert.IsTrue("s2".ToString().Contains("New text"));
    }
}
