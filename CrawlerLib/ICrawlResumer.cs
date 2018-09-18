using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    interface ICrawlResumer
    {
        bool CanCrawl(CrawlingState crawlingState);
    }
}
