namespace WHMS.Data.Models.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class Warehouse : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public bool IsSellable { get; set; }

        public string StreetAdress { get; set; }

        public string ZIP { get; set; }

        public string Country { get; set; }

        public ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new HashSet<ProductWarehouse>();
    }
}
