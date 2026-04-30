using Exam.Core.interfaces;
using Exam.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace Exam.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExamDbContext _context;

        public UserRepository(ExamDbContext context) 
        { 
          _context = context;
        }
        public async Task<bool> ExistsByAccountAsync(string account)
        {
            return await _context.Users.AnyAsync(x => x.Account == account);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByAccountAsync(string account)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Account == account);
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
