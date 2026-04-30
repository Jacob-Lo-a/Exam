using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class CreateOrderDetailDto : BaseResponse
    {
        public string OrderId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }
    }

    
}
