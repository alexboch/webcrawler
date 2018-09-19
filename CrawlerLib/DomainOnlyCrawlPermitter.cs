using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    /// <summary>
    /// Может переходить на ссылку, только если ссылка принадлежит тому же домену
    /// </summary>
    public class DomainOnlyCrawlPermitter:ICrawlPermitter
    {
        public bool CanCrawl(CrawlingState crawlingState, Uri nextUri, Uri domainUri)
        {
            return nextUri.Host==domainUri.Host;
        }
    }
}
