using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblType
    {
        public TblType()
        {
            TblCustomers = new HashSet<TblCustomer>();
            TblProductCategories = new HashSet<TblProductCategory>();
        }

        public Guid TypeId { get; set; }
        public string? TypeName { get; set; }
        public string? Active { get; set; }

        public virtual ICollection<TblCustomer>? TblCustomers { get; set; }
        public virtual ICollection<TblProductCategory>? TblProductCategories { get; set; }
    }
}
