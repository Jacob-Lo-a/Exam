using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _service;
    
        public OrderDetailsController(IOrderDetailService service)
        {
            _service = service;
           
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
        string? orderId,
        int pageNumber = 1,
        int pageSize = 10)
        {
           
            pageSize = Math.Min(pageSize, 50);

            var result = await _service.GetPagedAsync(orderId, pageNumber, pageSize);

            return Ok(new
            {
                result.PageNumber,
                result.PageSize,
                result.TotalItemCount,
                result.PageCount,
                Data = result
            });
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDetailDto dto)
        {
      

            var result = await _service.CreateAsync(dto, User);

            return Ok(result);
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrderDetailDto dto)
        {
        
            var result = await _service.UpdateAsync(dto, User);

            return Ok(result);
        }
    }
}
