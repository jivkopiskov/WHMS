namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.PurchaseOrders.Enum;

    public class PurchaseOrdersFilterModel : IFilter
    {
        public int Page { get; set; } = 1;

        public int? Id { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "SKU / Product ID")]
        public string SKU { get; set; }

        [Display(Name = "PO Status")]
        public PurchaseOrderStatus? PurchaseOrderStatus { get; set; }

        [Display(Name = "Receiving Status")]
        public ReceivingStatus? ReceivingStatus { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();

            if (this.Id != null)
            {
                dictionary["id"] = this.Id.ToString();
            }

            if (this.VendorId != null && this.VendorId != 0)
            {
                dictionary["vendorId"] = this.VendorId.ToString();
            }

            if (this.PurchaseOrderStatus != null)
            {
                dictionary["purchaseOrderStatus"] = ((int)this.PurchaseOrderStatus).ToString();
            }

            if (this.ReceivingStatus != null)
            {
                dictionary["receivingStatus"] = ((int)this.ReceivingStatus).ToString();
            }

            if (this.SKU != null && !string.IsNullOrEmpty(this.SKU))
            {
                dictionary["SKU"] = this.SKU;
            }

            return dictionary;
        }
    }
}
