using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    class AlwaysCanCrawlPermitter:ICrawlPermitter
    {
        public bool CanCrawl(CrawlingState crawlingState, Uri nextUri,Uri domainUri)
        {
            return true;
        }
    }
}
