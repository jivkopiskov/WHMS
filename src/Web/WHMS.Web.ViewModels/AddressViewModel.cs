namespace WHMS.Web.ViewModels
{
    using WHMS.Data.Models;
    using WHMS.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>, IMapTo<Address>
    {
        public string StreetAddress { get; set; }

        public string StreetAddress2 { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public override string ToString()
        {
            return $"{this.StreetAddress},{this.StreetAddress2},{this.Zip},{this.Country}";
        }
    }
}
