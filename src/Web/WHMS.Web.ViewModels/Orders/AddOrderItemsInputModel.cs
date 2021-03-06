﻿namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using WHMS.Web.ViewModels.ValidationAttributes;

    public class AddOrderItemsInputModel
    {
        [ValidOrder]
        public int OrderId { get; set; }

        public ICollection<AddOrderItemViewModel> OrderItems { get; set; }
    }
}
