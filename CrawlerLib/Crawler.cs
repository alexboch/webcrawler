using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using HtmlAgilityPack;

namespace CrawlerLib
{
    public class Crawler
    {
        #region Fields
        private const string RegexString = "<a.+href\\s*=(?<url>\\s*\".*\"\\s*)>.*</a>";
//.* "\s*)>.*</a>";
        private readonly Regex _hrefRegex = new Regex(RegexString);
        private readonly CancellationTokenSource _cts=new CancellationTokenSource();
#endregion

        public void CancelCrawling()
        {
            _cts.Cancel();
        }

        private void CrawlDomain(Uri domainUri)
        {
            Debug.WriteLine(domainUri);
            var uriQueue = new Queue<Uri>();//Очередь для нерекурсивного обхода в ширину
            var uriHashSet = new HashSet<Uri>();//Множество обойденных ури
            uriQueue.Enqueue(domainUri);
            var hc = new HtmlWeb();
            Uri baseUri = domainUri;
            while (true)
            {
                if (uriQueue.Count == 0) break;
                Uri uri = uriQueue.Dequeue();
                if (!uriHashSet.Contains(uri))//Чтобы не зациклиться
                {
                    var htmlDoc = hc.Load(uri);
                    uriHashSet.Add(uri);
                    var uris = CrawlPage(htmlDoc,baseUri);
                    foreach (var u in uris)
                    {
                        uriQueue.Enqueue(u);
                    }
                }
            }

        }


        private List<Uri> CrawlPage(HtmlDocument htmlDoc,Uri baseUri)
        {
            var uriList = new List<Uri>();
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))//Все ссылки
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                if(!String.IsNullOrEmpty(hrefValue))
                    uriList.Add(new Uri(hrefValue));
            }
            return uriList;
        }

        public async Task CrawlDomainsAsync(IEnumerable<string> domainNames,string outputPath)
        {
            var uris = domainNames.Select((name) => new Uri(name,UriKind.Absolute));
            await Task.Factory.StartNew(()=>Parallel.ForEach(uris, (CrawlDomain)),_cts.Token);
        }

    }
}
