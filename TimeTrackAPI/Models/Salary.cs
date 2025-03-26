using System;
using System.Collections.Generic;

namespace TimeTrackAPI.Models;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int UserId { get; set; }

    public decimal SalaryAmount { get; set; }

    public DateOnly SalaryDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
