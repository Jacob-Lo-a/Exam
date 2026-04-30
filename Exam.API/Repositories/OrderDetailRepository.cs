using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using X.PagedList;
using X.PagedList.EF;

namespace Exam.API.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ExamDbContext _context;

        public OrderDetailRepository(ExamDbContext context)
        {
            _context = context;
        }
        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }
        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public void Update(OrderDetail detail)
        {
            _context.OrderDetails.Update(detail);
        }

        public async Task<IPagedList<OrderDetail>> GetPagedAsync(
            string? orderId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.OrderDetails
                .Include(x => x.Product)
                .AsQueryable();

           
            if (!string.IsNullOrEmpty(orderId))
            {
                query = query.Where(x => x.OrderId == orderId);
            }

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedListAsync(pageNumber, pageSize);
        }
    }
}
