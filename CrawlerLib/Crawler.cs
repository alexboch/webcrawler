using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public class Crawler
    {
        #region Fields
        private readonly CancellationTokenSource _cts=new CancellationTokenSource();
#endregion

        public void CancelCrawling()
        {
            _cts.Cancel();
        }

        private void CrawlDomain(string domain)
        {
            
        }

        private void CrawlPage()
        {
            
        }

        public async Task CrawlDomainsAsync(IEnumerable<string> domainNames,string outputPath)
        {
            
            Parallel.ForEach(domainNames, (CrawlDomain));
        }

    }
}
