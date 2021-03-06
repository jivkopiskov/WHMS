﻿namespace WHMS.Data.Models.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class Customer : BaseDeletableModel<int>
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
