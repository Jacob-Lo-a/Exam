using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace Exam.API.Services
{
    public class OrderJobService : IOrderJobService
    {
        private readonly ExamDbContext _context;
        private readonly ILogger<OrderJobService> _logger;

        public OrderJobService(ExamDbContext context, ILogger<OrderJobService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessPendingOrdersAsync()
        {
            _logger.LogInformation("開始執行訂單排程");

            var orders = _context.Orders
                .Where(x => x.Status == "成立")
                .ToList();

            var exportList = new List<object>();
            _logger.LogInformation("成立訂單數量：{Count}", orders.Count);
            
            foreach (var order in orders)
            {
                try
                {
                    var orderData = new
                    {
                        order.OrderId,
                        order.OrderTitle,
                        order.Applicant,
                        order.CreatedDate,
                        Details = _context.OrderDetails
                            .Where(d => d.OrderId == order.OrderId)
                            .Select(d => new
                            {
                                d.ProductId,
                                d.Quantity
                            }).ToList()
                    };
                    exportList.Add(orderData);
                    // 更新狀態
                    order.Status = "生產";
                    order.UpdatedDate = DateTime.Now;
                                     
                    _logger.LogInformation("訂單匯出成功 OrderId={OrderId}", order.OrderId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "訂單匯出失敗 OrderId={OrderId}", order.OrderId);
                }
            }
            // 建立資料夾
            var folder = @"C:\Users\jacob lo\source\repos\Exam\order\out";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 檔名
            var fileName = $"order{DateTime.Now:yyyyMMddHHmmss}.json";
            var path = Path.Combine(folder, fileName);

            // 寫入 JSON
            var json = JsonSerializer.Serialize(exportList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            await File.WriteAllTextAsync(path, json, Encoding.UTF8);
            await _context.SaveChangesAsync();

            _logger.LogInformation("訂單排程完成");
        }
        public async Task ProductMaterialAsync()
        {
            _logger.LogInformation("開始執行產品明細排程");
            var data = await _context.Products
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.Price,

                    Materials = p.Boms.Select(b => new
                    {
                        b.MaterialId,
                        b.Quantity,
                        b.Material.MaterialName,
                        b.Material.Cost
                    }).ToList()
                })
                .ToListAsync();

            _logger.LogInformation("產品數量：{Count}", data.Count);
            // 建立資料夾
            var folder = @"C:\Users\jacob lo\source\repos\Exam\product\detail";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 檔名
            var fileName = $"order{DateTime.Now:yyyyMMddHHmmss}.json";
            var path = Path.Combine(folder, fileName);

            // 寫入 JSON
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            await File.WriteAllTextAsync(path, json, Encoding.UTF8);
            _logger.LogInformation("產品明細排程完成");
        }

        public async Task OrderDetailAsync()
        {
            _logger.LogInformation("開始執行訂單明細排程");
            var orders = _context.Orders.ToList();

            _logger.LogInformation("訂單數量：{Count}", orders.Count);

            var exportList = new List<object>();
            foreach (var order in orders)
            {

                var orderData = new
                {
                    order.OrderId,
                    order.OrderTitle,
                    order.Applicant,
                    order.CreatedDate,
                    Details = _context.OrderDetails
                                .Where(d => d.OrderId == order.OrderId)
                                .Select(d => new
                                {
                                    d.ProductId,
                                    d.Quantity,
                                    Material = _context.Boms
                                    .Where(p => p.ProductId == d.ProductId)
                                    .Select(p => new 
                                    {
                                        p.MaterialId,
                                        p.Quantity,
                                        p.Material.MaterialName,
                                        p.Material.Cost
                                    }).ToList()
                                }).ToList()
                };
                exportList.Add(orderData);
            }
            // 建立資料夾
            var folder = @"C:\Users\jacob lo\source\repos\Exam\order\detail";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 檔名
            var fileName = $"order{DateTime.Now:yyyyMMddHHmmss}.json";
            var path = Path.Combine(folder, fileName);

            // 寫入 JSON
            var json = JsonSerializer.Serialize(exportList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            await File.WriteAllTextAsync(path, json, Encoding.UTF8);
            _logger.LogInformation("訂單明細排程結束");
        }
    }
}
