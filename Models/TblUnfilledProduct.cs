using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblUnfilledProduct
    {
        public Guid UnfilledProductId { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public int? UnfilledProductQuantity { get; set; }
        public string? Active { get; set; }
        public Guid? BranchId { get; set; }

        public virtual TblBranch? Branch { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
    }
}
