using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.DTOs
{
    public class UpdateMaterialDto : BaseResponse
    {
        public string MaterialId { get; set; } = null!;
        public string MaterialName { get; set; } = null!;
        public decimal Cost { get; set; }
        public int Stock { get; set; }
    }
}
