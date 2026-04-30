using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class BaseResponse
    {
        public bool Result { get; set; }
        public string? Message { get; set; }
    }
}
