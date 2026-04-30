using Exam.Core.DTOs;
using System.Security.Claims;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IBomService
    {
        Task<string> CreateAsync(CreateBomDto dto, ClaimsPrincipal user);
        Task<string> UpdateAsync(UpdateBomDto dto, ClaimsPrincipal user);
        Task<string> DeleteAsync(int bomId);
        Task<IPagedList<BomDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
