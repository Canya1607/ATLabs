using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using File = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveTest
{
    public class GoogleDriveV3 : GoogleDrive<File>
    {
        public DriveService ds { get; private set; }

        public override string[] scopes => new[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile, DriveService.Scope.DriveMetadata, DriveService.Scope.DriveAppdata, DriveService.Scope.DriveScripts };

        public GoogleDriveV3(string appname) : base()
        {
            ds = driveServiceFromCredential(appname);
        }
        private DriveService driveServiceFromCredential(string applicationName)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = applicationName,
            });
        }

        override public List<File> retrieveAllFiles()
        {
            List<File> result = new List<File>();
            FilesResource.ListRequest request = ds.Files.List();
            do
            {
                try
                {
                    FileList files = request.Execute();
                    result.AddRange(files.Files);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return result;
        }


        override public File retrieveFile(String path, File parent = null)
        {
            String[] sPath = path.Split('/');
            sPath = (from s in sPath where s.Length > 0 select s).ToArray();
            
            FilesResource.ListRequest lr = ds.Files.List();
            lr.Q = parent != null ? $"name='{sPath[0]}' and parents in '{parent.Id}'" : $"name='{sPath[0]}' and parents in 'root'";
            FileList fileList = lr.Execute();
            if (fileList.Files.Count < 1) throw new GoogleDriveFileNotFoundException();
            IEnumerable<File> files = from f in fileList.Files select f;

            return sPath.Length>1 ? retrieveFile(String.Join("/", sPath.Skip(1)), parent = files.First()) : files.First();
        }

        override public bool uploadFile(File file)
        {
            var fs = System.IO.File.Open("D:/ITSteps/2020/Automation Testing/Lab2Try/GoogleDriveTest/file.txt", FileMode.OpenOrCreate);
            var insertion = ds.Files.Create(file);
            insertion.Fields = "id";
            var result = insertion.Execute();

            return true;
        }
    }
}
