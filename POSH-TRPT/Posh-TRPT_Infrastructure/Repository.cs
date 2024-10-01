using Posh_TRPT_Domain.Base;
using Posh_TRPT_Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Posh_TRPT_Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbFactory _dbFactory;

        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Repository(DbFactory dbFactory)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _dbFactory = dbFactory;
        }

        public void Add(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
            }
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            {
                ((IDeleteEntity)entity).IsDeleted = true;
                DbSet.Update(entity);
            }
            else
                DbSet.Remove(entity);
        }

        public IQueryable<T> List(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }
        public T Get(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression).FirstOrDefault()!;
        }
        public void Update(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).UpdatedDate = DateTime.UtcNow;
            }
            DbSet.Update(entity);
        }
        public PoshDbContext DbContextObj()
        {
            return _dbFactory.DbContextObj();

        }


    }
}
