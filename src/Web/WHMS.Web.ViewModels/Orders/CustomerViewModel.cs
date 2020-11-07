namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;

    public class CustomerViewModel : IHaveCustomMappings, IMapTo<Customer>
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public AddressViewModel Address { get; set; }

        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Customer, CustomerViewModel>().ForMember(
                x => x.Address,
                opt => opt.MapFrom(a => a.Address));
        }
    }
}
