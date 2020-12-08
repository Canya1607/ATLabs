using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime;

using Xunit;

namespace Practice1xUnit
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

        public static List<object[]> ReadFromCSV(string path)
        {
            List<object[]> datas = new List<object[]>();
            using (var csvReader = new StreamReader(@path))
            {
                while (!csvReader.EndOfStream)
                {
                    var line = csvReader.ReadLine();
                    var values = line.Split(';');
                    double val1 = Double.Parse(values[0]);
                    double val2 = Double.Parse(values[1]);
                    datas.Add(new object[] { val1, val2 });
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

    public class xUnit
    {
        public static List<Data> modelA = new List<Data>
        {
            new Data(-26.0, 2.0),
            new Data(-101.0, 1.0)
        };

        public static IEnumerable<object[]> modelB => Data.ReadFromCSV(@"D:\ITSteps\2020\Automation Testing\Lab1\Practice1\my.csv");

        public static IEnumerable<object[]> modelC => Data.GenerateRandom(1, 5, 5);

        public double MathFunction(double x)
        {
            double y = 10 / Math.Sqrt((-x - 1));
            return y;
        }

        [Theory]
        [InlineData(-26.0, 2.0)]
        public void DataDriven(double inp, double outp)
        {
            //Assert.DoesNotThrow(Throws.)
            Assert.Equal(MathFunction(inp), outp);
        }

        [Theory]
        [InlineData(-1.0, Double.PositiveInfinity)]
        public void DataDrivenWithDisionByZero1(double inp, double outp)
        {
            Assert.Equal(MathFunction(inp), outp);
        }

        [Theory]
        [MemberData(nameof(modelB))]
        public void DataDrivenCSV(double inp, double outp)
        {
            Assert.Equal(MathFunction(inp), outp);
        }

        [Theory]
        [MemberData(nameof(modelC))]
        public void DataDrivenRandom(double inp, double outp)
        {
            Assert.Equal(MathFunction(inp), outp);
        }

        [Fact(Timeout = 1000)]
        public void TestWithStop()
        {
            Assert.Equal("one", 1.ToString());
        }
    }
}
