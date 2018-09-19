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
        public ICrawlPermitter CrawlPermitter { get;private set; }=new AlwaysCanCrawlPermitter();
        #endregion

        public event EventHandler<PageContentLoadedEventArgs> PageContentLoaded;

        public void CancelCrawling()
        {
            _cts.Cancel();
        }

        private void CrawlDomain(Uri domainUri)
        {
            CrawlingState state=new CrawlingState();//Состояние кроулинга для домена
            var uriQueue = new Queue<Uri>();//Очередь для нерекурсивного обхода в ширину
            var uriHashSet = new HashSet<string>();//Множество обойденных ури
            uriQueue.Enqueue(domainUri);
            Uri baseUri = domainUri;
            var mwc=new MyWebClient();
            int levelRefsCount = uriQueue.Count;//Количество ссылок на уровне
            int nextLevelRefs = 0;
            while (true)
            {
                if (uriQueue.Count == 0) break;
                Uri uri = uriQueue.Dequeue();
                levelRefsCount--;
                if (levelRefsCount == 0)//Если прошли все ссылки уровня
                {
                    state.Depth++;
                    levelRefsCount = nextLevelRefs;
                    nextLevelRefs = 0;
                }
                baseUri = UriHelper.MakeAbsoluteUriIfNeeded(uri, baseUri);
                if (!uriHashSet.Contains(uri.AbsolutePath)//Чтобы не зациклиться
                    &&CrawlPermitter.CanCrawl(state,uri,domainUri))
                {
                    try
                    {
                        //todo:статистика кодов статуса
                        var htmlDoc=new HtmlDocument();
                        string htmlStr=mwc.DownloadString(baseUri);
                        htmlDoc.LoadHtml(htmlStr);
                        PageContentLoaded?.Invoke(this, new PageContentLoadedEventArgs(uri, htmlDoc.DocumentNode.OuterHtml));
                        uriHashSet.Add(baseUri.AbsolutePath);
                        var uris = GetPageLinks(htmlDoc, baseUri);
                        foreach (var u in uris)
                        {
                            nextLevelRefs++;
                            uriQueue.Enqueue(u);
                        }
                    }
                    catch(WebException wex)
                    {
                        var status = wex.Status;
                        
                    }

                }
            }
        }



        private List<Uri> GetPageLinks(HtmlDocument htmlDoc, Uri baseUri)
        {

            var uriList = new List<Uri>();
            var refNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
            if (refNodes != null)
            {
                foreach (HtmlNode link in refNodes) //Все ссылки
                {
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    if (!String.IsNullOrEmpty(hrefValue))
                    {
                        try
                        {
                            var uri = new Uri(hrefValue, UriKind.RelativeOrAbsolute);
                            uriList.Add(UriHelper.MakeAbsoluteUriIfNeeded(uri, baseUri));
                        }
                        catch (UriFormatException) //todo:Добавить проверку Uri
                        {

                        }
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
            var opt = new ParallelOptions();
            opt.CancellationToken = _cts.Token;
            await Task.Factory.StartNew(() => Parallel.ForEach(uris,opt, (CrawlDomain)), _cts.Token);
        }

        public Crawler(ICrawlPermitter crawlPermitter)
        {
            if (crawlPermitter == null)
            {
                throw new ArgumentNullException(nameof(crawlPermitter));
            }
            CrawlPermitter = crawlPermitter;
        }
        public Crawler()
        {
            
        }

    }
}
