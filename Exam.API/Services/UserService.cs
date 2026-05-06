using Exam.API.Controllers;
using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using System.Security.Claims;

namespace Exam.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly PasswordHasher<User> _hasher;
        private readonly JwtService _jwtService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repo, JwtService jwtService, ILogger<UserService> logger)
        {
            _repo = repo;
            _hasher = new PasswordHasher<User>();
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                _logger.LogInformation("開始建立使用者");
                if (await _repo.ExistsByAccountAsync(dto.Account))
                {
                    return "帳號已存在";
                }

                var validRoles = new[] { "客戶", "員工", "管理者" };
                if (!validRoles.Contains(dto.Role))
                {
                    return "角色錯誤";
                }


                var user = new User
                {
                    Account = dto.Account,
                    UserName = dto.UserName,
                    Email = dto.Email,
                    Role = dto.Role,
                    CreatedBy = dto.CreatedBy
                };


                user.Password = _hasher.HashPassword(user, dto.Password);

                await _repo.AddAsync(user);

                _logger.LogInformation("建立使用者成功");
                return "建立成功";
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "建立使用者失敗");
                throw;
            }

            
        }

        public async Task<LoginDto> LoginAsync(LoginDto dto)
        {
            try
            {
               
                var user = await _repo.GetByAccountAsync(dto.Account);

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "使用者資料不存在");
                }
                else
                {
                    var result = _hasher.VerifyHashedPassword(
                        user,
                        user.Password,
                        dto.Password
                    );

                    if (result == PasswordVerificationResult.Failed)
                    {
                        dto.Message = "密碼錯誤";
                        dto.Result = false;
                        dto.Jwt = "";
                    }
                    else
                    {
                        var token = _jwtService.GenerateToken(user);
                        dto.Result = true;
                        dto.Message = "登入成功";
                        dto.Jwt = token;
                        _logger.LogInformation("使用者登入成功");
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "使用者登入失敗");
                throw;
            }

            

        }

        public async Task<string> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _repo.GetByIdAsync(dto.UserId);
                if (user == null)
                {
                    return "使用者資料不存在";
                }

                var validRoles = new[] { "客戶", "員工", "管理者" };
                if (!validRoles.Contains(dto.Role))
                {
                    return "角色錯誤";
                }

                user.UserName = dto.UserName;
                user.Email = dto.Email;
                user.Role = dto.Role;
                user.UpdatedBy = dto.UpdateBy;
                user.UpdatedDate = DateTime.Now;

                await _repo.UpdateAsync(user);
                _logger.LogInformation("使用者資料更新成功");

                return "更新成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "使用者資料更新失敗");
                throw;
            }

            
        }
    }
}
