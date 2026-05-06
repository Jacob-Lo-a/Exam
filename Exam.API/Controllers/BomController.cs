using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class BomController : ControllerBase
    {
        private readonly IBomService _service;

        public BomController(IBomService service)
        {
            _service = service;
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
        int pageNumber = 1,
        int pageSize = 10)
        {
            
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
        public async Task<IActionResult> Create(CreateBomDto dto)
        {
           
            var result = await _service.CreateAsync(dto, User);
            
            return Ok(result);
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut()]
        public async Task<IActionResult> Update(UpdateBomDto dto)
        {
           
            var result = await _service.UpdateAsync(dto, User);
           
            return Ok(result);
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
        
            var result = await _service.DeleteAsync(id);
          
            return Ok(result);
        }
    }
}
