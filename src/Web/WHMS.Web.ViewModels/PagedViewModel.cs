namespace WHMS.Web.ViewModels
{
    using System;

    public abstract class PagedViewModel
    {
        public int Page { get; set; }

        public int PagesCount { get; set; }

        public string NextDisabled => this.Page == this.PagesCount ? "disabled" : string.Empty;

        public string PreviousDisabled => this.Page == 1 ? "disabled" : string.Empty;

        public int FirstPage => this.PagesCount > 10 ? this.MidPage() - 4 : 1;

        public int LastPage => this.PagesCount > 10 ? this.MidPage() + 5 : this.PagesCount;

        private int MidPage()
        {
            if (this.Page - 5 < 1)
            {
                return 5;
            }
            else if (this.Page + 5 > this.PagesCount)
            {
                return this.PagesCount - 5;
            }

            return this.Page;
        }
    }
}
