using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class OrderItemDto
    {
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
