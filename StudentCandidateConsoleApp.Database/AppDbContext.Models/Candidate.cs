using System;
using System.Collections.Generic;

namespace StudentCandidateConsoleApp.Database.AppDbContext.Models;

public partial class Candidate
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? VoteCount { get; set; }

    public bool DeleteFlag { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
