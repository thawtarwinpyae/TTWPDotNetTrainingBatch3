using System;
using System.Collections.Generic;

namespace LaundryServiceConsoleApp.Database.AppDbContext.Models;

public partial class TblOrder
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}
