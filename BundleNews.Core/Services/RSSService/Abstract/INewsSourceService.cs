using BundleNews.Data.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Core.Services.RSSService.Abstract
{
    public interface INewsSourceService
    {
        NewsSource GetByURL(string url, string name);
    }
}
