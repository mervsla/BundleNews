using BundleNews.Data.Entities.Abstract;
using BundleNews.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Data.UnitOfWork.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        int SaveChanges();
    }
}
