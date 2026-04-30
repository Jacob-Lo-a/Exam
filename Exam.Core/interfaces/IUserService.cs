using Exam.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(CreateUserDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<string> UpdateUserAsync(UpdateUserDto dto);
    }
}
