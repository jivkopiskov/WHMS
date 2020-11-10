namespace WHMS.Services.Common
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICountriesService
    {
        IEnumerable<SelectListItem> GetAllCountries();
    }
}
