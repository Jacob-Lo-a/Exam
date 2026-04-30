using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderWithDetailsAsync(string orderId);
        Task AddOrderAsync(Order order);
        Task<Order?> GetByIdAsync(string orderId);
        Task<IPagedList<Order>> GetPagedAsync(
            string? keyword,
            string? status,
            int pageNumber,
            int pageSize);
    }
}
