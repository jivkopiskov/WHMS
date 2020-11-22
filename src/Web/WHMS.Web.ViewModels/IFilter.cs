namespace WHMS.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IFilter
    {
        public int Page { get; set; }

        public abstract Dictionary<string, string> ToDictionary();
    }
}
