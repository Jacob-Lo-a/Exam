using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class UpdateOrderDetailDto : BaseResponse
    {
        public int DetailId { get; set; }   // 明細流水號
        public int Quantity { get; set; }
    }
}
