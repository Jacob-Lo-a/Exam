using Exam.API.Services;
using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            string? keyword,
            int pageNumber = 1,
            int pageSize = 10)
        {
            _logger.LogInformation("呼叫查詢產品 API");
            pageSize = Math.Min(pageSize, 50);

            var result = await _service.GetPagedAsync(keyword, pageNumber, pageSize);

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
        [Authorize(Roles = "員工,管理者")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            _logger.LogInformation("呼叫建立產品 API");
            var result = await _service.CreateProductAsync(dto, User);

            if (result != "建立成功")
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
        [HttpPut()]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            _logger.LogInformation("呼叫更新產品 API");
            var result = await _service.UpdateProductAsync(dto, User);

            if (result != "更新成功")
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("呼叫刪除產品 API");
            var result = await _service.DeleteProductAsync(id);

            if (result != "刪除成功")
                return BadRequest(new
                {
                    result = false,
                    message = result
                });

            return Ok(new
            {
                result = true,
                message = result
            });
        }
        
    }
}
