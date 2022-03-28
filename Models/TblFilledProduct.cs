using System;
using System.Collections.Generic;

namespace GasB360_server.Models
{
    public partial class TblFilledProduct
    {
        public TblFilledProduct()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        public Guid FilledProductId { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public int? FilledProductQuantity { get; set; }
        public string? Active { get; set; }
        public Guid? BranchId { get; set; }

        public virtual TblBranch? Branch { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
        public virtual ICollection<TblOrder>? TblOrders { get; set; }
    }
}
