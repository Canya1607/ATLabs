using FileHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LoaderSpace;
using TechTalk.SpecFlow;

namespace SpecFlowTest
{
    public class Tests
    {
        Car car = new Car();

        [Test, TestCase(1000)]
        public void TestFile(int sleep)
        {
            string fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, JsonConvert.SerializeObject(car.Get()));
            Thread.Sleep(sleep);
            var newCars = car.Get().ToArray();
            var trashCars = JsonConvert.DeserializeObject<Loader[]>(File.ReadAllText(fileName));
            Assert.IsTrue(newCars.Length == trashCars.Length);
            Assert.AreEqual(trashCars, newCars);
        }

        [Test, TestCase("BMW X5"), TestCase("Fails", Ignore = "Will Fail")]
        public void FindExactCar(string name)
        {
            var cars = car.Get();
            Assert.IsNotEmpty(from c in cars where c.name == name select c);
        }

        static Func<string, string> FetchResults = (string r) => JObject.Parse(r)["results"][0].ToString();
        static IEnumerable testacases
        {
            get
            {
                yield return new TestCaseData("https://swapi.dev/api/vehicles/", FetchResults).Ignore("This test will fails exceptually");
            }
        }

        [Test, 
         TestCaseSource(nameof(testacases))]
        public void TestWork(string url2test, Func<string,string> getFunction)
        {
            var resp = car.send()
            var e = getFunction(resp);
            Assert.IsTrue(car.ValidateSchema(e));
        }
        [Test]
        public void TestGet()
        {
            var results = car.Get();
            Assert.IsTrue(results.Count > 0);
        }
    }
}