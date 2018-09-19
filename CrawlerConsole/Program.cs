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
        static async void Main(string[] args)
        {
            var crawler = new Crawler();
            await crawler.CrawlDomainsAsync(new []{"https://google.ru/" },"Output");
            Console.ReadKey();
        }
    }
}
