using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblAddress
    {
        public Guid AddressId { get; set; }
        public string? AddressStreetName { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressCountry { get; set; }
        public string? AddressPincode { get; set; }
        public string? Active { get; set; }
        public Guid? CustomerId { get; set; }

        public virtual TblCustomer? Customer { get; set; }
    }
}
