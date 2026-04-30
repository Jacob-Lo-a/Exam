using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class CreateBomDto : BaseResponse
    {
        public string ProductId { get; set; } = null!;
        public string MaterialId { get; set; } = null!;
        public int Quantity { get; set; } = 1;
    }
}
