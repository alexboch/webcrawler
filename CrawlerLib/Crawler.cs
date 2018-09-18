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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        #endregion

        public void CancelCrawling()
        {
            _cts.Cancel();
        }

        private void CrawlDomain(Uri domainUri)
        {
            Debug.WriteLine(domainUri);
            var uriQueue = new Queue<Uri>();//Очередь для нерекурсивного обхода в ширину
            var uriHashSet = new HashSet<string>();//Множество обойденных ури
            uriQueue.Enqueue(domainUri);
            var hc = new HtmlWeb();
            Uri baseUri = domainUri;
            while (true)
            {
                if (uriQueue.Count == 0) break;
                Uri uri = uriQueue.Dequeue();
                baseUri = UriHelper.MakeAbsoluteUriIfNeeded(uri, baseUri);
                if (!uriHashSet.Contains(uri.AbsolutePath))//Чтобы не зациклиться
                {
                    var htmlDoc = hc.Load(baseUri);//todo:обработка ошибок(напр. 404)
                    uriHashSet.Add(baseUri.AbsolutePath);
                    var uris = CrawlPage(htmlDoc, baseUri);
                    foreach (var u in uris)
                    {
                        uriQueue.Enqueue(u);
                    }
                }
            }
        }



        private List<Uri> CrawlPage(HtmlDocument htmlDoc, Uri baseUri)
        {

            var uriList = new List<Uri>();
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))//Все ссылки
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                if (!String.IsNullOrEmpty(hrefValue))
                {
                    try
                    {
                        //var uri = new UriBuilder("http\\",baseUri.Host).Uri;
                        var uri = new Uri(hrefValue, UriKind.RelativeOrAbsolute);
                        uriList.Add(UriHelper.MakeAbsoluteUriIfNeeded(uri, baseUri));
                    }
                    catch(UriFormatException)//todo:Добавить проверку Uri
                    {

                    }
                }
            }
            return uriList;
        }

        public async Task CrawlDomainsAsync(IEnumerable<string> domainNames, string outputPath)
        {
            var uris = domainNames.Select((name) =>
            {
                return new UriBuilder(name).Uri;
            });
            await Task.Factory.StartNew(() => Parallel.ForEach(uris, (CrawlDomain)), _cts.Token);
        }

    }
}
