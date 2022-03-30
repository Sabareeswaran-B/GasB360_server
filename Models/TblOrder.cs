using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblDeliveries = new HashSet<TblDelivery>();
        }

        public Guid OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? Active { get; set; }
        public int? OrderOtp { get; set; }
        public int? OrderTotalprice { get; set; }
        public Guid? FilledProductId { get; set; }
        public Guid? AddressId { get; set; }
        public string? OrderStatus { get; set; }

        public virtual TblAddress? Address { get; set; }
        public virtual TblCustomer? Customer { get; set; }
        public virtual TblEmployee? Employee { get; set; }
        public virtual TblFilledProduct? FilledProduct { get; set; }
        public virtual ICollection<TblDelivery>? TblDeliveries { get; set; }
    }
}
