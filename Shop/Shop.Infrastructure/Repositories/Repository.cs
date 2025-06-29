using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions; // Cần thiết cho Expression
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Cần thiết cho DbContext, DbSet, FirstOrDefaultAsync, FindAsync, ToListAsync
using Shop.Domain.Interfaces;
using Shop.Infrastructure.Data; // Để truy cập ShopDbContext

namespace Shop.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ShopDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ShopDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // Khởi tạo DbSet cho kiểu T
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            // FirstOrDefaultAsync được gọi trên DbSet
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // FindAsync thường chỉ hoạt động với primary key
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            // Trả về số lượng bản ghi đã được lưu
            return await _context.SaveChangesAsync();
        }
    }
}