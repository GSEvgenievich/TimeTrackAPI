using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
