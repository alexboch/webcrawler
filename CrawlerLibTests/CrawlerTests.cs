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
    public class CrawlerTests
    {
        [TestMethod()]
        public void CancelCrawlingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CrawlDomainsAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CrawlPageTest()
        {
            var crawler = new Crawler();
            PrivateObject pObj = new PrivateObject(crawler);
            List<Uri> urls=pObj.Invoke("CrawlPage", "<div> <a href=\"mysite.com\"></a> </div>") as List<Uri>;
            
        }
    }
}