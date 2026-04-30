using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace Exam.API.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ExamDbContext _context;

        public MaterialRepository(ExamDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetMaxMaterialIdAsync()
        {
            return await _context.Materials
                .OrderByDescending(x => x.MaterialId)
                .Select(x => x.MaterialId)
                .FirstOrDefaultAsync();
        }

        public async Task<Material?> GetByIdAsync(string materialId)
        {
            return await _context.Materials.FindAsync(materialId);
        }

        public async Task AddAsync(Material material)
        {
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Material material)
        {
            _context.Materials.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Material material)
        {
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsInBomAsync(string materialId)
        {
            return await _context.Boms
                .AnyAsync(x => x.MaterialId == materialId);
        }

        public async Task<IPagedList<Material>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Materials
                .OrderBy(x => x.MaterialId)
                .ToPagedListAsync(pageNumber, pageSize);
        }
    }
}
