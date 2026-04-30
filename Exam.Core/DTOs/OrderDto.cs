using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class OrderDto
    {
        public string OrderId { get; set; } = null!;
        public string OrderTitle { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Applicant { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public List<OrderDetailDto> Details { get; set; } = new();
    }

    public class OrderDetailDto
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
