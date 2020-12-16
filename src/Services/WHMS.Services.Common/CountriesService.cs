namespace WHMS.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CountriesService : ICountriesService
    {
        public IEnumerable<SelectListItem> GetAllCountries()
        {
            List<string> list = new List<string>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures |
                        CultureTypes.SpecificCultures);
            foreach (CultureInfo cultureInfo in cultures)
            {
                if (cultureInfo.IsNeutralCulture || cultureInfo.LCID == 127)
                {
                    continue;
                }

                RegionInfo regionInfo = new RegionInfo(cultureInfo.Name);
                if (!list.Contains(regionInfo.EnglishName))
                {
                    list.Add(regionInfo.EnglishName);
                }
            }

            list.Sort();

            return list.Select(x => new SelectListItem { Text = x, Value = x });
        }
    }
}
