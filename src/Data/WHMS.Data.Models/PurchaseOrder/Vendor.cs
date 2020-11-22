namespace WHMS.Data.Models.PurchaseOrder
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class Vendor : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public Address Address { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new HashSet<PurchaseOrder>();

        public ICollection<VendorProduct> VendorProducts { get; set; } = new HashSet<VendorProduct>();
    }
}
