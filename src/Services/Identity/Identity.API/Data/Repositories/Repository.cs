using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Identity.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly IdentityDbContext _context;

        public RepositoryBase(IdentityDbContext context)
        {
            _context = context;
        }
        public virtual async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}