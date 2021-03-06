﻿namespace WHMS.Data.Models.Orders
{
    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Products;

    public class OrderItem : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Qty { get; set; }

        public decimal Price { get; set; }
    }
}
