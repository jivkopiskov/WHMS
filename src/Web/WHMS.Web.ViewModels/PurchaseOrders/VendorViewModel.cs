namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using WHMS.Data;
    using WHMS.Data.Models.PurchaseOrder;
    using WHMS.Services.Mapping;

    public class VendorViewModel : IMapFrom<Vendor>, IMapTo<Vendor>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public AddressViewModel Address { get; set; }

        public IEnumerable<PurchaseOrderSummaryViewModel> PurchaseOrders { get; set; }
    }
}
