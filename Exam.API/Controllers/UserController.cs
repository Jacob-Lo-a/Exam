using Exam.API.Services;
using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize(Roles = "管理者")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            _logger.LogInformation("呼叫建立使用者 API");
            var result = await _service.CreateUserAsync(dto);

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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            _logger.LogInformation("呼叫使用者登入 API");
            var result = await _service.LoginAsync(dto);

            if (result == "帳號或密碼錯誤")
            {
                dto.Result = false;
                dto.Message = result;
                return Unauthorized(new
                {
                    result = dto.Result,
                    message = dto.Message
                });
            }
            
            dto.Result = true;
            dto.Message = "登入成功";
            
            return Ok(new 
            { 
                result = dto.Result, 
                message = dto.Message,
                token = result
            });
        }

        [Authorize(Roles = "員工,管理者")]
        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            _logger.LogInformation("呼叫更新使用者資料 API");
            var result = await _service.UpdateUserAsync(dto);

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
    }
}
