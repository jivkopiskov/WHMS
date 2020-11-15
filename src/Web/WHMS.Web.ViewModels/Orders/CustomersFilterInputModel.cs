namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class CustomersFilterInputModel
    {
        public int Page { get; set; } = 1;

        public CustomerSorting Sorting { get; set; } = CustomerSorting.Id;

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ZipCode { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(this.Email))
            {
                dict["email"] = this.Email;
            }

            if (!string.IsNullOrEmpty(this.PhoneNumber))
            {
                dict["phoneNumber"] = this.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(this.ZipCode))
            {
                dict["zipCode"] = this.ZipCode;
            }

            return dict;
        }
    }
}
