using System;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.ComponentModel;

namespace ParsingBooks
{
    public class Parser
    {
        private HtmlWeb web = new HtmlWeb();
        private ushort Page { get; set; }

        public Parser(ushort currentPage)
        {
            this.Page = currentPage;
        }

        private void Download(string fUrl,string fPath )
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Finished);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Progress);
            webClient.DownloadFileAsync(new Uri(fUrl), fPath);
            while (webClient.IsBusy) { }
        }
        private void Progress(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("{0} downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }

        private void Download(string elementPath, HtmlDocument html)
        {
            Parallel.ForEach(html.DocumentNode.SelectNodes(elementPath), item => InJob(item));
        }

        private void Finished(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Download down.");
            }
            else
            {
                Console.WriteLine("Download up!");
            }
        }
        private void InJob(HtmlNode item)
        {
            var bookPagelink = item.Attributes["href"].Value;
            var bookPage = web.Load(bookPagelink);
            try { 

                var url = bookPage.DocumentNode.SelectSingleNode("//div/article/footer/div/span[1]/a").Attributes["href"].Value;
                if (isPDF(url))
                {
                    string filename = getFilename(url);
                    Download(url, @"D:\\ITSteps\\2020\\Automation Testing\\books\\" + filename);
                }
                else
                {
                    url = bookPage.DocumentNode.SelectSingleNode("//div/article/footer/div/span[2]/a").Attributes["href"].Value;
                    if (isPDF(url))
                    {
                        string filename = getFilename(url);
                        Download(url, @"D:\\ITSteps\\2020\\Automation Testing\\books\\" + filename);
                    }
                    else Console.WriteLine("No PDF");
                }
            }
            catch (System.NullReferenceException e) 
            {
                Console.WriteLine("No PDF");
            }


        }

        private Boolean isPDF(string url)
        {
            var result = url.Substring(url.Length - 3)=="pdf" ? true:false ;
            return result;
        }

        private string getFilename(string href)
        {
            Uri uri = new Uri(href);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        public void parse()
        {
            var html = $"http://www.allitebooks.com/page/"+Page;
            var htmlDoc = web.Load(html);

            Download("//header/h2/a", htmlDoc);
        }
    }
}
