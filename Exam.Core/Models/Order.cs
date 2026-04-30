using System;
using System.Collections.Generic;

namespace Exam.Core.Models;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public string? OrderTitle { get; set; }

    public string? Applicant { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
