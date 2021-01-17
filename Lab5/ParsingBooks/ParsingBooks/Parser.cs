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
        private void Download(string fileUrl,string filePath )
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            webClient.DownloadFileAsync(new Uri(fileUrl), filePath);
            while (webClient.IsBusy) { }
        }
        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }
        private void DownloadElementFromPage(string elementPath, HtmlDocument html)
        {
            Parallel.ForEach(html.DocumentNode.SelectNodes(elementPath), item => Proces(item));
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Download has been canceled.");
            }
            else
            {
                Console.WriteLine("Download completed!");
            }
        }
        private void Proces(HtmlNode item)
        {
            var bookPagelink = item.Attributes["href"].Value;
            var bookPage = web.Load(bookPagelink);
            try { 

                var downloadUrl = bookPage.DocumentNode.SelectSingleNode("//div/article/footer/div/span[1]/a").Attributes["href"].Value;
                if (PdfExist(downloadUrl))
                {
                    string filename = getFilename(downloadUrl);
                    Download(downloadUrl, @"D:\\books\\" + filename);
                }
                else
                {
                    downloadUrl = bookPage.DocumentNode.SelectSingleNode("//div/article/footer/div/span[2]/a").Attributes["href"].Value;
                    if (PdfExist(downloadUrl))
                    {
                        string filename = getFilename(downloadUrl);
                        Download(downloadUrl, @"D:\\books\\" + filename);
                    }
                    else Console.WriteLine("There is no PDF file here");
                }
            }
            catch (System.NullReferenceException e) 
            {
                Console.WriteLine("There is no PDF file here");
            }


        }
        private Boolean PdfExist(string downloadUrl)
        {
            var result = downloadUrl.Substring(downloadUrl.Length - 3)=="pdf" ? true:false ;
            return result;
        }
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }
        public void parseBook()
        {
            var html = $"http://www.allitebooks.com/page/"+Page;
            var htmlDoc = web.Load(html);

            DownloadElementFromPage("//header/h2/a", htmlDoc);
        }
    }
}
