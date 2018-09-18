using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    static class UriHelper
    {
        public static Uri MakeAbsoluteUriIfNeeded(Uri uri,Uri baseUri)
        {
            if (!uri.IsAbsoluteUri)
                uri = new Uri(baseUri, uri);
            return uri;
        }
    }
}
