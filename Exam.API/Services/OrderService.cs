using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using X.PagedList;

namespace Exam.API.Services
{
    
    public class OrderService : IOrderService
    {
        private readonly ExamDbContext _context;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly ILogger<OrderService> _logger;
        private readonly IEmailService _emailService;
        public OrderService(ExamDbContext context, IOrderRepository orderRepo, IOrderDetailRepository orderDetailRepo, ILogger<OrderService> logger, IEmailService emailService) 
        {
            _context = context;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _logger = logger;
            _emailService = emailService;
        }
        public async Task<string> CreateOrderAsync(CreateOrderDto dto, ClaimsPrincipal user)
        {
            var orderId = await GenerateOrderIdAsync();

            var order = new Order
            {
                OrderId = orderId,
                OrderTitle = dto.OrderTitle,
                Applicant = dto.Applicant,
                Status = OrderStatus.成立.ToString(),
                CreatedBy = user.Identity?.Name
            };
            await _orderRepo.AddOrderAsync(order);

           
      

            foreach (var item in dto.Items)
            {
                await _orderDetailRepo.AddOrderDetailAsync(new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CreatedBy = user.Identity?.Name

                });
            }

            return orderId;
        }

        public async Task<string> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus, ClaimsPrincipal user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("開始修改訂單狀態 OrderId={OrderId}, Status={Status}", orderId, newStatus);

                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(x => x.OrderId == orderId);

                if (order == null)
                {
                    _logger.LogWarning("訂單不存在 OrderId={OrderId}", orderId);
                    return "訂單不存在";
                }
                _logger.LogDebug("目前訂單狀態 {Status}", order.Status);

                // 只能從成立 到 生產
                if (order.Status == OrderStatus.成立.ToString() &&
                    newStatus == OrderStatus.生產)
                {
                    // 檢查庫存
                    foreach (var item in order.OrderDetails)
                    {
                        _logger.LogDebug("檢查產品 {ProductId} 數量 {Qty}", item.ProductId, item.Quantity);

                        var boms = await _context.Boms
                            .Where(x => x.ProductId == item.ProductId)
                            .ToListAsync();

                        foreach (var bom in boms)
                        {
                            var material = await _context.Materials
                                .FirstAsync(x => x.MaterialId == bom.MaterialId);

                            var needQty = bom.Quantity * item.Quantity;

                            _logger.LogDebug("物料 {MaterialId} 需要 {Need}, 庫存 {Stock}",
                                bom.MaterialId, needQty, material!.Stock);

                            if (material.Stock < needQty)
                            {
                                _logger.LogError("庫存不足 MaterialId={MaterialId}", bom.MaterialId);
                                throw new Exception($"物料 {material.MaterialId} 庫存不足");
                            }
                        }
                    }
                    _logger.LogInformation("開始扣庫存 OrderId={OrderId}", orderId);
                    
                    //  扣庫存 
                    foreach (var item in order.OrderDetails)
                    {
                        var boms = await _context.Boms
                            .Where(x => x.ProductId == item.ProductId)
                            .ToListAsync();

                        foreach (var bom in boms)
                        {
                            var material = await _context.Materials
                                .FirstAsync(x => x.MaterialId == bom.MaterialId);

                            var needQty = bom.Quantity * item.Quantity;

                            material.Stock -= needQty;
                        }
                    }
                }

                order.Status = newStatus.ToString();
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = user.FindFirst(ClaimTypes.Name)?.Value;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("訂單成功更新 OrderId={OrderId}", orderId);
                return "狀態更新成功";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "訂單更新失敗 OrderId={OrderId}", orderId);
                return ex.Message;
            }
        }

        public async Task<string> CancelOrderAsync(string orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
                return "訂單不存在";

            if (order.Status == OrderStatus.完成.ToString())
                return "已完成訂單不可取消";

            order.Status = "取消";

            await _context.SaveChangesAsync();

            return "訂單取消成功";
        }

        public async Task<IPagedList<OrderDto>> GetPagedAsync(
            string? keyword,
            string? status,
            int pageNumber,
            int pageSize)
        {
            var data = await _orderRepo.GetPagedAsync(keyword, status, pageNumber, pageSize);

            var dtoList = data.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                OrderTitle = order.OrderTitle,
                Status = order.Status,
                Applicant = order.Applicant,
                CreatedDate = (DateTime)order.CreatedDate,

                Details = order.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product.ProductName,
                    Quantity = d.Quantity
                }).ToList()
            }).ToList();

            return new StaticPagedList<OrderDto>(
                dtoList,
                data.PageNumber,
                data.PageSize,
                data.TotalItemCount
            );
        }

        private async Task<string> GenerateOrderIdAsync()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");

            var lastOrder = await _context.Orders
                .Where(x => x.OrderId.StartsWith("O" + datePart))
                .OrderByDescending(x => x.OrderId)
                .Select(x => x.OrderId)
                .FirstOrDefaultAsync();

            int seq = 1;

            if (!string.IsNullOrEmpty(lastOrder))
            {
                var lastSeq = int.Parse(lastOrder.Substring(9));
                seq = lastSeq + 1;
            }

            return $"O{datePart}{seq.ToString("D5")}";
        }
    }

}
