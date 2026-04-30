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
        private readonly ILogger<OrderDetailsController> _logger;
        public OrderDetailsController(IOrderDetailService service, ILogger<OrderDetailsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
        string? orderId,
        int pageNumber = 1,
        int pageSize = 10)
        {
            _logger.LogInformation("呼叫查詢訂單明細 API");
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
            _logger.LogInformation("呼叫建立訂單明細 API");

            var result = await _service.CreateAsync(dto, User);

            if (result != "新增成功")
            {
                dto.Result = false;
                dto.Message = result;
                return BadRequest(dto);
            }
            dto.Result = true;
            dto.Message = result;
            return Ok(dto);
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrderDetailDto dto)
        {
            _logger.LogInformation("呼叫更新訂單明細 API");
            var result = await _service.UpdateAsync(dto, User);

            if (result != "修改成功")
            {
                dto.Result = false;
                dto.Message = result;
                return BadRequest(dto);
            }

            dto.Result = true;
            dto.Message = result;
            return Ok(dto);
        }
    }
}
