using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class ProductDto
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
