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
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(CreateOrderDto dto, ClaimsPrincipal user);
        Task<string> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus, ClaimsPrincipal user);
        Task<string> CancelOrderAsync(string orderId);
        Task<IPagedList<OrderDto>> GetPagedAsync(
            string? keyword,
            string? status,
            int pageNumber,
            int pageSize);
    }
}
