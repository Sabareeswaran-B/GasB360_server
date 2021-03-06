using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GasB360_server.Models
{
    public partial class TblProductCategory
    {
        public TblProductCategory()
        {
            TblFilledProducts = new HashSet<TblFilledProduct>();
            TblUnfilledProducts = new HashSet<TblUnfilledProduct>();
        }

        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? ProductWeight { get; set; }
        public int? ProductPrice { get; set; }
        public Guid? TypeId { get; set; }
        public string? Active { get; set; }
        public string? ProductImage { get; set; }

        public virtual TblType? Type { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblFilledProduct>? TblFilledProducts { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblUnfilledProduct>? TblUnfilledProducts { get; set; }
    }
}
