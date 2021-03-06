using BundleNews.Data.Context;
using BundleNews.Data.Entities.Abstract;
using BundleNews.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BundleNews.Data.Repositories.Concrete
{
    public class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity :BaseEntity, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public EFRepository(BundleNewsContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context boş olamaz.");

            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }
        public void AddList(List<TEntity> entityList)
        {
            _dbset.AddRange(entityList);
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return _dbset.Where(filter);
        }

        public List<TEntity> GetAll()
        {
            return _dbset.ToList();
        }

        //public void Remove(TEntity entity)
        //{
        //    if (entity.GetType().GetProperty("IsDeleted") != null)
        //    {
        //        entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
        //        this.Update(entity);
        //    }
        //    else
        //    {
        //        if (_context.Entry(entity).State != EntityState.Deleted)
        //            _context.Entry(entity).State = EntityState.Deleted;
        //        else
        //        {
        //            _dbset.Attach(entity);
        //            _dbset.Remove(entity);
        //        }
        //    }
        //}

        public void Update(TEntity entity)
        {
            _dbset.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
