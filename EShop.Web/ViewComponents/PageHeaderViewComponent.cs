using Microsoft.AspNetCore.Mvc;
using System;

namespace EShop.Web.ViewComponents
{
    public class PageHeaderViewComponent : ViewComponent
    {

        public PageHeaderViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            Tuple<string, string> header;
            
            if (ViewData["PageHeader"] == null)
            {
                header = Tuple.Create(string.Empty, string.Empty);
            }
            else
            {
                header = ViewData["PageHeader"] as Tuple<string, string>;
            }
            return View(header);
        }
    }
}
