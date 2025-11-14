using System;
using System.Collections.Generic;

namespace LaundryServiceConsoleApp.Database.AppDbContext.Models;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool DeleteFlag { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}
