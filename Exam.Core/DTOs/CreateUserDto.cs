using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class CreateUserDto : BaseResponse
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string CreatedBy {  get; set; } = null!;
    }
}
