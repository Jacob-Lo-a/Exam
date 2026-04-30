using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;
using X.PagedList.EF;

namespace Exam.API.Repositories
{
    public class BomRepository : IBomRepository
    {
        private readonly ExamDbContext _context;

        public BomRepository(ExamDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string productId, string materialId)
        {
            return await _context.Boms
                .AnyAsync(x => x.ProductId == productId && x.MaterialId == materialId);
        }

        public async Task<Bom?> GetByIdAsync(int bomId)
        {
            return await _context.Boms.FindAsync(bomId);
        }

        public async Task AddAsync(Bom bom)
        {
            await _context.Boms.AddAsync(bom);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bom bom)
        {
            _context.Boms.Update(bom);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bom bom)
        {
            _context.Boms.Remove(bom);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductHasOrderAsync(string productId)
        {
            return await _context.OrderDetails
                .AnyAsync(x => x.ProductId == productId);
        }

        public async Task<IPagedList<Bom>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Boms
                .Include(x => x.Product)
                .Include(x => x.Material)
                .OrderBy(x => x.BomId)
                .ToPagedListAsync(pageNumber, pageSize);
        }
    }
}
