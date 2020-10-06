﻿namespace WHMS.Data.Models.Orders.Enum
{
    public enum PaymentStatus
    {
        NoPayment = 0,
        PartiallyPaid = 1,
        FullyCharged = 2,
        PartiallyRefunded = 3,
        FullyRefunded = 4,
    }
}
