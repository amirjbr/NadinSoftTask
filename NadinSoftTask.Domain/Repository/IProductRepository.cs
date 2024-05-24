using Microsoft.Win32.SafeHandles;
using NadinSoftTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NadinSoftTask.Domain.Repository
{
    public interface IProductRepository
    {
        // chon faghat 1 dune model darim niaz nist Repositorymon be surat generic bashe
        Task<List<Product>> GetAllAsync(Expression<Func<Product,bool>> filter = null);
        Task<Product> GetByIdAsync(Expression<Func<Product,bool>> filter = null, bool tracked = true);
        Task CreateAsync(Product entity);
        Task UpdateAsync(Product entity);
        Task RemoveAsync(Product entity);
        Task SaveAsync();
    }
}
