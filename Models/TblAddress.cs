using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GasB360_server.Models
{
    public partial class TblAddress
    {
        public TblAddress()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        public Guid AddressId { get; set; }
        public string? AddressStreetName { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressCountry { get; set; }
        public string? AddressPincode { get; set; }
        public string? Active { get; set; }
        public Guid? CustomerId { get; set; }

        [JsonIgnore]
        public virtual TblCustomer? Customer { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblOrder>? TblOrders { get; set; }
    }
}
