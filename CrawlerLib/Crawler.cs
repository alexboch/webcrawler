using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
            using (var wc = new WebClient())
            {
                while (true)
                {
                    if (uriQueue.Count == 0) break;
                    Uri uri = uriQueue.Dequeue();
                    if (!uriHashSet.Contains(uri))//Чтобы не зациклиться
                    {
                        string pageContent = wc.DownloadString(uri);
                        uriHashSet.Add(uri);
                        var uris = CrawlPage(pageContent);
                        foreach (var u in uris)
                        {
                            uriQueue.Enqueue(u);
                        }
                    }
                }
            }
        }


        private List<Uri> CrawlPage(string pageContent)
        {
            var uriList = new List<Uri>();
            var matches = _hrefRegex.Matches(pageContent);
            
            foreach (Match m in matches)
            {
                var group = m.Groups["url"];
                string urlString =group.Value;
                var uri = new Uri(urlString);
                uriList.Add(uri);
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
