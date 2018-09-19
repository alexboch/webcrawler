using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public interface ICrawlPermitter
    {
        bool CanCrawl(CrawlingState crawlingState,Uri nextUri,Uri domainUri);
    }
}
