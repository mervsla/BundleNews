using BundleNews.Core.RSSService;
using BundleNews.Core.Services.RSSService.Abstract;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Core.Services.RSSService.Concrete
{
    public class RssService:IRssService
    {
        public bool RunRssGenerator(string url, string name, IHostEnvironment environment)
        {
            RssFeedGenerator generator = new RssFeedGenerator(url, name, environment);
            return generator.RSSCreator();
        }
    }
}
