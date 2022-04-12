using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GasB360_server.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblCustomers = new HashSet<TblCustomer>();
            TblEmployees = new HashSet<TblEmployee>();
        }

        public Guid RoleId { get; set; }
        public string? RoleType { get; set; }
        public string? Active { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblCustomer>? TblCustomers { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblEmployee>? TblEmployees { get; set; }
    }
}
