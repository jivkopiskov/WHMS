namespace WHMS.Web.ViewModels.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Web.ViewModels.ValidationAttributes;

    public class AddProductToOrderInputModel
    {
        [ValidProduct]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        [ValidOrder]
        public int OrderId { get; set; }
    }
}
