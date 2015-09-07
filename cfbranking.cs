using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.IO;

namespace cfbranking
{
    static class cfbranking
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            InternetScraper scraper = new InternetScraper();
            scraper.setURL("http://sports.yahoo.com/ncaa/football/stats/byteam?cat1=offense&cat2=Total&conference=I-A_SEC");
            scraper.grabStats();
            scraper.readHTML();
        }
    }

    class InternetScraper
    {
        String url;
        String html;

        public InternetScraper()
        {
            
        }

        public void grabStats()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            webResponse.Close();
        }

        public void readHTML()
        {
            // The HtmlWeb class is a utility class to get the HTML over HTTP
            HtmlWeb htmlWeb = new HtmlWeb();

            // Creates an HtmlDocument object from an URL
            HtmlDocument document = htmlWeb.Load(url);

            // Extracts all links under a specific node that have an href that begins with "http://"
            HtmlNodeCollection allLinks = document.DocumentNode.SelectNodes("//*[@id='mynode']//a[starts-with(@href,'http://')]");

            // Outputs the href for external links
            foreach (HtmlNode link in allLinks)
                Console.WriteLine(link.Attributes["href"].Value);
        }

        public void setURL(String _url)
        {
            url = _url;
        }
    }
}
