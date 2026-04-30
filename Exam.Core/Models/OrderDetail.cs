using System;
using System.Collections.Generic;

namespace Exam.Core.Models;

public partial class OrderDetail
{
    public int DetailId { get; set; }

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
