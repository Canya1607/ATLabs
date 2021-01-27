using System;
using System.Collections.Generic;

namespace GoogleDrive
{
    namespace GeneratorV2
    {
        using Bogus;
        using Google.Apis.Drive.v2.Data;

        public class Generator
        {
            Generator() { }

            private String[] mimeTypes = new String[] {
                    "application/vnd.google-apps.audio",
                    "application/vnd.google-apps.document",
                    "application/vnd.google-apps.drive-sdk",
                    "application/vnd.google-apps.drawing",
                    "application/vnd.google-apps.file",
                    "application/vnd.google-apps.folder",
                    "application/vnd.google-apps.form",
                    "application/vnd.google-apps.fusiontable",
                    "application/vnd.google-apps.map",
                    "application/vnd.google-apps.photo",
                    "application/vnd.google-apps.presentation",
                    "application/vnd.google-apps.script",
                    "application/vnd.google-apps.shortcut",
                    "application/vnd.google-apps.site",
                    "application/vnd.google-apps.spreadsheet",
                    "application/vnd.google-apps.unknown",
                    "application/vnd.google-apps.video",
            };
            public static File getFile()
            {
                Faker<File> faker = new Faker<File>();
                faker.Locale = "en";
                faker.RuleFor(o => o.Title, f => f.System.CommonFileName());
                // faker.RuleFor(o => o.MimeType, f => f.PickRandom(mimeTypes));
                faker.RuleFor(o => o.Parents, f => new List<ParentReference>() { new ParentReference() { Id = "root" } });
                return faker.Generate();
            }
        }
    }

    namespace GeneratorV3
    {
        using Bogus;
        using Google.Apis.Drive.v3.Data;
        public class Generator 
        {
            Generator() { }

            private String[] mimeTypes = new String[] {
                    "application/vnd.google-apps.audio",
                    "application/vnd.google-apps.document",
                    "application/vnd.google-apps.drive-sdk",
                    "application/vnd.google-apps.drawing",
                    "application/vnd.google-apps.file",
                    "application/vnd.google-apps.folder",
                    "application/vnd.google-apps.form",
                    "application/vnd.google-apps.fusiontable",
                    "application/vnd.google-apps.map",
                    "application/vnd.google-apps.photo",
                    "application/vnd.google-apps.presentation",
                    "application/vnd.google-apps.script",
                    "application/vnd.google-apps.shortcut",
                    "application/vnd.google-apps.site",
                    "application/vnd.google-apps.spreadsheet",
                    "application/vnd.google-apps.unknown",
                    "application/vnd.google-apps.video",
        };
            public static File getFile()
            {
                Faker<File> faker = new Faker<File>();
                faker.Locale = "en";
                
                faker.RuleFor(o => o.Name, f => f.System.CommonFileName());
                //faker.RuleFor(o => o.MimeType, f => f.PickRandom(mimeTypes));
                faker.RuleFor(o => o.Parents, f => new List<String>() { "root" });
                return faker.Generate();
            }
        }
    }
}