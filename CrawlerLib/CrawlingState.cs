using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CrawlerLib
{
    struct CrawlingState
    {
        public int Depth { get; set; }
        public int TotalPagesVisited { get;set; }
        public string CurrentUriString { get; set; }
        public string CurrentDocumentContent { get; set; }
        

    }
}
