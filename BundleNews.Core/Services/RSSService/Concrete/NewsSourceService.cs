using BundleNews.Core.Services.RSSService.Abstract;
using BundleNews.Data.Context;
using BundleNews.Data.Entities.Concrete;
using BundleNews.Data.UnitOfWork.Abstract;
using BundleNews.Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BundleNews.Core.Services.RSSService.Concrete
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly IUnitOfWork _uow;
        public NewsSourceService()
        {
            _uow = new EFUnitOfWork(new BundleNewsContext());
        }
        #region Add source by url
        public int Add(string name, string url)
        {
            NewsSource source = new NewsSource()
            {
                InsertedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = name,
                URL = url,
                IsDeleted = false
            };
            _uow.GetRepository<NewsSource>().Add(source);
            return _uow.SaveChanges();
        }
        #endregion
        #region Get source by url
        public NewsSource GetByURL(string url, string name)
        {
            return _uow.GetRepository<NewsSource>().Get(x => (x.URL == url || x.Name == name) && x.IsDeleted == false).FirstOrDefault();
        }
        #endregion
    }
}
