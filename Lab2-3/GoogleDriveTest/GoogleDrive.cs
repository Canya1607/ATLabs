using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleDriveTest
{
    public abstract class GoogleDrive<File>
    {
        public class GoogleDriveFileNotFoundException : Exception
        {
            public new string Message = "There is no such file on your Google Drive";
        }
        public UserCredential credentials { get; private set; }
        abstract public string[] scopes { get; }

        public GoogleDrive(string credentialPath = "D:\\ITSteps\\2020\\Automation Testing\\Lab2Try\\GoogleDriveTest\\credentials.json")
        {
            string newCredentialPath = $"credentials_{DateTime.Now.Date.ToFileTimeUtc()}";
            
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(newCredentialPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + newCredentialPath);
            }
        }
        public abstract List<File> retrieveAllFiles();
        public abstract File retrieveFile(String path, File parent);
        public abstract bool uploadFile(File file);
    }
}
