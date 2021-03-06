using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GasB360_server.Models
{
    public partial class TblBranch
    {
        public TblBranch()
        {
            TblFilledProducts = new HashSet<TblFilledProduct>();
            TblUnfilledProducts = new HashSet<TblUnfilledProduct>();
        }

        public Guid BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchLocation { get; set; }
        public string? Active { get; set; }
        public Guid? AdminId { get; set; }

        public virtual TblEmployee? Admin { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblFilledProduct>? TblFilledProducts { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblUnfilledProduct>? TblUnfilledProducts { get; set; }
    }
}
