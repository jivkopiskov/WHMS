namespace WHMS.Data.Models
{
    using WHMS.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public string StreetAddress { get; set; }

        public string StreetAddress2 { get; set; }

        public string ZIP { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
