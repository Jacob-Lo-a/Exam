using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Exam.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly PasswordHasher<User> _hasher;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository repo, JwtService jwtService)
        {
            _repo = repo;
            _hasher = new PasswordHasher<User>();
            _jwtService = jwtService;
           
        }

        public async Task<string> CreateUserAsync(CreateUserDto dto)
        {
            
            if (await _repo.ExistsByAccountAsync(dto.Account))
                return "帳號已存在";

            
            var validRoles = new[] { "客戶", "員工", "管理者" };
            if (!validRoles.Contains(dto.Role))
                return "角色錯誤";

        
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

            return "建立成功";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            
            var user = await _repo.GetByAccountAsync(dto.Account);
            if (user == null)
                return "帳號或密碼錯誤";

            
            var result = _hasher.VerifyHashedPassword(
                user,
                user.Password,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return "帳號或密碼錯誤";

            var token = _jwtService.GenerateToken(user);

            return token;
        }

        public async Task<string> UpdateUserAsync(UpdateUserDto dto)
        {
           
            var user = await _repo.GetByIdAsync(dto.UserId);
            if (user == null)
                return "使用者不存在";
         
       
            var validRoles = new[] { "客戶", "員工", "管理者" };
            if (!validRoles.Contains(dto.Role))
                return "角色錯誤";

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.UpdatedBy = dto.UpdateBy;
            user.UpdatedDate = DateTime.Now;

            await _repo.UpdateAsync(user);

            return "更新成功";
        }
    }
}
