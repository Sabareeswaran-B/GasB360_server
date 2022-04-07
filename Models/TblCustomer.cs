using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblAddresses = new HashSet<TblAddress>();
            TblOrders = new HashSet<TblOrder>();
        }

        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? Password { get; set; }
        public Guid? RoleId { get; set; }
        public string? Active { get; set; }
        public Guid? TypeId { get; set; }
        public int? CustomerConnection { get; set; }
        public int? AllowedLimit { get; set; }
        public string? Requested { get; set; }
        public string? CustomerImage { get; set; }

        public virtual TblRole? Role { get; set; }
        public virtual TblType? Type { get; set; }
        public virtual ICollection<TblAddress>? TblAddresses { get; set; }
        public virtual ICollection<TblOrder>? TblOrders { get; set; }
    }
}
