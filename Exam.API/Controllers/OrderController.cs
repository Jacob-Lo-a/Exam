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
        private readonly IEmailService _emailService;
        
        public OrderController(IOrderService service, IEmailService emailService)
        {
            _service = service;
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
          
            var result = await _service.CreateOrderAsync(dto, User);
            
            
            return Ok(result);
            
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, OrderStatus status)
        {
            
            var result = await _service.UpdateOrderStatusAsync(id, status, User);
            return Ok(result);
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
      
            var result = await _service.CancelOrderAsync(id);
            return Ok(result);
        }

        
    }
}
