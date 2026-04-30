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
    public interface IProductService
    {
        Task<string> CreateProductAsync(CreateProductDto dto, ClaimsPrincipal user);
        Task<string> UpdateProductAsync(UpdateProductDto dto, ClaimsPrincipal user);
        Task<string> DeleteProductAsync(string productId);
        Task<IPagedList<ProductDto>> GetPagedAsync(string? keyword, int pageNumber, int pageSize);
    }
}
