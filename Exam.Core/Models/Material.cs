using System;
using System.Collections.Generic;

namespace Exam.Core.Models;

public partial class Material
{
    public string MaterialId { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public decimal Cost { get; set; }

    public int? Stock { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Bom> Boms { get; set; } = new List<Bom>();
}
