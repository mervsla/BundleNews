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
    public class CategoryService : ICategoryService
    {
        IUnitOfWork _uow;
        public CategoryService()
        {
            _uow = new EFUnitOfWork(new BundleNewsContext());
        }

        #region Get category by url
        public Category GetByRSSURL(string rssUrl)
        {
            return _uow.GetRepository<Category>().Get(x => x.Link == rssUrl && x.IsDeleted == false).FirstOrDefault();
        }
        #endregion
        #region Add category
        public int AddCategory(string name, string url, int sourceId)
        {
            _uow.GetRepository<Category>().Add(new Category
            {
                Name = name.Trim(),
                Link = url.Trim(),
                InsertedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                NewsSourceId = sourceId,
            });
            return _uow.SaveChanges();
        }
        #endregion
    }
}
