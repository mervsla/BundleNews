using BundleNews.Data.Context;
using BundleNews.Data.Entities.Abstract;
using BundleNews.Data.Repositories.Abstract;
using BundleNews.Data.Repositories.Concrete;
using BundleNews.Data.UnitOfWork.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Data.UnitOfWork.Concrete
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly BundleNewsContext _context;
        public EFUnitOfWork(BundleNewsContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context boş olamaz");
            _context = context;
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this.disposed = true;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            return new EFRepository<TEntity>(_context);
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch
            {
                return 0;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
