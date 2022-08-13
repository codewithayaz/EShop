using Microsoft.AspNetCore.Mvc;
using EShop.Core.Models;

namespace EShop.Web.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        public BreadcrumbViewComponent()
        {

        }

        public IViewComponentResult Invoke(string filter)
        {
            List<PageLink> links = new List<PageLink>();

            string route = "";
            
            string area = "";
            string page = "";
            
            if (HttpContext.Request.RouteValues.Keys.Contains("area"))
            {
                area = HttpContext.Request.RouteValues["area"].ToString();
            }
            if (HttpContext.Request.RouteValues.Keys.Contains("page"))
            {
                page = HttpContext.Request.RouteValues["page"].ToString();
            }

            if (!string.IsNullOrWhiteSpace(area))
            {
                route += "/" + area;
            }

            if (!string.IsNullOrWhiteSpace(page))
            {
                var parts = page.Split('/').Where(x => !string.IsNullOrWhiteSpace(x) && x != "Index").ToList();
                if (parts.Count > 0)
                {
                    links.Add(new PageLink
                    {
                        Name = "Home",
                        URLPath = "/"
                    });
                    foreach (var part in parts)
                    {
                        route += "/" + part;
                        links.Add(new PageLink
                        {
                            Name = part,
                            URLPath = route
                        });
                    }

                }
            }
            
            return View(links);
        }

    }
}
