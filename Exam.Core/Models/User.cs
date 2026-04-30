using System;
using System.Collections.Generic;

namespace Exam.Core.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }
}
