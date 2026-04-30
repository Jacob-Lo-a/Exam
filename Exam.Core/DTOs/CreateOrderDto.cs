using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class CreateOrderDto : BaseResponse
    {
        public string OrderTitle { get; set; } = null!;
        public string Applicant { get; set; } = null!;
        public List<OrderItemDto> Items { get; set; } = new();
    }
    
}
