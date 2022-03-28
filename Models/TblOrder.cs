using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblOrder
    {
        public Guid OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? Active { get; set; }
        public string? OrderStatus { get; set; }
        public int? OrderOtp { get; set; }
        public int? OrderTotalprice { get; set; }

        public virtual TblCustomer? Customer { get; set; }
        public virtual TblEmployee? Employee { get; set; }
        public virtual TblFilledProduct? Product { get; set; }
    }
}
