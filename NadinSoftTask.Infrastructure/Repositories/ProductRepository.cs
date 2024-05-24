using Microsoft.EntityFrameworkCore;
using NadinSoftTask.Domain.Entities;
using NadinSoftTask.Domain.Repository;
using NadinSoftTask.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NadinSoftTask.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Product entity)
        {
            await _db.Products.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<Product>> GetAllAsync(Expression<Func<Product,bool>> filter = null)
        {
            IQueryable<Product> query = _db.Products;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Expression<Func<Product,bool>> filter = null, bool tracked = true)
        {
            IQueryable<Product> query = _db.Products;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Product entity)
        {
            _db.Products.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _db.Products.Update(entity);
            await SaveAsync();
        }
    }
}
