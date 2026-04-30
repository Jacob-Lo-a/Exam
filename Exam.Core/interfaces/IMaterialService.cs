using Exam.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IMaterialService
    {
        Task<string> CreateAsync(CreateMaterialDto dto, ClaimsPrincipal user);
        Task<string> UpdateAsync(UpdateMaterialDto dto, ClaimsPrincipal user);
        Task<string> DeleteAsync(string materialId);
        Task<IPagedList<MaterialDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
