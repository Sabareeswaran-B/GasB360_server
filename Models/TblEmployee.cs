using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblEmployee
    {
        public TblEmployee()
        {
            TblBranches = new HashSet<TblBranch>();
            TblOrders = new HashSet<TblOrder>();
        }

        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public Guid? RoleId { get; set; }
        public string? Active { get; set; }
        public string? EmployeePhone { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? Password { get; set; }

        public virtual TblRole? Role { get; set; }
        public virtual ICollection<TblBranch>? TblBranches { get; set; }
        public virtual ICollection<TblOrder>? TblOrders { get; set; }
    }
}
