using Microsoft.AspNetCore.Mvc;
using EShop.Core.Models;

namespace EShop.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public SidebarViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            //            //you can do the access rights checking here by using session, user, and/or filter parameter
            var sidebars = new List<PageLink>();

            //            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Home));
            //            sidebars.Add(ModuleHelper.AddHeader("MAIN NAVIGATION"));
            //            sidebars.Add(ModuleHelper.AddTree("Administrator"));
            //            sidebars.Last().TreeChild = new List<PageLink>()
            //            {
            //                ModuleHelper.AddModule(ModuleHelper.Module.Role),
            //                ModuleHelper.AddModule(ModuleHelper.Module.User),
            //                //ModuleHelper.AddModule(ModuleHelper.Module.Register, Tuple.Create(1, 1, 1)),
            //            };

            //            sidebars.Add(ModuleHelper.AddTree("Configuration"));
            //            sidebars.Last().TreeChild = new List<PageLink>()
            //            {
            //                ModuleHelper.AddModule(ModuleHelper.Module.Section),
            //                ModuleHelper.AddModule(ModuleHelper.Module.Unit),
            //                ModuleHelper.AddModule(ModuleHelper.Module.ProductGroup),
            //ModuleHelper.AddModule(ModuleHelper.Module.Specimen),
            //ModuleHelper.AddModule(ModuleHelper.Module.Parameter),

            //                //ModuleHelper.AddModule(ModuleHelper.Module.Register, Tuple.Create(1, 1, 1)),
            //            };

            //            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Error, Tuple.Create(0, 0, 1)));
            //            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.About, Tuple.Create(0, 1, 0)));
            //            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Contact, Tuple.Create(1, 0, 0)));
            //            //sidebars.Add(ModuleHelper.AddTree("Account"));
            //            //sidebars.Last().TreeChild = new List<PageLink>()
            //            //{
            //            //    ModuleHelper.AddModule(ModuleHelper.Module.Login),
            //            //    ModuleHelper.AddModule(ModuleHelper.Module.Register, Tuple.Create(1, 1, 1)),
            //            //};

            //            //if (User.IsInRole("SuperAdmins"))
            //            //{
            //            //    sidebars.Add(ModuleHelper.AddTree("Administration"));
            //            //    sidebars.Last().TreeChild = new List<PageLink>()
            //            //    {
            //            //        ModuleHelper.AddModule(ModuleHelper.Module.Tenant),
            //            //        ModuleHelper.AddModule(ModuleHelper.Module.SuperAdmin),
            //            //        ModuleHelper.AddModule(ModuleHelper.Module.Role),
            //            //    };
            //            //    sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.UserLogs));
            //            //}


            return View(sidebars);
        }
    }
}
