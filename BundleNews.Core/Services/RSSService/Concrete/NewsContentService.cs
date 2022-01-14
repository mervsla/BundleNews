using BundleNews.Core.Services.RSSService.Abstract;
using BundleNews.Data.Context;
using BundleNews.Data.Entities.Concrete;
using BundleNews.Data.UnitOfWork.Abstract;
using BundleNews.Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Core.Services.RSSService.Concrete
{
    public class NewsContentService : INewsContentService
    {
        IUnitOfWork _uow;
        public NewsContentService()
        {
            _uow = new EFUnitOfWork(new BundleNewsContext());
        }
        public int AddSourceContent(List<NewsContent> newsContentList)
        {
            _uow.GetRepository<NewsContent>().AddList(newsContentList);
            return _uow.SaveChanges();
        }
    }
}
