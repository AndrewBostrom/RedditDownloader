using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;



namespace RedditDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = GetURL();
            List<string> imageUrls = GetImageUrls(url);

            foreach (string imgurl in imageUrls)
            {
                Console.WriteLine(imgurl);
            }
            Console.ReadLine();

            DownloadImages(imageUrls);
        }
        static bool ValidSubreddit(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }
        static string GetURL()
        {
            string subreddit;
            string displayType;
            string url;

            while (true)
            {
                Console.WriteLine("Enter subreddit: ");
                subreddit = Console.ReadLine();
                Console.WriteLine("Hot, new, or top?");
                displayType = Console.ReadLine();

                url = "https://www.reddit.com/r/" + subreddit + "/" + displayType;

                if (displayType == "top")
                {
                    Console.WriteLine("Hour, day, week, month, year, or all?");
                    url += "/?sort=top&t=" + Console.ReadLine();
                }

                if (ValidSubreddit(url))
                {
                    return url;
                }
            }
        }
        static List<string> GetImageUrls(string url)
        {
            List<string> images = new List<string>();
            HtmlDocument source = new HtmlDocument();

            var webRequest = HttpWebRequest.Create(url);
            Stream stream = webRequest.GetResponse().GetResponseStream();

            source.Load(stream);

            HtmlNodeCollection imageUrls = source.DocumentNode.SelectNodes("//img[@src]");

            foreach (HtmlNode image in imageUrls)
            {
                if (image.Attributes["src"] == null)
                {
                    continue;
                }

                HtmlAttribute src = image.Attributes["src"];

                images.Add(src.Value);
            }

            return images;
            // foreach(string uri in images)
        }
        static void DownloadImages(List<string> imageUrls)
        {

        }
    }
}



/* 
//a.thumbs.redditmedia.com/s?s7mAa7UGOBCyFC2paVCaVc9qCTCUnMd9PwZGoEIUP98.png
//www.redditstatic.com/subscribe-header.svg
//www.redditstatic.com/subscribe-header-thanks.svg
//reddit.com/static/pixel.png
*/