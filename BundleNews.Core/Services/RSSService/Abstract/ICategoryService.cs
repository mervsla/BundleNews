using BundleNews.Data.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Core.Services.RSSService.Abstract
{
    public interface ICategoryService
    {
        Category GetByRSSURL(string rssUrl);
        int AddCategory(string name, string url, int sourceId);
    }
}
