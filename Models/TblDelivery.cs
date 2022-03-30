using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblDelivery
    {
        public Guid DeliveryId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? Active { get; set; }
        public Guid? OrderId { get; set; }

        public virtual TblOrder? Order { get; set; }
    }
}
