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
  
        public UserController(IUserService service)
        {
            _service = service;
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            
            var result = await _service.CreateUserAsync(dto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            
            var result = await _service.LoginAsync(dto);
            
            return Ok(result);
        }

        [Authorize(Roles = "客戶,員工,管理者")]
        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
           
            var result = await _service.UpdateUserAsync(dto);
            
            return Ok(result);
        }
    }
}
