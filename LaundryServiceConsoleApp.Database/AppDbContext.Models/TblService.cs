using System;
using System.Collections.Generic;

namespace LaundryServiceConsoleApp.Database.AppDbContext.Models;

public partial class TblService
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal PricePerKg { get; set; }

    public bool DeleteFlag { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}
