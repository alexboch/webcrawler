using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrawlerLib;

namespace CrawlerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var crawler = new Crawler();
            //crawler.CrawlDomainsAsync(new []{"https://google.ru/" },"Output").RunSynchronously();

            var uri1 = new UriBuilder("google.yandex.com").Uri;
            var subdomainUri = new UriBuilder("yandex.com").Uri;
             subdomainUri.IsSubdomainOf(uri1);
            Console.ReadKey();
        }
    }
}
