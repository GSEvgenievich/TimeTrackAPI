using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int ShiftId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? StatusId { get; set; }

    public virtual Shift Shift { get; set; } = null!;

    public virtual Status? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
