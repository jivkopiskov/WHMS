namespace WHMS.Data.Models.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Orders;

    public class Warehouse : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public bool IsSellable { get; set; }

        public Address Address { get; set; }

        public ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new HashSet<ProductWarehouse>();

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
