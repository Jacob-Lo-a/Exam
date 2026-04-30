using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IProductRepository
    {
        Task<bool> ExistsAsync(string productName);
        Task<string?> GetMaxProductIdAsync();
        Task<Product?> GetByIdAsync(string productId);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task AddAsync(Product product);
        Task<bool> ExistsInOrderAsync(string productId);
        Task<IPagedList<Product>> GetPagedAsync(string? keyword, int pageNumber, int pageSize);
    }
}
