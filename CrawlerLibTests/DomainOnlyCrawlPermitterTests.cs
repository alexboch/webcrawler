using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrawlerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib.Tests
{
    [TestClass()]
    public class DomainOnlyCrawlPermitterTests
    {
        [TestMethod()]
        public void CanCrawlTest()
        {
            var cp=new DomainOnlyCrawlPermitter();
            var uri=new UriBuilder("news.google.com/page1").Uri;
            var domainUri=new UriBuilder("https://google.com").Uri;
            bool cantCrawl = cp.CanCrawl(new CrawlingState(), uri, domainUri);
            Assert.AreEqual(cantCrawl,false);
            uri=new UriBuilder("google.com/page.html").Uri;
            bool canCrawl = cp.CanCrawl(new CrawlingState(), uri, domainUri);
            Assert.AreEqual(canCrawl, true);
        }
    }
}