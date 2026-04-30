using System;
using System.Collections.Generic;

namespace Exam.Core.Models;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Bom> Boms { get; set; } = new List<Bom>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
