using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using RestSharp;

namespace LoaderSpace
{
    public class Car
    {
        public string name { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public int cost_in_credits { get; set; }
        public float length { get; set; }
        public int max_atmosphering_speed { get; set; }
        public int crew { get; set; }
        public int passengers { get; set; }
        public int cargo_capacity { get; set; }
        public string consumables { get; set; }
        public string vehicle_class { get; set; }
        public string created { get; set; }
        public string edited { get; set; }
        public string[] pilots { get; set; }
        public string[] films { get; set; }
        public string url { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Car loader &&
                   name == loader.name &&
                   model == loader.model &&
                   manufacturer == loader.manufacturer &&
                   cost_in_credits == loader.cost_in_credits &&
                   length == loader.length &&
                   max_atmosphering_speed == loader.max_atmosphering_speed &&
                   crew == loader.crew &&
                   passengers == loader.passengers &&
                   cargo_capacity == loader.cargo_capacity &&
                   consumables == loader.consumables &&
                   vehicle_class == loader.vehicle_class &&
                   created == loader.created &&
                   edited == loader.edited &&
                   url == loader.url;
        }

        public override int GetHashCode()
        {
            int ownHash = -152488099;
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(model);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(manufacturer);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(consumables);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(vehicle_class);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(created);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(edited);
            ownHash = ownHash * -1521134295 + EqualityComparer<string>.Default.GetHashCode(url);

            ownHash = ownHash * -1521134295 + cost_in_credits.GetHashCode();
            ownHash = ownHash * -1521134295 + length.GetHashCode();
            ownHash = ownHash * -1521134295 + max_atmosphering_speed.GetHashCode();
            ownHash = ownHash * -1521134295 + crew.GetHashCode();
            ownHash = ownHash * -1521134295 + passengers.GetHashCode();
            ownHash = ownHash * -1521134295 + cargo_capacity.GetHashCode();
            
            return ownHash;
        }
    }

    public abstract class Loader<T> where T : new()
    {
        protected string URL;
        RestClient client;
        public Loader(string url)
        {
            this.URL = url;
            client = new RestClient(URL);
        }
        public string sendGET(string url)
        {
            var req = new RestRequest(url, Method.GET);
            var resp = client.Execute(req);
            return resp.Content;
        }
        protected string sendGET() { return sendGET(URL); }
        public bool ValidateSchema(string validateObj)
        {
            var baseSchema = JsonSchema.FromType<T>();
            var exampleSchema = JsonSchema.FromType<Car>();
          
            return baseSchema.Validate(validateObj).Count == 0;
        }
        public List<T> Get()
        {
            string content = sendGET();

            JArray jItems = (JArray)JObject.Parse(content)["results"];
            List<T> loaders = new List<T>();
            foreach (JToken i in jItems)
            {
                T loader = new T();

                JsonSchema schema = JsonSchema.FromSampleJson(i.ToString());
                foreach (var p in schema.Properties)
                {
                    try
                    {
                        typeof(Car).GetProperty(p.Key).SetValue(loader, i[p.Key].ToObject(typeof(Loader).GetProperty(p.Key).PropertyType));
                    }
                    catch (NullReferenceException) { }
                    catch (FormatException) { }
                }
                loaders.Add(loader);
            }
            return loaders;
        }
    }
    public class LoaderLoader : Loader<Car>
    {
        public LoaderLoader() : base("https://swapi.dev/api/vehicles/")
        {
        }
    }
}
