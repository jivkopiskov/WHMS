namespace WHMS.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models;
    using WHMS.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>, IMapTo<Address>
    {
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Display(Name = "Street Address 2")]
        public string StreetAddress2 { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public override string ToString()
        {
            return $"{this.StreetAddress},{this.StreetAddress2},{this.Zip},{this.Country}";
        }
    }
}
