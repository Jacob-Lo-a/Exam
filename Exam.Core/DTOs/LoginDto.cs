using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class LoginDto : BaseResponse
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
