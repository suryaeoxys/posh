using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Infrastructure
{
    public class DbFactory : IDisposable
    {
        //private bool _disposed;
        private readonly Func<PoshDbContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DbFactory(Func<PoshDbContext> dbContextFactory)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            //if (!_disposed && _dbContext != null)
            //{
            //    _disposed = true;
            //    _dbContext.Dispose();
            //}
           }
        public PoshDbContext DbContextObj()
        {
            var context = _instanceFunc();
            return context;
        }

    }
}
