using System;
using System.Collections.Generic;

namespace TimeTrackAPI.Models;

public partial class Shift
{
    public int ShiftId { get; set; }

    public DateOnly ShiftDate { get; set; }

    public TimeOnly ShiftStartTime { get; set; }

    public TimeOnly ShiftEndTime { get; set; }

    public bool ShiftIsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? LocationId { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Location? Location { get; set; }
}
