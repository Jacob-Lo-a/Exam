using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Exam.Core.interfaces
{
    public interface IOrderDetailRepository
    {
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task<OrderDetail?> GetByIdAsync(int id);
        void Update(OrderDetail detail);
        Task<IPagedList<OrderDetail>> GetPagedAsync(
            string? orderId,
            int pageNumber,
            int pageSize);
    }
}
