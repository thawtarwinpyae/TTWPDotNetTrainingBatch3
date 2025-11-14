using System;
using System.Collections.Generic;

namespace LaundryServiceConsoleApp.Database.AppDbContext.Models;

public partial class TblOrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public decimal Weight { get; set; }

    public decimal SubTotal { get; set; }

    public bool DeleteFlag { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}
