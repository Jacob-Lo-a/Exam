using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class UpdateBomDto : BaseResponse
    {
        public int BomId { get; set; }
        public string MaterialId { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
