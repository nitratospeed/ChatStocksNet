using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Audit
    {
        private readonly AppDbContext _appDbContext;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<T>> GetAllByFilter(Expression<Func<T, bool>> condition)
        {
            return await _appDbContext.Set<T>().Where(condition).ToListAsync();
        }

        public async Task<T> GetByFilter(Expression<Func<T, bool>> condition)
        {
            return await _appDbContext.Set<T>().FirstOrDefaultAsync(condition);
        }

        public async Task<bool> AnyFilter(Expression<Func<T, bool>> condition)
        {
            return await _appDbContext.Set<T>().AnyAsync(condition);
        }

        public async Task<T> Insert(T entity)
        {
            entity.CreatedBy = "system";
            entity.CreatedAt = DateTime.Now;
            await _appDbContext.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
