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
    
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _service;
        private readonly ILogger<MaterialController> _logger;
        public MaterialController(IMaterialService service, ILogger<MaterialController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [Authorize(Roles = "員工,管理者")]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
        int pageNumber = 1,
        int pageSize = 10)
        {
            _logger.LogInformation("呼叫查詢物料 API");
            var result = await _service.GetPagedAsync(pageNumber, pageSize);

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
        public async Task<IActionResult> Create(CreateMaterialDto dto)
        {
            _logger.LogInformation("呼叫建立物料 API");
            var result = await _service.CreateAsync(dto, User);

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
        public async Task<IActionResult> Update(UpdateMaterialDto dto)
        {
            _logger.LogInformation("呼叫更新物料 API");
            var result = await _service.UpdateAsync(dto, User);
            
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
            _logger.LogInformation("呼叫刪除物料 API");
            var result = await _service.DeleteAsync(id);

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
