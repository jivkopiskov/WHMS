namespace WHMS.Services.Tests
{
    using System.Reflection;

    using AutoMapper;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels;

    public class BaseServiceTest
    {
        public BaseServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }
    }
}
