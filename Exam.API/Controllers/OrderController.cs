using Exam.API.Services;
using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderController> _logger;
        private readonly IEmailService _emailService;
        
        public OrderController(IOrderService service, ILogger<OrderController> logger, IEmailService emailService)
        {
            _service = service;
            _logger = logger;
            _emailService = emailService;
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            string? keyword,
            string? status,
            int pageNumber = 1,
            int pageSize = 10)
        {
            _logger.LogInformation("呼叫查詢訂單 API");
            pageSize = Math.Min(pageSize, 50);

            var result = await _service.GetPagedAsync(
                keyword,
                status,
                pageNumber,
                pageSize);

            return Ok(new
            {
                result = true,
                message = "查詢成功",
                result.PageNumber,
                result.PageSize,
                result.TotalItemCount,
                result.PageCount,
                Data = result
            });
        }
        
        [Authorize(Roles = "客戶,員工,管理者")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            _logger.LogInformation("呼叫建立訂單 API");
            var result = await _service.CreateOrderAsync(dto, User);
            
            if (!string.IsNullOrEmpty(result)) 
            {
                dto.Result = true;
                dto.Message = "建立訂單成功";
                await _emailService.SendAsync();
                return Ok(dto);
            }
         
            dto.Result = false;
            dto.Message = "建立訂單失敗";
            
            return BadRequest(dto);
            
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, OrderStatus status)
        {
            _logger.LogInformation("呼叫更新訂單 API");
            var Result = await _service.UpdateOrderStatusAsync(id, status, User);
            if (Result == "狀態更新成功")
            {
                return Ok(new
                {
                    OrderId = id,
                    result = true,
                    message = Result
                });
            }

            return BadRequest(new
            {
                OrderId = id,
                result = false,
                message = Result
            });
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            _logger.LogInformation("呼叫取消訂單 API");
            var result = await _service.CancelOrderAsync(id);
            if (result == "訂單取消成功")
            {
                return Ok(new
                {
                    OrderId = id,
                    result = true,
                    message = result
                });
            }

            return BadRequest(new
            {
                OrderId = id,
                result = false,
                message = result
            });
        }

        
    }
}
