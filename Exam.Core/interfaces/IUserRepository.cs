using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByAccountAsync(string account);
        Task AddAsync(User user);
        Task<User?> GetByAccountAsync(string account);
        Task<User?> GetByIdAsync(int userId);
        Task UpdateAsync(User user);
    }
}
