using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;
using X.PagedList.EF;

namespace Exam.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ExamDbContext _context;

        public ProductRepository(ExamDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string productName)
        {
            return await _context.Products
                .AnyAsync(x => x.ProductName == productName);
        }

        public async Task<string?> GetMaxProductIdAsync()
        {
            return await _context.Products
                .OrderByDescending(x => x.ProductId)
                .Select(x => x.ProductId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Product?> GetByIdAsync(string productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsInOrderAsync(string productId)
        {
            return await _context.OrderDetails
                .AnyAsync(x => x.ProductId == productId);
        }
        public IQueryable<Product> GetQuery(string? keyword)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.ProductName.Contains(keyword) ||
                    x.ProductId.Contains(keyword));
            }

            return query;
        }
    }
}
