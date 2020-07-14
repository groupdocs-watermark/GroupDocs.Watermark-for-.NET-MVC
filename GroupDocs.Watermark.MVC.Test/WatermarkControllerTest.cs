using GroupDocs.Watermark.MVC.Controllers;
using Huygens;
using MvcContrib.TestHelper;
using NUnit.Framework;
using System;
using System.Web.Routing;

namespace GroupDocs.Watermark.MVC.Test
{
    [TestFixture]
    public class WatermarkControllerTest
    {
        
        [SetUp]
        public void TestInitialize()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }

        [Test]
        public void ViewStatusTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/../../../src";
            using (var server = new DirectServer(path))
            {
                var request = new SerialisableRequest
                {
                    Method = "GET",
                    RequestUri = "/watermark",
                    Content = null
                };

                var result = server.DirectCall(request);
                Assert.That(result.StatusCode, Is.EqualTo(200));
            }
        }

        [Test]
        public void ViewMapControllerTest()
        {
            "~/watermark".Route().ShouldMapTo<WatermarkController>(x => x.Index());
        }
    }
}
