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
        IQueryable<Order> GetQuery(string? keyword, string? status);
    }
}
