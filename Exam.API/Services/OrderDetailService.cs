using Exam.API.Controllers;
using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.EF;

namespace Exam.API.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _detailRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly ExamDbContext _context;
        private readonly ILogger<OrderDetailService> _logger;
        public OrderDetailService(
            IOrderDetailRepository detailRepo,
            IOrderRepository orderRepo,
            ExamDbContext context,
            ILogger<OrderDetailService> logger)
        {
            _detailRepo = detailRepo;
            _orderRepo = orderRepo;
            _context = context;
            _logger = logger;
        }

        
        public async Task<string> CreateAsync(CreateOrderDetailDto dto, ClaimsPrincipal user)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _orderRepo.GetOrderWithDetailsAsync(dto.OrderId);
                if (order == null)
                    return "訂單不存在";

                if (order.Status != "成立")
                    return "只有成立狀態才能新增明細";

                if (dto.Quantity <= 0)
                    return "數量必須大於0";

                // 防止同產品重複
                if (order.OrderDetails.Any(x => x.ProductId == dto.ProductId))
                    return "同一訂單不可重複產品";

                var detail = new OrderDetail
                {
                    OrderId = dto.OrderId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.FindFirst(ClaimTypes.Name)?.Value
                };

                await _detailRepo.AddOrderDetailAsync(detail);

                await _context.SaveChangesAsync();   
                await tx.CommitAsync();
                _logger.LogInformation($"OrderId:{dto.OrderId} 訂單明細新增成功");
                return "新增成功";
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, $"OrderId:{dto.OrderId} 訂單明細新增失敗");
                return ex.Message;
            }
        }

        
        public async Task<string> UpdateAsync(UpdateOrderDetailDto dto, ClaimsPrincipal user)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var detail = await _detailRepo.GetByIdAsync(dto.DetailId);
                if (detail == null)
                    return "明細不存在";

                var order = await _orderRepo.GetOrderWithDetailsAsync(detail.OrderId);

                if (order!.Status != "成立")
                    return "只有成立狀態才能修改明細";

                if (dto.Quantity <= 0)
                    return "數量必須大於0";

                detail.Quantity = dto.Quantity;
                detail.UpdatedDate = DateTime.Now;
                detail.UpdatedBy = user.FindFirst(ClaimTypes.Name)?.Value;

                _detailRepo.Update(detail);

                await _context.SaveChangesAsync();   
                await tx.CommitAsync();
                _logger.LogInformation($"DetailId:{dto.DetailId} 訂單明細修改成功");
                return "修改成功";
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, $"DetailId:{dto.DetailId} 訂單明細新增失敗");
                return ex.Message;
            }
        }
        public async Task<IPagedList<OrderDetailsDto>> GetPagedAsync(
            string? orderId,
            int pageNumber,
            int pageSize)
        {
            try
            {
                var query = _detailRepo.GetQuery(orderId);

                var pagedData = await query
                    .OrderByDescending(x => x.CreatedDate)
                    .Select(x => new OrderDetailsDto
                    {
                        Id = x.DetailId,
                        OrderId = x.OrderId,
                        ProductId = x.ProductId,
                        ProductName = x.Product.ProductName,
                        Quantity = x.Quantity,
                        CreatedDate = (DateTime)x.CreatedDate
                    })
                    .ToPagedListAsync(pageNumber, pageSize);
                _logger.LogInformation("訂單明細查詢成功");
                return pagedData;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "訂單明細查詢失敗");
                throw;
            }
        }
    }
}
