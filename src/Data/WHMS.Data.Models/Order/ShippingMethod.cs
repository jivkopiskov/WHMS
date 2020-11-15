namespace WHMS.Data.Models.Orders
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class ShippingMethod : BaseDeletableModel<int>
    {
        public int CarrierId { get; set; }

        public Carrier Carrier { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
