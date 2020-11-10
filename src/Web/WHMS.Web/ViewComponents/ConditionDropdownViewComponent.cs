namespace WHMS.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;

    public class ConditionDropdownViewComponent : ViewComponent
    {
        private readonly ICondiitonsService conditionsService;

        public ConditionDropdownViewComponent(ICondiitonsService conditionsService)
        {
            this.conditionsService = conditionsService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var conditions = this.conditionsService.GetAllConditions<ConditionViewModel>().
                OrderBy(x => x.Name).
                Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == id,
                });
            return this.View(conditions);
        }
    }
}
