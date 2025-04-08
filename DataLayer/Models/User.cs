using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class User
{
    [JsonIgnore]
    public int? UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPasswordHash { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserFirstName { get; set; } = null!;

    public string UserLastName { get; set; } = null!;
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    public bool? IsActive { get; set; }
    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();
    [JsonIgnore]
    public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
    [JsonIgnore]
    public virtual ICollection<Salary> ? Salaries { get; set; } = new List<Salary>();
}
