using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IMaterialRepository
    {
        Task<string?> GetMaxMaterialIdAsync();
        Task<Material?> GetByIdAsync(string materialId);

        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task DeleteAsync(Material material);

        Task<bool> ExistsInBomAsync(string materialId);
        Task<IPagedList<Material>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
