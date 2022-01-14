using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Core.Services.RSSService.Abstract
{
   public interface IRssService
    {
        bool RunRssGenerator(string url, string name, IHostEnvironment environment);
    }
}
