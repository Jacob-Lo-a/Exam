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
    public interface IOrderDetailService
    {
        Task<string> CreateAsync(CreateOrderDetailDto dto, ClaimsPrincipal user);
        Task<string> UpdateAsync(UpdateOrderDetailDto dto, ClaimsPrincipal user);
        Task<IPagedList<OrderDetailsDto>> GetPagedAsync(
            string? orderId,
            int pageNumber,
            int pageSize);
    }
}
