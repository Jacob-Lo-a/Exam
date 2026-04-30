using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }

        public string OrderId { get; set; } = null!;

        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;

        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
