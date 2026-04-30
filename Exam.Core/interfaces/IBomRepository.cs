using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IBomRepository
    {
        Task<bool> ExistsAsync(string productId, string materialId);
        Task<Bom?> GetByIdAsync(int bomId);

        Task AddAsync(Bom bom);
        Task UpdateAsync(Bom bom);
        Task DeleteAsync(Bom bom);

        Task<bool> ProductHasOrderAsync(string productId);
        Task<IPagedList<Bom>> GetPagedAsync(int pageNumber, int pageSize);

    }
}
