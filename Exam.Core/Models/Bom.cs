using System;
using System.Collections.Generic;

namespace Exam.Core.Models;

public partial class Bom
{
    public int BomId { get; set; }

    public string ProductId { get; set; } = null!;

    public string MaterialId { get; set; } = null!;

    public int? Quantity { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
