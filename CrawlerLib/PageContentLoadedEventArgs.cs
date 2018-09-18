using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public class PageContentLoadedEventArgs:EventArgs
    {
        public Uri PageUri { get; set; }
        public string PageContent { get; set; }

        public PageContentLoadedEventArgs(Uri uri,string pageContent)
        {
            PageUri = uri;
            PageContent = pageContent;
        }
    }
}
