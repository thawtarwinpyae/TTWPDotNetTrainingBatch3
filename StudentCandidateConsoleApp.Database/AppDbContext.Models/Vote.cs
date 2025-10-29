using System;
using System.Collections.Generic;

namespace StudentCandidateConsoleApp.Database.AppDbContext.Models;

public partial class Vote
{
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? CandidateId { get; set; }

    public DateTime? VotedAt { get; set; }

    public virtual Candidate? Candidate { get; set; }

    public virtual Student? Student { get; set; }
}
