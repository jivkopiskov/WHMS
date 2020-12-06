namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Web.ViewModels.ValidationAttributes;

    public class ProductAdjustmentInputModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid warehouse")]
        public int WarehouseId { get; set; }

        [ValidProduct]
        public int ProductId { get; set; }

        public int Qty { get; set; }
    }
}
