using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;
using X.PagedList.EF;

namespace Exam.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ExamDbContext _context;

        public OrderRepository(ExamDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderWithDetailsAsync(string orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(string orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<IPagedList<Order>> GetPagedAsync(
            string? keyword,
            string? status,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .AsQueryable();

            // 關鍵字搜尋（訂單編號 / 主旨）
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.OrderId.Contains(keyword) ||
                    x.OrderTitle.Contains(keyword));
            }

            //  狀態篩選
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedListAsync(pageNumber, pageSize);
        }
    }

}