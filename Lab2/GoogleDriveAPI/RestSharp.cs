using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace GoogleDriveAPI
{
    public class RestSharpTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var client = new RestClient("https://www.googleapis.com");
            //client.Authenticator = new HttpBasicAuthenticator("username", "password");

            var request = new RestRequest("drive/v2/files", DataFormat.Json);

            var response = client.Get(request);
        }
    }
}
